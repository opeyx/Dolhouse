using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Shader
    /// </summary>
    public class Shader
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
        /// Material Index for texUnits[0-7].
        /// (-1 == unused)
        /// </summary>
        public short[] MaterialIndices { get; set; }

        /// <summary>
        /// Unknown5. (Indices?)
        /// </summary>
        public short[] Unknown5 { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new shader.
        /// </summary>
        public Shader()
        {

            // Read Unknown 1.
            Unknown1 = 0;

            // Read Unknown 2.
            Unknown2 = 0;

            // Read Unknown 3.
            Unknown3 = 0;

            // Read Tint.
            Tint = 0;

            // Read Unknown 4. (Padding)
            Unknown4 = 0;

            // Read Material Indices array.
            MaterialIndices = new short[8] { -1, -1, -1, -1, -1, -1, -1, -1 };

            // Read Unknown 5 array. (Indices?)
            Unknown5 = new short[8];
        }

        /// <summary>
        /// Read a single shader from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public Shader(DhBinaryReader br)
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

            // Read Material Indices array.
            MaterialIndices = br.ReadS16s(8);

            // Read Unknown 5 array. (Indices?)
            Unknown5 = br.ReadS16s(8);
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

            // Write Material Indices.
            bw.WriteS16s(MaterialIndices);

            // Write Unknown 5. (Indices?)
            bw.WriteS16s(Unknown5);
        }
    }
}
