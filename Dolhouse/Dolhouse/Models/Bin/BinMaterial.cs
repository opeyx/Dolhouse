using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{
    
    /// <summary>
    /// Bin Material
    /// </summary>
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

            // Define a array to hold the unknown 3 values.
            Unknown3 = new int[3];

            // Loop through the unknown 3 values.
            for (int i = 0; i < Unknown3.Length; i++)
            {
                // Write the current unknown value.
                Unknown3[i] = br.ReadS32();
            }
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

            // Loop through the Unknown 3 values.
            for (int i = 0; i < Unknown3.Length; i++)
            {
                // Write the current Unknown 3 value.
                bw.WriteS32(Unknown3[i]);
            }
        }

    }
}
