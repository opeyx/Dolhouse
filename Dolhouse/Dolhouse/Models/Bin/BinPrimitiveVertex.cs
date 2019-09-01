using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Primitive Vertex
    /// </summary>
    public class BinPrimitiveVertex
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

        #endregion


        /// <summary>
        /// Read a single primitive vertex from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinPrimitiveVertex(DhBinaryReader br, byte useNBT, int uvCount, BinBatchAttributes attributes)
        {

            // Read Matrix Index.
            MatrixIndex = br.ReadS16();

            // Check position attribute.
            if (attributes.HasFlag(BinBatchAttributes.Position))
            {
                // Read Position Index.
                PositionIndex = br.ReadS16();
            }

            // Check normal attribute.
            if (attributes.HasFlag(BinBatchAttributes.Normal))
            {

                // Read Normal Index.
                NormalIndex = br.ReadS16();

                // Check UseNBT flag.
                if (useNBT == 1)
                {
                    // Read BiNormal Index.
                    BiNormalIndex = br.ReadS16();

                    // Read Tangent Index.
                    TangentIndex = br.ReadS16();
                }
            }

            // Define ColorIndex array to hold ColorIndex values.
            ColorIndex = new short[2];

            // Check color2 attribute.
            if (attributes.HasFlag(BinBatchAttributes.Color0))
            {
                // Read the Color1 value.
                ColorIndex[0] = br.ReadS16();
            }

            // Check color1 attribute.
            if (attributes.HasFlag(BinBatchAttributes.Color1))
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
                if (attributes.HasFlag((BinBatchAttributes)(1 << (13 + i))))
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
        public void Write(DhBinaryWriter bw)
        {
            // Write Matrix Index.
            bw.WriteS16(MatrixIndex);

            // Write Position Index.
            bw.WriteS16(PositionIndex);

            // Write Normal Index.
            bw.WriteS16(NormalIndex);

            // Write BiNormal Index.
            bw.WriteS16(BiNormalIndex);

            // Write Tangent Index.
            bw.WriteS16(TangentIndex);

            // Loop through the ColorIndex array.
            for (int i = 0; i < 2; i++)
            {
                // Write ColorIndex value.
                bw.WriteS16(ColorIndex[i]);
            }

            // Loop through the UVIndex array.
            for (int i = 0; i < 8; i++)
            {
                // Write UVIndex value.
                bw.WriteS16(UVIndex[i]);
            }
        }
    }
}
