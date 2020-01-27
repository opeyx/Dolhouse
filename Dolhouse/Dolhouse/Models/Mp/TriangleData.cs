using Dolhouse.Binary;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Triangle Data
    /// </summary>
    public class TriangleData
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
        /// Plane Point Index.
        /// Credits to: @Sage-Of-Mirrors
        /// </summary>
        public short PlanePointIndex { get; set; }

        /// <summary>
        /// PlaneD Value.
        /// Credits to: @Sage-Of-Mirrors
        /// </summary>
        public float PlaneDValue { get; set; }

        /// <summary>
        /// Unknown 1.
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2.
        /// </summary>
        public short Unknown2 { get; set; }

        #endregion


        /// <summary>
        /// Read a single triangle data entry from MP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public TriangleData(DhBinaryReader br)
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

            // Read PlanePointIndex.
            PlanePointIndex = br.ReadS16();

            // Read PlaneD Value.
            PlaneDValue = br.ReadF32();

            // Read Unknown 1.
            Unknown1 = br.ReadS16();

            // Read Unknown 2.
            Unknown2 = br.ReadS16();
        }

        /// <summary>
        /// Write a single triangle data entry with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write Vertex Indices.
            bw.WriteS16s(VertexIndices);

            // Write Normal Index.
            bw.WriteS16(NormalIndex);

            // Write Edge Tangent Indices.
            bw.WriteS16s(EdgeTangentIndices);

            // Write PlanePointIndex.
            bw.WriteS16(PlanePointIndex);

            // Write PlaneD Value.
            bw.WriteF32(PlaneDValue);

            // Write Unknown 1.
            bw.WriteS16(Unknown1);

            // Write Unknown 2.
            bw.WriteS16(Unknown2);
        }
    }
}
