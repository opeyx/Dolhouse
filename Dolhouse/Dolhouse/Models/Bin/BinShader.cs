using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Shader
    /// </summary>
    public class BinShader
    {

        #region Properties

        /// <summary>
        /// Unknown 1.
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2.
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <summary>
        /// Unknown 3.
        /// </summary>
        public byte Unknown3 { get; set; }

        /// <summary>
        /// RGBA Tint.
        /// </summary>
        public int Tint { get; set; }

        /// <summary>
        /// Unknown 4. (Padding)
        /// </summary>
        public byte Unknown4 { get; set; }

        /// <summary>
        /// Index of Material for a texUnit[0-7]
        /// -1 if that texUnit is unused.
        /// </summary>
        public short[] MaterialIndices { get; set; }

        /// <summary>
        /// Unknown5. (Indices?)
        /// </summary>
        public short[] Unknown5 { get; set; }

        #endregion


        /// <summary>
        /// Read a single shader from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinShader(DhBinaryReader br)
        {

            // Read Unknown 1.
            Unknown1 = br.Read();

            // Read Unknown 2.
            Unknown2 = br.Read();

            // Read Unknown 3.
            Unknown3 = br.Read();

            // Read Tint.
            Tint = br.ReadS32();

            // Read Unknown 4. (Padding)
            Unknown4 = br.Read();

            // Define a new array to hold the Material Indices.
            MaterialIndices = new short[8];

            // Loop through Material Indices.
            for (int i = 0; i < 8; i++)
            {
                // Read a material index and store it in Material Indices array.
                MaterialIndices[i] = br.ReadS16();
            }

            // Define a new array to hold Unknown 5. (Indices?)
            Unknown5 = new short[8];

            // Loop through Unknown 5.
            for (int i = 0; i < 8; i++)
            {

                // Read a Unknown 5 value and store it in Unknown5 array.
                Unknown5[i] = br.ReadS16();
            }
        }

        /// <summary>
        /// Write a single shader with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write Unknown 1.
            bw.Write(Unknown1);

            // Write Unknown 2.
            bw.Write(Unknown2);

            // Write Unknown 3.
            bw.Write(Unknown3);

            // Write Tint.
            bw.WriteS32(Tint);

            // Write Unknown 4. (Padding)
            bw.Write(Unknown4);

            // Loop through Material Indices.
            for (int i = 0; i < 8; i++)
            {
                // Write a material index.
                bw.WriteS16(MaterialIndices[i]);
            }

            // Loop through Unknown 5. (Indices?)
            for (int i = 0; i < 8; i++)
            {
                // Write a Unknown 5 value.
                bw.WriteS16(Unknown5[i]);
            }
        }
    }
}
