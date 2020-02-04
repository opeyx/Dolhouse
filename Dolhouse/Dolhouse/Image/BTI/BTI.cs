using Dolhouse.Binary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Dolhouse.Image.BTI
{

    /// <summary>
    /// (B)inary (T)exture (I)mage
    /// </summary>
    public class BTI
    {

        #region Properties

        /// <summary>
        /// Texture Format.
        /// </summary>
        public TextureFormat Format { get; set; }

        /// <summary>
        /// Alpha Flag.
        /// (0 = No Alpha, 0 <= Alpha)
        /// </summary>
        public byte AlphaFlag { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// Repeat, clamp or mirror wraps
        /// for U projection maps.
        /// </summary>
        public WrapMode WrapS { get; set; }

        /// <summary>
        /// Repeat, clamp or mirror wraps
        /// for V projection maps.
        /// </summary>
        public WrapMode WrapT { get; set; }

        /// <summary>
        /// Palette Format.
        /// </summary>
        public PaletteFormat PaletteFormat { get; set; }

        /// <summary>
        /// Amount of Palettes.
        /// </summary>
        public ushort PaletteCount { get; set; }

        /// <summary>
        /// Offset To Palette Data. (Absolute)
        /// </summary>
        public uint PaletteOffset { get; set; }

        /// <summary>
        /// Unknown 1.
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Minification Filter Mode.
        /// </summary>
        public FilterMode MinFilterMode { get; set; }

        /// <summary>
        /// Magnification Filter Mode.
        /// </summary>
        public FilterMode MagFilterMode { get; set; }

        /// <summary>
        /// Min LOD.
        /// Fixed point number, 1/8 = Conversion.
        /// (Thanks to @Sage-Of-Mirrors)
        /// </summary>
        public ushort MinLOD { get; set; }

        /// <summary>
        /// Mag LOD.
        /// Fixed point number, 1/8 = Conversion.
        /// (Thanks to @Sage-Of-Mirrors)
        /// </summary>
        public byte MagLOD { get; set; }

        /// <summary>
        /// Mip-Map Count.
        /// </summary>
        public byte MipMapCount { get; set; }

        /// <summary>
        /// Lod Bias.
        /// Fixed point number, 1/100 = conversion.
        /// (Thanks to @Sage-Of-Mirrors)
        /// </summary>
        public ushort LodBias { get; set; }

        /// <summary>
        /// Data Offset.
        /// </summary>
        public uint DataOffset { get; set; }

        /// <summary>
        /// Image Data.
        /// </summary>
        public byte[] Data { get; set; }

        #endregion


        /// <summary>
        /// Reads BTI using a BinaryReader. (BinTexture Format)
        /// </summary>
        /// <param name="br">The BinaryReader to use.</param>
        public BTI(DhBinaryReader br, uint textureHeader)
        {

            // Set Width.
            Width = br.ReadU16();

            // Set Height.
            Height = br.ReadU16();

            // Set Texture Format.
            Format = (TextureFormat)br.Read();

            // Set Alpha Flag.
            AlphaFlag = br.Read();

            // Set WrapS.
            WrapS = WrapMode.ClampToEdge;

            // Set WrapT.
            WrapT = WrapMode.ClampToEdge;

            // Set Palette Format.
            PaletteFormat = PaletteFormat.RGB565;

            // Set Palette Count.
            PaletteCount = 0;

            // Set Palette Offset.
            PaletteOffset = 0;

            // Set Unknown 1. (Padding?)
            Unknown1 = 0;

            // Set MinFilterMode.
            MinFilterMode = FilterMode.Linear;

            // Set MagFilterMode.
            MagFilterMode = FilterMode.Linear;

            // Set MinLOD.
            MinLOD = 0;

            // Set MagLOD.
            MagLOD = 1;

            // Set MipMap Count.
            MipMapCount = 0;

            // Set LodBias.
            LodBias = 0;

            // Skip 2 bytes of padding.
            br.Skip(2);

            // Set Data Offset.
            DataOffset = br.ReadU32();

            // Read data.
            Data = br.ReadAt(textureHeader + DataOffset, (Width * Height) / 2);
        }

        /// <summary>
        /// Reads BTI from a stream.
        /// </summary>
        /// <param name="stream">The stream containing the BTI data.</param>
        public BTI(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read Texture Format.
            Format = (TextureFormat)br.Read();

            // Read Alpha Flag.
            AlphaFlag = br.Read();

            // Read Width.
            Width = br.ReadU16();

            // Read Height.
            Height = br.ReadU16();

            // Read WrapS.
            WrapS = (WrapMode)br.Read();

            // Read WrapT.
            WrapT = (WrapMode)br.Read();

            // Read Palette Format.
            PaletteFormat = (PaletteFormat)br.ReadU16();

            // Read Palette Count.
            PaletteCount = br.ReadU16();

            // Read Palette Offset.
            PaletteOffset = br.ReadU32();

            // Read Unknown 1.
            Unknown1 = br.ReadU32();

            // Read MinFilterMode.
            MinFilterMode = (FilterMode)br.Read();

            // Read MagFilterMode.
            MagFilterMode = (FilterMode)br.Read();

            // Read MinLOD.
            MinLOD = br.ReadU16();

            // Read MagLOD.
            MagLOD = br.Read();

            // Read MipMap Count.
            MipMapCount = br.Read();

            // Read LodBias.
            LodBias = br.ReadU16();

            // Read DataOffset.
            DataOffset = br.ReadU32();

            // Read data.
            Data = br.Read((Width * Height) / 2);

            // Read Data.
            //Data = br.ReadAt(DataOffset, (Width * Height) / 4);
        }

        /// <summary>
        /// Method for converting the BTI into a Bitmap.
        /// </summary>
        /// <returns>The BTI as a Bitmap.</returns>
        public Bitmap ToBitmap(DhBinaryReader br)
        {

            // This is not implemented yet.
            throw new NotImplementedException();

        }

        /// <summary>
        /// Method for converting the Bitmap into a BTI.
        /// </summary>
        /// <returns>The Bitmap as BTI.</returns>
        public byte[] ToBTI()
        {

            // This is not implemented yet.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a stream from this BTI.
        /// </summary>
        /// <returns>The BTI as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our BTI data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write Texture Format.
            bw.Write((byte)Format);

            // Write Alpha Flag.
            bw.Write(AlphaFlag);

            // Write Width.
            bw.WriteU16(Width);

            // Write Height.
            bw.WriteU16(Height);

            // Write WrapS.
            bw.Write((byte)WrapS);

            // Write WrapT.
            bw.Write((byte)WrapT);

            // Write PaletteFormat.
            bw.WriteU16((ushort)PaletteFormat);

            // Write PaletteCount.
            bw.WriteU16(PaletteCount);

            // Write PaletteOffset.
            bw.WriteU32(PaletteOffset);

            // Write Unknown1.
            bw.WriteU32(Unknown1);

            // Write MagFilterMode.
            bw.Write((byte)MagFilterMode);

            // Write MinFilterMode.
            bw.Write((byte)MinFilterMode);

            // Write Min LOD.
            bw.WriteU16(MinLOD);

            // Write Mag LOD.
            bw.Write(MagLOD);

            // Write MipMap Count.
            bw.Write(MipMapCount);

            // Write LodBias.
            bw.WriteU16(LodBias);

            // Write Data Offset.
            bw.WriteU32(DataOffset);

            // Write Data.
            bw.Write(ToBTI());

            // Return the BTI a as stream.
            return stream;
        }
    }
}
