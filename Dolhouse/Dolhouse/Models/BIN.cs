using Dolhouse.Binary;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Models
{

    /// <summary>
    /// (Bin)ary Model
    /// </summary>
    public class BIN
    {

        #region Properties


        /// <summary>
        /// Bin version. Should read either 0x01 or 0x02.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Bin model name.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Bin offsets. There's always 21 offsets!
        /// </summary>
        public List<uint> Offsets { get; set; }

        #endregion

        /// <summary>
        /// Reads BIN from a stream.
        /// </summary>
        /// <param name="stream">The stream containing the BIN data.</param>
        public BIN(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read bin version.
            Version = br.Read();

            // Make sure the bin version is either 0x01 or 0x02.
            if (Version == 0x00 || Version > 0x02 )
            { throw new Exception(string.Format("{0} is not a valid bin version!", Version.ToString())); }

            // Read bin model name.
            ModelName = br.ReadStr(11);

            // Define a new list to hold the BIN's offsets.
            Offsets = new List<uint>();

            // Loop through the BIN's offsets.
            for (int i = 0; i < 21; i++)
            {
                // Read offset and add it to the offsets list.
                Offsets.Add(br.ReadU32());
            }
        }

        /// <summary>
        /// Creates a stream from this BIN.
        /// </summary>
        /// <returns>The BIN as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our BIN data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write BIN's Version.
            bw.Write(Version);

            // Write BIN's Model Name.
            bw.WriteStr(ModelName);

            // Loop through BIN's Offsets.
            for (int i = 0; i < Offsets.Count; i++)
            {
                // Write BIN Offset.
                bw.WriteU32(Offsets[i]);
            }

            // TODO: WRITE THE REST OF BIN.

            return stream;
        }
    }

    public class BinTexture
    {

        #region Properties

        /// <summary>
        /// Texture Width. Always power of 2.
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// Texture Height. Always power of 2.
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// Texture Format. Same as BTI/TPL; usually S3TC1.
        /// </summary>
        public byte Format { get; set; }

        /// <summary>
        /// Unknown 1. Always either 0x20 or 0x10, rarely 0x08 (Flags?)
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2. Always 0, padding?
        /// </summary>
        public ushort Unknown2 { get; set; }

        /// <summary>
        /// Texture Data Offset. Offset to texture padding. (Relative to texture header list)
        /// </summary>
        public uint DataOffset { get; set; }
        
        /// <summary>
        /// Texture Data. Raw BTI data will be stored here.
        /// </summary>
        public byte[] Data { get; set; }

        #endregion


        /// <summary>
        /// Read a single texture from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinTexture(DhBinaryReader br)
        {

            // Read texture width.
            Width = br.ReadU16();

            // Read texture height.
            Height = br.ReadU16();

            // Read texture format.
            Format = br.ReadU8();

            // Read texture unknown 1. (Flags?)
            Unknown1 = br.ReadU8();

            // Read texture unknown 2. (Padding)
            Unknown2 = br.ReadU16();

            // Read texture data offset.
            DataOffset = br.ReadU32();
        }


        /// <summary>
        /// Write a single texture to stream.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write texture width.
            bw.WriteU16(Width);

            // Write texture width.
            bw.WriteU16(Height);

            // Write texture format.
            bw.WriteU8(Format);

            // Write texture unknown 1. (Flags?)
            bw.WriteU8(Unknown1);

            // Write texture unknown 2. (Padding)
            bw.WriteU16(Unknown2);

            // Write texture data offset.
            bw.WriteU32(DataOffset);
        }
    }

    public class BinMaterial
    {

        #region Properties

        /// <summary>
        /// Index.
        /// </summary>
        public short Index { get; set; }

        /// <summary>
        /// Unknown 1. Always -1, unused index?
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// U-Wrapping. 0 = Clamp, 1 = Repeat, 2 = Mirror.
        /// </summary>
        public byte WrapU { get; set; }

        /// <summary>
        /// V-Wrapping. 0 = Clamp, 1 = Repeat, 2 = Mirror.
        /// </summary>
        public byte WrapV { get; set; }

        /// <summary>
        /// Unknown 2. (Flags?)
        /// </summary>
        public short Unknown2 { get; set; }

        /// <summary>
        /// Unknown 3. (Padding)
        /// </summary>
        public int[] Unknown3 { get; set; }

        #endregion


        /// <summary>
        /// Read a single material from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinMaterial(DhBinaryReader br)
        {

            // Read Index.
            Index = br.ReadS16();

            // Read Unknown 1. (Unused index?)
            Unknown1 = br.ReadS16();

            // Read U-Wrapping.
            WrapU = br.Read();

            // Read V-Wrapping.
            WrapV = br.Read();

            // Read Unknown 2. (Flags?)
            Unknown2 = br.ReadS16();

            // Define a array to hold the unknown values.
            Unknown3 = new int[3];

            // Loop through the unknown values.
            for (int i = 0; i < Unknown3.Length; i++)
            {
                // Write the current unknown value.
                Unknown3[i] = br.ReadS32();
            }
        }

        /// <summary>
        /// Write a single material to stream.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write material index.
            bw.WriteS16(Index);

            // Write material unknown 1.
            bw.WriteS16(Unknown1);

            // Write material U-wrapping.
            bw.Write(WrapU);

            // Write material V-wrapping.
            bw.Write(WrapV);

            // Write material unknown 2.
            bw.WriteS16(Unknown2);

            // Loop through the material unknown 3 values.
            for (int i = 0; i < Unknown3.Length; i++)
            {
                // Write the current material unknown 3 value.
                bw.WriteS32(Unknown3[i]);
            }
        }

    }
}
