using Dolhouse.Binary;
using Dolhouse.Image.BTI;

namespace Dolhouse.Models.Bin
{
    
    /// <summary>
    /// Material
    /// </summary>
    public class Material
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
        public WrapMode WrapU { get; set; }

        /// <summary>
        /// V-Wrapping. 0 = Clamp, 1 = Repeat, 2 = Mirror.
        /// </summary>
        public WrapMode WrapV { get; set; }

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
        /// Initialize a new material.
        /// </summary>
        public Material()
        {

            // Set Index.
            Index = 0;

            // Set Unknown 1. (Unused index?)
            Unknown1 = -1;

            // Set U-Wrapping.
            WrapU = WrapMode.ClampToEdge;

            // Set V-Wrapping.
            WrapV = WrapMode.ClampToEdge;

            // Set Unknown 2. (Flags?)
            Unknown2 = 0;

            // Set Unknown 3.
            Unknown3 = new int[3];
        }

        /// <summary>
        /// Read a single material from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public Material(DhBinaryReader br)
        {

            // Read Index.
            Index = br.ReadS16();

            // Read Unknown 1. (Unused index?)
            Unknown1 = br.ReadS16();

            // Read U-Wrapping.
            WrapU = (WrapMode)br.Read();

            // Read V-Wrapping.
            WrapV = (WrapMode)br.Read();

            // Read Unknown 2. (Flags?)
            Unknown2 = br.ReadS16();

            // Read Unknown 3.
            Unknown3 = br.ReadS32s(3);
        }

        /// <summary>
        /// Write a single material with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write Index.
            bw.WriteS16(Index);

            // Write Unknown 1. (Unused index?)
            bw.WriteS16(Unknown1);

            // Write U-Wrapping.
            bw.Write(WrapU);

            // Write V-Wrapping.
            bw.Write(WrapV);

            // Write Unknown 2. (Flags?)
            bw.WriteS16(Unknown2);

            // Write Unknown 3.
            bw.WriteS32s(Unknown3);
        }

    }
}
