using Dolhouse.Binary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Compression
{

    /// <summary>
    /// Yay0 Compression
    /// TODO: Add compression functionality + Document properly.
    /// </summary>
    public static class Yay0
    {

        /// <summary>
        /// Decompressing Yay0 compressed data. Full credits for this snippet goes to Daniel McCarthy:
        /// https://github.com/Daniel-McCarthy/Mr-Peeps-Compressor/blob/master/PeepsCompress/PeepsCompress/Algorithm%20Classes/YAY0.cs
        /// </summary>
        /// <param name="stream">Stream containing the Yay0 compressed data.</param>
        /// <returns>Decompressed data as a stream.</returns>
        public static Stream Decompress(Stream stream)
        {
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Define a list of bytes to hold the decompressed data.
            List<byte> output = new List<byte>();

            // Make sure the magic is "Yay0"
            if (br.ReadU32() != 1499560240)
            { throw new Exception("No valid Yay0 signature was found!"); }
            int decompressedLength = br.ReadS32();
            int compressedOffset = br.ReadS32();
            int decompressedOffset = br.ReadS32();
            int layoutBitsOffset;

            // Begin decompression.
            while (output.Count < decompressedLength)
            {
                // Define variable to hold the layout byte.
                byte LayoutByte = br.Read();
                // Define variable to hold the individual layout bits.
                BitArray LayoutBits = new BitArray(new byte[1] { LayoutByte });

                // Loop through the layout bits.
                for (int i = 7; i > -1 && (output.Count < decompressedLength); i--)
                {
                    // The next layout bit is 1. (Uncompressed data)
                    if (LayoutBits[i] == true)
                    {
                        // Yay0 Non-compressed Data Chunk:
                        // Add one byte from decompressedOffset to decompressedData.

                        // Define variable to hold the current layout bits offset.
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

                        // Advance the compressed data offset by 2 (Since we just read 2 bytes)
                        compressedOffset += 2;

                        // Get 4 upper bits of the first byte.
                        byte byte1Upper = (byte)((byte1 & 0x0F));

                        // Get 4 lower bits of the first byte.
                        byte byte1Lower = (byte)((byte1 & 0xF0) >> 4);

                        int finalOffset = ((byte1Upper << 8) | byte2) + 1;
                        int finalLength;

                        // Compressed chunk length is higher than 17.
                        if (byte1Lower == 0)
                        {
                            stream.Seek(decompressedOffset, SeekOrigin.Begin);
                            finalLength = br.Read() + 0x12;

                            // Advance the decompressed offset by one byte.
                            decompressedOffset++;
                        }
                        // Compressed chunk length is lower or equal to 17.
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
