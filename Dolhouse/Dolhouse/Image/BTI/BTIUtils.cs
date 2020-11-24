using Dolhouse.Binary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Image.BTI
{
    /// Many thanks to LordNed for code reference:
    /// https://github.com/LordNed/JStudio/
    /// And Sage-Of-Mirrors for much of this code:
    /// https://github.com/Sage-of-Mirrors/
    public static class BTIUtils
    {
        public static byte[] DecodeData(DhBinaryReader br, ushort width, ushort height, TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.I4:
                    return ReadI4(br, width, height);
                case TextureFormat.I8:
                    return ReadI8(br, width, height);
                case TextureFormat.IA4:
                    return ReadIA4(br, width, height);
                case TextureFormat.IA8:
                    return ReadIA8(br, width, height);
                case TextureFormat.RGB5A3:
                    return ReadRGB5A3(br, width, height);
                case TextureFormat.RGB565:
                    return ReadRGB565(br, width, height);
                case TextureFormat.RGBA32:
                    return ReadRGBA32(br, width, height);
                case TextureFormat.CMPR:
                    return ReadCMPR(br, width, height);
                default:
                    throw new NotImplementedException(string.Format("[BTI] {0} is not a supported texture format!", format));
            }
        }

        /// <summary>
        /// Method for reading I4 encoded data.
        /// </summary>
        /// <param name="br">BinaryReader to read with.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
        /// <returns>Texture as a stream containing RGBA data.</returns>
        private static byte[] ReadI4(DhBinaryReader br, ushort width, ushort height)
        {

            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int y = 0; y < (width / 8); y++)
            {

                // Loop through blocks x.
                for (int x = 0; x < (height / 8); x++)
                {

                    // Loop through pixels in block y.
                    for (int yx = 0; yx < 8; yx++)
                    {

                        // Loop through pixels in block x.
                        for (int xy = 0; xy < 8; xy += 2)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((y * 8 + yx >= height) || (x * 8 + xy >= width))
                            {

                                // Skip.
                                continue;
                            }

                            // Read current pixel.
                            byte pixel = br.Read();

                            // Calculate RGBA data.
                            byte r1 = (byte)(((pixel & 0xF0) >> 4) * 0x11);
                            byte g1 = (byte)(((pixel & 0xF0) >> 4) * 0x11);
                            byte b1 = (byte)(((pixel & 0xF0) >> 4) * 0x11);
                            byte a1 = 0xFF;
                            byte r2 = (byte)((pixel & 0x0F) * 0x11);
                            byte g2 = (byte)((pixel & 0x0F) * 0x11);
                            byte b2 = (byte)((pixel & 0x0F) * 0x11);
                            byte a2 = 0xFF;

                            // Calculate offset for decoded data.
                            uint offset = (uint)(4 * (width * ((y * 8) + yx) + (x * 8) + xy));

                            // Store decoded data in buffer.
                            decoded[offset + 0] = r1;
                            decoded[offset + 1] = g1;
                            decoded[offset + 2] = b1;
                            decoded[offset + 3] = a1;
                            decoded[offset + 4] = r2;
                            decoded[offset + 5] = g2;
                            decoded[offset + 6] = b2;
                            decoded[offset + 7] = a2;
                        }
                    }
                }
            }

            // Return the texture as RGBA.
            return decoded;
        }


        /// <summary>
        /// Method for reading IA4 encoded data.
        /// </summary>
        /// <param name="br">BinaryReader to read with.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
        /// <returns>Texture as a byte[] containing RGBA data.</returns>
        private static byte[] ReadIA4(DhBinaryReader br, ushort width, ushort height)
        {

            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int y = 0; y < (height / 4); y++)
            {

                // Loop through blocks x.
                for (int x = 0; x < (width / 8); x++)
                {

                    // Loop through pixels in block y.
                    for (int yx = 0; yx < 4; yx++)
                    {

                        // Loop through pixels in block x.
                        for (int xy = 0; xy < 8; xy++)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((x * 8 + xy >= width) || (y * 4 + yx >= height))
                            {

                                // Skip.
                                continue;
                            }

                            // Read current pixel.
                            byte value = br.Read();

                            // Calculate alpha and luminosity values.
                            byte alpha = (byte)(((value & 0xF0) >> 4) * 0x11);
                            byte luminosity = (byte)((value & 0x0F) * 0x11);

                            // Calculate offset for decoded data.
                            uint offset = (uint)(4 * (width * ((y * 4) + yx) + (x * 8) + xy));

                            // Store decoded data in buffer.
                            decoded[offset + 0] = luminosity;
                            decoded[offset + 1] = luminosity;
                            decoded[offset + 2] = luminosity;
                            decoded[offset + 3] = alpha;
                        }
                    }
                }
            }

            // Return the texture as RGBA.
            return decoded;
        }


        /// <summary>
        /// Method for reading I8 encoded data.
        /// </summary>
        /// <param name="br">BinaryReader to read with.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
        /// <returns>Texture as a stream containing RGBA data.</returns>
        private static byte[] ReadI8(DhBinaryReader br, ushort width, ushort height)
        {

            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int y = 0; y < (height / 4); y++)
            {

                // Loop through blocks y.
                for (int x = 0; x < (width / 8); x++)
                {

                    // Loop through pixels in block y.
                    for (int yx = 0; yx < 4; yx++)
                    {

                        // Loop through pixels in block y.
                        for (int xy = 0; xy < 8; xy++)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((x * 8 + xy >= width) || (y * 4 + yx >= height))
                            {

                                // Skip.
                                continue;
                            }

                            // Read current pixel.
                            byte data = br.Read();

                            // Calculate offset for decoded data.
                            uint offset = (uint)(4 * (width * ((y * 4) + yx) + (x * 8) + xy));

                            // Store decoded data in buffer.
                            decoded[offset + 0] = data;
                            decoded[offset + 1] = data;
                            decoded[offset + 2] = data;
                            decoded[offset + 3] = 0xFF;
                        }
                    }
                }
            }

            // Return the texture as RGBA.
            return decoded;
        }


        /// <summary>
        /// Method for reading IA8 encoded data.
        /// </summary>
        /// <param name="br">BinaryReader to read with.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
        /// <returns>Texture as a stream containing RGBA data.</returns>
        private static byte[] ReadIA8(DhBinaryReader br, ushort width, ushort height)
        {

            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int y = 0; y < (height / 4); y++)
            {

                // Loop through blocks x.
                for (int x = 0; x < (width / 4); x++)
                {

                    // Loop through pixels in block y.
                    for (int yx = 0; yx < 4; yx++)
                    {

                        // Loop through pixels in block x.
                        for (int xy = 0; xy < 4; xy++)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((x * 4 + xy >= width) || (y * 4 + yx >= height))
                            {

                                // Skip.
                                continue;
                            }

                            // Calculate offset for decoded data.
                            uint offset = (uint)(4 * (width * ((y * 4) + yx) + (x * 4) + xy));

                            // Read current pixel.
                            ushort data = br.ReadU16();

                            // Store decoded data in buffer.
                            decoded[offset + 3] = (byte)data;
                            decoded[offset + 2] = (byte)(data >> 8);
                            decoded[offset + 1] = (byte)(data >> 8);
                            decoded[offset + 0] = (byte)(data >> 8);
                        }
                    }
                }
            }

            // Return the texture as RGBA.
            return decoded;
        }

        private static byte[] ReadRGB565(DhBinaryReader br, uint width, uint height)
        {
            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int yBlock = 0; yBlock < (height / 4); yBlock++)
            {

                // Loop through blocks x.
                for (int xBlock = 0; xBlock < (width / 4); xBlock++)
                {

                    // Loop through pixels in block y.
                    for (int pY = 0; pY < 4; pY++)
                    {
                        // Loop through pixels in block x.
                        for (int pX = 0; pX < 4; pX++)
                        {
                            
                            // Determine if pixel is oob (x || y).
                            if ((xBlock * 4 + pX >= width) || (yBlock * 4 + pY >= height))
                            {
                                // Skip.
                                continue;
                            }

                            // Calculate offset for decoded data.
                            int offset = (int)(4 * (width * ((yBlock * 4) + pY) + (xBlock * 4) + pX));

                            // Read current pixel.
                            ushort pixel = br.ReadU16();

                            // Declare three bytes to hold our R, G, B components.
                            byte r = (byte)((((pixel & 0xF100) >> 11) << 3) | (((pixel & 0xF100) >> 11) >> 2));
                            byte g = (byte)((((pixel & 0x7E0) >> 5) << 2) | (((pixel & 0x7E0) >> 5) >> 4));
                            byte b = (byte)(((pixel & 0x1F) << 3) | ((pixel & 0x1F) >> 2));

                            // Store decoded data in buffer.
                            decoded[offset + 0] = b;
                            decoded[offset + 1] = g;
                            decoded[offset + 2] = r;
                            decoded[offset + 3] = 0xFF;
                        }
                    }
                }
            }

            return decoded;
        }

        /// <summary>
        /// Method for reading RBG5A3 encoded data.
        /// </summary>
        /// <param name="br">BinaryReader to read with.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
        /// <returns>Texture as a stream containing RGBA data.</returns>
        private static byte[] ReadRGB5A3(DhBinaryReader br, ushort width, ushort height)
        {

            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int y = 0; y < (height / 4); y++)
            {

                // Loop through blocks x.
                for (int x = 0; x < (width / 4); x++)
                {

                    // Loop through pixels in block y.
                    for (int yx = 0; yx < 4; yx++)
                    {

                        // Loop through pixels in block x.
                        for (int xy = 0; xy < 4; xy++)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((x * 4 + xy >= width) || (y * 4 + yx >= height))
                            {

                                // Skip.
                                continue;
                            }

                            // Read current pixel.
                            ushort pixel = br.ReadU16();

                            // Calculate offset for decoded data.
                            int offset = 4 * (width * ((y * 4) + yx) + (x * 4) + xy);

                            // Declare four bytes to hold our R, G, B, A components.
                            byte r, g, b, a;

                            // Determine if there is any alpha bits.
                            if ((pixel & 0x8000) == 0x8000)
                            {

                                // Alpha component. (No Alpha)
                                a = 0xFF;

                                // Red component.
                                r = (byte)((pixel & 0x7C00) >> 10);
                                r = (byte)((r << 3) | (r >> 2));

                                // Green component.
                                g = (byte)((pixel & 0x3E0) >> 5);
                                g = (byte)((g << 3) | (g >> 2));

                                // Blue component.
                                b = (byte)(pixel & 0x1F);
                                b = (byte)((b << 3) | (b >> 2));
                            }
                            else
                            {

                                // Alpha component.
                                a = (byte)((pixel & 0x7000) >> 12);
                                a = (byte)((a << 2) | (a << 2) | (a >> 1));

                                // Red component.
                                r = (byte)((pixel & 0xF00) >> 8);
                                r = (byte)((r << 4) | r);

                                // Green component.
                                g = (byte)((pixel & 0xF0) >> 4);
                                g = (byte)((g << 4) | g);

                                // Blue component.
                                b = (byte)(pixel & 0xF);
                                b = (byte)((b << 4) | b);
                            }

                            // Store decoded data in buffer.
                            decoded[offset + 0] = a;
                            decoded[offset + 1] = b;
                            decoded[offset + 2] = g;
                            decoded[offset + 3] = r;
                        }
                    }
                }
            }

            // Return the texture as RGBA.
            return decoded;
        }


        /// <summary>
        /// Method for reading RGBA32 encoded data.
        /// </summary>
        /// <param name="br">BinaryReader to read with.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
        /// <returns>Texture as a stream containing RGBA data.</returns>
        private static byte[] ReadRGBA32(DhBinaryReader br, ushort width, ushort height)
        {

            // Buffer to store our decoded data.
            byte[] decoded = new byte[width * height * 4];

            // Loop through blocks y.
            for (int y = 0; y < (height / 4); y++)
            {

                // Loop through blocks x.
                for (int x = 0; x < (width / 4); x++)
                {

                    // Loop through subblock 1 y.
                    for (int yx = 0; yx < 4; yx++)
                    {

                        // Loop through subblock 1 x.
                        for (int xy = 0; xy < 4; xy++)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((x * 4 + xy >= width) || (y * 4 + yx >= height))
                            {

                                // Skip.
                                continue;
                            }

                            // Calculate offset for decoded data.
                            uint offset = (uint)(4 * (width * ((y * 4) + yx) + (x * 4) + xy));

                            // Store decoded Alpha, Red subblock data in buffer.
                            decoded[offset + 3] = br.Read();
                            decoded[offset + 2] = br.Read();
                        }
                    }

                    // Loop through subblock 2 y.
                    for (int yx = 0; yx < 4; yx++)
                    {

                        // Loop through subblock 2 x.
                        for (int xy = 0; xy < 4; xy++)
                        {

                            // Determine if pixel is oob (x || y).
                            if ((x * 4 + xy >= width) || (y * 4 + yx >= height))
                            {

                                // Skip.
                                continue;
                            }

                            // Calculate offset for decoded data.
                            uint offset = (uint)(4 * (width * ((y * 4) + yx) + (x * 4) + xy));

                            // Store decoded Green, Blue subblock data in buffer.
                            decoded[offset + 1] = br.Read();
                            decoded[offset + 0] = br.Read();
                        }
                    }

                }
            }

            // Return the texture as RGBA.
            return decoded;
        }

        /// <summary>
        /// Convert a RGB565 encoded pixel (two bytes in length) to a RGBA (4 byte in length)
        /// pixel.
        /// </summary>
        /// <param name="pixel">RGB565 encoded pixel.</param>
        /// <param name="dest">Destination array for RGBA pixel.</param>
        /// <param name="destOffset">Offset into destination array to write RGBA pixel.</param>
        private static void RGB565ToRGBA8(ushort pixel, ref byte[] dest, int destOffset)
        {

            // Declare and calculate three bytes to for our R, G, B components.
            byte r = (byte)((pixel & 0xF100) >> 11);
            byte g = (byte)((pixel & 0x7E0) >> 5);
            byte b = (byte)((pixel & 0x1F));

            r = (byte)((r << (8 - 5)) | (r >> (10 - 8)));
            g = (byte)((g << (8 - 6)) | (g >> (12 - 8)));
            b = (byte)((b << (8 - 5)) | (b >> (10 - 8)));

            dest[destOffset] = b;
            dest[destOffset + 1] = g;
            dest[destOffset + 2] = r;
            dest[destOffset + 3] = 0xFF;
        }

        private static byte[] ReadCMPR(DhBinaryReader br, uint width, uint height)
        {
            //Decode S3TC1
            byte[] buffer = new byte[width * height * 4];

            for (int y = 0; y < height / 4; y += 2)
            {
                for (int x = 0; x < width / 4; x += 2)
                {
                    for (int yx = 0; yx < 2; ++yx)
                    {
                        for (int xy = 0; xy < 2; ++xy)
                        {
                            if (4 * (x + xy) < width && 4 * (y + yx) < height)
                            {
                                byte[] fileData = br.Read(8);
                                //Array.Reverse(fileData);
                                Buffer.BlockCopy(fileData, 0, buffer, (int)(8 * ((y + yx) * width / 4 + x + xy)), 8);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < width * height / 2; i += 8)
            {
                // Micro swap routine needed
                Swap(ref buffer[i], ref buffer[i + 1]);
                Swap(ref buffer[i + 2], ref buffer[i + 3]);

                buffer[i + 4] = S3TC1ReverseByte(buffer[i + 4]);
                buffer[i + 5] = S3TC1ReverseByte(buffer[i + 5]);
                buffer[i + 6] = S3TC1ReverseByte(buffer[i + 6]);
                buffer[i + 7] = S3TC1ReverseByte(buffer[i + 7]);
            }

            //Now decompress the DXT1 data within it.
            return DecompressDxt1(buffer, width, height);
        }

        private static void Swap(ref byte b1, ref byte b2)
        {
            byte tmp = b1; b1 = b2; b2 = tmp;
        }

        private static ushort Read16Swap(byte[] data, uint offset)
        {
            return (ushort)((Buffer.GetByte(data, (int)offset + 1) << 8) | Buffer.GetByte(data, (int)offset));
        }

        private static uint Read32Swap(byte[] data, uint offset)
        {
            return (uint)((Buffer.GetByte(data, (int)offset + 3) << 24) | (Buffer.GetByte(data, (int)offset + 2) << 16) | (Buffer.GetByte(data, (int)offset + 1) << 8) | Buffer.GetByte(data, (int)offset));
        }

        private static byte S3TC1ReverseByte(byte b)
        {
            byte b1 = (byte)(b & 0x3);
            byte b2 = (byte)(b & 0xC);
            byte b3 = (byte)(b & 0x30);
            byte b4 = (byte)(b & 0xC0);

            return (byte)((b1 << 6) | (b2 << 2) | (b3 >> 2) | (b4 >> 6));
        }

        private static byte[] DecompressDxt1(byte[] src, uint width, uint height)
        {

            byte[] decoded = new byte[width * height * 4];

            uint offset = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    ushort color1 = Read16Swap(src, offset);
                    ushort color2 = Read16Swap(src, offset + 2);
                    uint bits = Read32Swap(src, offset + 4);
                    offset += 8;

                    byte[][] ColorTable = new byte[4][];
                    for (int i = 0; i < 4; i++)
                    {
                        ColorTable[i] = new byte[4];
                    }

                    RGB565ToRGBA8(color1, ref ColorTable[0], 0);
                    RGB565ToRGBA8(color2, ref ColorTable[1], 0);

                    if (color1 > color2)
                    {
                        ColorTable[2][0] = (byte)((2 * ColorTable[0][0] + ColorTable[1][0]) / 3);
                        ColorTable[2][1] = (byte)((2 * ColorTable[0][1] + ColorTable[1][1]) / 3);
                        ColorTable[2][2] = (byte)((2 * ColorTable[0][2] + ColorTable[1][2]) / 3);
                        ColorTable[2][3] = 0xFF;

                        ColorTable[3][0] = (byte)((ColorTable[0][0] + 2 * ColorTable[1][0]) / 3);
                        ColorTable[3][1] = (byte)((ColorTable[0][1] + 2 * ColorTable[1][1]) / 3);
                        ColorTable[3][2] = (byte)((ColorTable[0][2] + 2 * ColorTable[1][2]) / 3);
                        ColorTable[3][3] = 0xFF;
                    }
                    else
                    {
                        ColorTable[2][0] = (byte)((ColorTable[0][0] + ColorTable[1][0]) / 2);
                        ColorTable[2][1] = (byte)((ColorTable[0][1] + ColorTable[1][1]) / 2);
                        ColorTable[2][2] = (byte)((ColorTable[0][2] + ColorTable[1][2]) / 2);
                        ColorTable[2][3] = 0xFF;

                        ColorTable[3][0] = (byte)((ColorTable[0][0] + 2 * ColorTable[1][0]) / 3);
                        ColorTable[3][1] = (byte)((ColorTable[0][1] + 2 * ColorTable[1][1]) / 3);
                        ColorTable[3][2] = (byte)((ColorTable[0][2] + 2 * ColorTable[1][2]) / 3);
                        ColorTable[3][3] = 0x00;
                    }


                    for (int yy = 0; yy < 4; ++yy)
                    {
                        for (int xx = 0; xx < 4; ++xx)
                        {
                            if (((x + xx) < width) && ((y + yy) < height))
                            {
                                int di = (int)(4 * ((y + yy) * width + x + xx));
                                decoded[di + 0] = ColorTable[bits & 0x3][0];
                                decoded[di + 1] = ColorTable[bits & 0x3][1];
                                decoded[di + 2] = ColorTable[bits & 0x3][2];
                                decoded[di + 3] = ColorTable[bits & 0x3][3];
                            }

                            // Shift bits by two.
                            bits >>= 2;
                        }
                    }
                }
            }

            return decoded;
        }
    }
}
