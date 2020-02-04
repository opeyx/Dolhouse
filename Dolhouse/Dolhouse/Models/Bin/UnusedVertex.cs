using Dolhouse.Binary;
using Dolhouse.Models.GX;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Vertex
    /// (THIS CLASS CURRENTLY NOT IN USE, FOR VERTICES,
    /// THE VERTEX CLASS (SEE GX FOLDER) IS USED INSTEAD)
    /// </summary>
    public class UnusedVertex
    {

        #region Properties

        /// <summary>
        /// Matrix Index.
        /// </summary>
        public short MatrixIndex { get; set; }

        /// <summary>
        /// Position Index.
        /// </summary>
        public short PositionIndex { get; set; }

        /// <summary>
        /// Normal Index.
        /// </summary>
        public short NormalIndex { get; set; }

        /// <summary>
        /// BiNormal Index.
        /// </summary>
        public short BiNormalIndex { get; set; }

        /// <summary>
        /// Tangent Index.
        /// </summary>
        public short TangentIndex { get; set; }

        /// <summary>
        /// Color Indices.
        /// </summary>
        public short[] ColorIndex { get; set; }

        /// <summary>
        /// UV Indices.
        /// </summary>
        public short[] UVIndex { get; set; }

        /// <summary>
        /// For use with MDL's only. TODO - REWRITE THIS ENTIRE CLASS!
        /// </summary>
        public int[] MDLIndices { get; set; }

        #endregion


        public UnusedVertex(int[] indices)
        {
            MDLIndices = indices;
        }

        /// <summary>
        /// Read a single primitive vertex from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public UnusedVertex(DhBinaryReader br, int uvCount, Attributes attributes)
        {

            // Check position attribute.
            if (attributes.HasFlag(Attributes.PosNormMatrix))
            {
                // Read Matrix Index.
                MatrixIndex = br.ReadS16();
            }

            // Check Position attribute.
            if (attributes.HasFlag(Attributes.Position))
            {
                // Read Position Index.
                PositionIndex = br.ReadS16();
            }

            // Check Normal attribute.
            if (attributes.HasFlag(Attributes.Normal))
            {

                // Read Normal Index.
                NormalIndex = br.ReadS16();

                // Check if UseNBT flag is true.
                if (attributes.HasFlag(Attributes.NormalBinormalTangent))
                {

                    // Read BiNormal Index.
                    BiNormalIndex = br.ReadS16();

                    // Read Tangent Index.
                    TangentIndex = br.ReadS16();
                }
            }

            // Define ColorIndex array to hold ColorIndex values.
            ColorIndex = new short[2];

            // Check Color0 attribute.
            if (attributes.HasFlag(Attributes.Color0))
            {
                // Read the Color0 value.
                ColorIndex[0] = br.ReadS16();
            }

            // Check Color1 attribute.
            if (attributes.HasFlag(Attributes.Color1))
            {
                // Read the Color1 value.
                ColorIndex[1] = br.ReadS16();
            }

            // Define UVIndex array to hold UVIndex values.
            UVIndex = new short[8];

            // Loop through texCoords.
            for (int i = 0; i < uvCount; i++)
            {

                // Check texCoordX attribute.
                if (attributes.HasFlag((Attributes)(1 << (13 + i))))
                {

                    // Read the current UVIndex value.
                    UVIndex[i] = br.ReadS16();
                }
            }
        }

        /// <summary>
        /// Write a single primitive vertex with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw, Attributes attributes)
        {

            // Check if Matrix Index exists.
            if (attributes.HasFlag(Attributes.PosNormMatrix))
            {

                // Write BiNormal Index.
                bw.WriteS16((short)MatrixIndex);
            }

            // Check if Position Index exists.
            if (attributes.HasFlag(Attributes.Position))
            {

                // Write BiNormal Index.
                bw.WriteS16((short)PositionIndex);
            }

            // Check if Normal Index exists.
            if (attributes.HasFlag(Attributes.Normal))
            {

                // Write Normal Index.
                bw.WriteS16((short)NormalIndex);

                // Check if NBT Indices exists.
                if (attributes.HasFlag(Attributes.NormalBinormalTangent))
                {

                    // Write BiNormal Index.
                    bw.WriteS16((short)BiNormalIndex);

                    // Write Tangent Index.
                    bw.WriteS16((short)TangentIndex);
                }
            }

            // Check if Color0 Index exists.
            if (attributes.HasFlag(Attributes.Color0))
            {

                // Write Color1 Index.
                bw.WriteS16((short)ColorIndex[0]);
            }

            // Check if Color1 Index exists.
            if (attributes.HasFlag(Attributes.Color1))
            {

                // Write Color1 Index.
                bw.WriteS16((short)ColorIndex[1]);
            }

            // Loop through UV Indices.
            for (int i = 0; i < UVIndex.Length; i++)
            {

                // Check if current UV Index exists.
                if (attributes.HasFlag((Attributes)(1 << (13 + i))))
                {

                    // Write UV Index.
                    bw.WriteS16((short)UVIndex[i]);
                }
            }
        }
    }
}
