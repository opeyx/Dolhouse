using Dolhouse.Binary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dolhouse.Compression
{

    /// <summary>
    /// Yay0 Compression
    /// </summary>
    public static class Yay0
    {

        /// <summary>
        /// Compressing data with Yay0. Full credits for this snippet goes to Cuyler36:
        /// https://github.com/Cuyler36/GCNToolKit/blob/master/GCNToolKit/Formats/Compression/Yay0.cs
        /// </summary>
        /// <param name="stream">Stream containing the data to Yay0 compress.</param>
        /// <returns>Compressed data as a stream.</returns>
        public static Stream Compress(Stream stream)
        {

            // Define byte array to hold our data.
            byte[] data;

            // Temporary create a memory stream, dispose it after.
            using (MemoryStream ms = new MemoryStream())
            {

                // Copy the contents of the stream into the memory stream.
                stream.CopyTo(ms);

                // Set the byte array to the content of the memory stream.
                data = ms.ToArray();
            }

            const int OFSBITS = 12;
            int decPtr = 0;

            // Setup mask buffer related variables.
            uint maskMaxSize = (uint)(data.Length + 32) >> 3; // 1 bit per byte
            uint maskBitCount = 0, mask = 0;
            uint[] maskBuffer = new uint[maskMaxSize / 4];
            int maskPtr = 0;

            // Initialize the links buffer related variables.
            uint linkMaxSize = (uint)data.Length / 2;
            ushort linkOffset = 0;
            ushort[] linkBuffer = new ushort[linkMaxSize];
            int linkPtr = 0;
            ushort minCount = 3, maxCount = 273;

            // Initialize the chunk buffer related variables.
            uint chunkMaxSize = (uint)data.Length;
            byte[] chunkBuffer = new byte[chunkMaxSize];
            int chunkPtr = 0;

            int windowPtr = decPtr;
            int windowLen = 0, length, maxlen;

            // Loop through the data we're going to compress.
            while (decPtr < data.Length)
            {

                if (windowLen >= (1 << OFSBITS))
                {
                    windowLen = windowLen - (1 << OFSBITS);
                    windowPtr = decPtr - windowLen;
                }

                if ((data.Length - decPtr) < maxCount)
                    maxCount = (ushort)(data.Length - decPtr);

                // Scan through the window.
                maxlen = 0;
                for (int i = 0; i < windowLen; i++)
                {

                    for (length = 0; length < (windowLen - i) && length < maxCount; length++)
                    {
                        if (data[decPtr + length] != data[windowPtr + length + i]) break;
                    }

                    if (length > maxlen)
                    {
                        maxlen = length;
                        linkOffset = (ushort)(windowLen - i);
                    }
                }
                length = maxlen;

                mask <<= 1;
                if (length >= minCount) // Add Link
                {

                    ushort link = (ushort)((linkOffset - 1) & 0x0FFF);

                    if (length < 18)
                    {
                        link |= (ushort)((length - 2) << 12);
                    }
                    else
                    {
                        // Store the current count as a chunk.
                        chunkBuffer[chunkPtr++] = (byte)(length - 18);
                    }

                    linkBuffer[linkPtr++] = (ushort)((link << 8) | (link >> 8));
                    decPtr += length;
                    windowLen += length;
                }
                else
                {

                    // Add single byte, increase Window.
                    chunkBuffer[chunkPtr++] = data[decPtr++];
                    windowLen++;
                    mask |= 1;
                }

                maskBitCount++;
                if (maskBitCount == 32)
                {
                    // Store the current mask in the mask buffer.
                    maskBuffer[maskPtr] = ((mask << 24) | ((mask >> 24) & 0xFF) | ((mask & 0xFF00) << 8) | ((mask >> 8) & 0xFF00));
                    maskPtr++;
                    maskBitCount = 0;
                }
            }

            // Flush the current mask from the mask buffer.
            if (maskBitCount > 0)
            {
                mask <<= (int)(32 - maskBitCount);

                // Store the current mask in the mask buffer.
                maskBuffer[maskPtr] = ((mask << 24) | ((mask >> 24) & 0xFF) | ((mask & 0xFF00) << 8) | ((mask >> 8) & 0xFF00));
                maskPtr++;
            }

            // Calculate the size of each section.
            uint maskSize = (uint)maskPtr * sizeof(uint);
            uint linkSize = (uint)linkPtr * sizeof(ushort);
            uint chunkSize = (uint)chunkPtr * sizeof(byte);

            // Calculate the total size of the compressed file.
            uint encodedSize = 0x10 + maskSize + linkSize + chunkSize;

            // Setup a buffer we'll write to.
            byte[] buffer = new byte[encodedSize];

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(new MemoryStream(buffer), DhEndian.Big);

            // Calculate the offset for each section.
            uint hdrLinkOffset = 0x10 + maskSize;
            uint hdrChunkOffset = hdrLinkOffset + linkSize;

            // Write header.
            bw.WriteStr("Yay0");

            // Write offsets.
            bw.WriteU32s(new uint[] { (uint)data.Length, hdrLinkOffset, hdrChunkOffset });

            // Copy each section into the buffer.
            Buffer.BlockCopy(maskBuffer, 0, buffer, 0x10, (int)maskSize);
            Buffer.BlockCopy(linkBuffer, 0, buffer, (int)hdrLinkOffset, (int)linkSize);
            Buffer.BlockCopy(chunkBuffer, 0, buffer, (int)hdrChunkOffset, (int)chunkSize);

            // Return the compressed data.
            return new MemoryStream(buffer);
        }

        /// <summary>
        /// Decompressing data with Yay0. Full credits for this snippet goes to Daniel McCarthy:
        /// https://github.com/Daniel-McCarthy/Mr-Peeps-Compressor/blob/master/PeepsCompress/PeepsCompress/Algorithm%20Classes/YAY0.cs
        /// </summary>
        /// <param name="stream">Stream containing the Yay0 compressed data.</param>
        /// <returns>Decompressed data as a stream.</returns>
        public static Stream Decompress(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Define a list of bytes to hold the decompressed data.
            List<byte> output = new List<byte>();

            // Make sure the magic is "Yay0".
            if (br.ReadU32() != 1499560240)
            { throw new InvalidDataException("No valid Yay0 signature was found!"); }

            // Read the length of the decompressed data.
            int decompressedLength = br.ReadS32();

            // Read the compressed data offset.
            int compressedOffset = br.ReadS32();

            // Read the non-compressed data offset.
            int decompressedOffset = br.ReadS32();

            // Define variable to hold each layout bits' offset.
            int layoutBitsOffset;

            // Begin decompression.
            while (output.Count < decompressedLength)
            {

                // Define variable to hold the layout byte.
                byte layoutByte = br.Read();

                // Define variable to hold the individual layout bits.
                BitArray layoutBits = new BitArray(new byte[1] { layoutByte });

                // Loop through the layout bits.
                for (int i = 7; i > -1 && (output.Count < decompressedLength); i--)
                {
                    // The next layout bit is 1. (Uncompressed data)
                    if (layoutBits[i] == true)
                    {
                        // Yay0 Non-compressed Data Chunk:
                        // Add one byte from decompressedOffset to decompressedData.

                        // Set variable to the current layout bits' offset.
                        layoutBitsOffset = (int)stream.Position;

                        // Seek to the compressed data offset.
                        stream.Seek(decompressedOffset, SeekOrigin.Begin);

                        // Read a byte and add it to the list of output bytes.
                        output.Add(br.Read());

                        // Advance the decompressed offset by one byte.
                        decompressedOffset++;

                        // Return to the current layout bits offset.
                        stream.Seek(layoutBitsOffset, SeekOrigin.Begin);

                    }
                    // The next layout bit is 0. (Compressed data)
                    else
                    {
                        // Yay0 Compressed Data Chunk:
                        // Total length is 2 bytes.
                        // 4 bits = Length
                        // 12 bits = Offset

                        // Define variable to hold the current offset.
                        layoutBitsOffset = (int)stream.Position;

                        // Seek to the compressed data offset.
                        stream.Seek(compressedOffset, SeekOrigin.Begin);

                        // Read the first byte of the compressed data.
                        byte byte1 = br.Read();

                        // Read the second byte of the compressed data.
                        byte byte2 = br.Read();

                        // Advance the compressed data offset by 2.
                        compressedOffset += 2;

                        // Get 4 upper bits of the first byte.
                        byte byte1Upper = (byte)((byte1 & 0x0F));

                        // Get 4 lower bits of the first byte.
                        byte byte1Lower = (byte)((byte1 & 0xF0) >> 4);

                        int finalOffset = ((byte1Upper << 8) | byte2) + 1;
                        int finalLength;

                        // Compressed chunk length is larger than 17.
                        if (byte1Lower == 0)
                        {
                            stream.Seek(decompressedOffset, SeekOrigin.Begin);
                            finalLength = br.Read() + 0x12;

                            // Advance the decompressed offset by one byte.
                            decompressedOffset++;
                        }
                        // Compressed chunk length is smaller or equal to 17.
                        else
                        {
                            // The data chunk length is equals to the first byte's 4 lower bits + 2.
                            finalLength = byte1Lower + 2;
                        }

                        // Add data for finalLength iterations.
                        for (int j = 0; j < finalLength; j++)
                        {
                            // Add byte at offset (fileSize - finalOffset) to file.
                            output.Add(output[output.Count - finalOffset]);
                        }

                        // Return to the current layout bits offset.
                        stream.Seek(layoutBitsOffset, SeekOrigin.Begin);
                    }
                }
            }

            // Return decompressed data.
            return new MemoryStream(output.ToArray());
        }
    }
}
