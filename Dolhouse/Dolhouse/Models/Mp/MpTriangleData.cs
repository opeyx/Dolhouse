using Dolhouse.Binary;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Triangle Data
    /// </summary>
    public class MpTriangleData
    {

        #region Properties
        
        /// <summary>
        /// Vertex indices.
        /// </summary>
        public short[] VertexIndices { get; set; }

        /// <summary>
        /// Normal index.
        /// </summary>
        public short NormalIndex { get; set; }

        /// <summary>
        /// Edge Tangent indices.
        /// </summary>
        public short[] EdgeTangentIndices { get; set; }

        /// <summary>
        /// Unknown 1.
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2.
        /// </summary>
        public float Unknown2 { get; set; }

        /// <summary>
        /// Unknown 3.
        /// </summary>
        public short Unknown3 { get; set; }

        /// <summary>
        /// Unknown 4.
        /// </summary>
        public short Unknown4 { get; set; }

        #endregion


        /// <summary>
        /// Read a single triangle data entry from MP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public MpTriangleData(DhBinaryReader br)
        {

            // Define a new array to hold vertex indices.
            VertexIndices = new short[3];

            // Loop through Vertex indices.
            for (int i = 0; i < 3; i++)
            {
                // Read Vertex index and store it in the VertexIndices array.
                VertexIndices[i] = br.ReadS16();
            }

            // Read Normal Index.
            NormalIndex = br.ReadS16();

            // Define a new array to hold edge tangent indices.
            EdgeTangentIndices = new short[3];

            // Loop through Edge Tangent indices.
            for (int i = 0; i < 3; i++)
            {
                // Read Edge Tangent index and store it in the EdgeTangentIndices array.
                EdgeTangentIndices[i] = br.ReadS16();
            }
            
            // Read Unknown 1.
            Unknown1 = br.ReadS16();

            // Read Unknown 2.
            Unknown2 = br.ReadF32();

            // Read Unknown 3.
            Unknown3 = br.ReadS16();

            // Read Unknown 4.
            Unknown4 = br.ReadS16();
        }

        /// <summary>
        /// Write a single triangle data entry with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {
            // TODO: Implement this.
        }
    }
}
