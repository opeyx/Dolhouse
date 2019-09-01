using Dolhouse.Binary;
using System.Collections.Generic;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Batch
    /// </summary>
    public class BinBatch
    {

        #region Properties

        /// <summary>
        /// Number of faces.
        /// </summary>
        public ushort FaceCount { get; set; }

        /// <summary>
        /// Size of the primitive list. (32-byte blocks)
        /// </summary>
        public ushort ListSize { get; set; }

        /// <summary>
        /// Primitive vertice attributes. (GX.pdf, LSB -> MSB)
        /// </summary>
        public BinBatchAttributes VertexAttributes { get; set; }
        
        /// <summary>
        /// Use Normals flag.
        /// 0x00000001 = enabled
        /// 0x00000000 = disabled
        /// </summary>
        public byte UseNormals { get; set; }

        /// <summary>
        /// Positions Winding.
        /// 0x00000001 = CCW Winding
        /// 0x00000001 = CW Winding
        /// 0x00000010 = ???
        /// </summary>
        public byte Positions { get; set; }

        /// <summary>
        /// UV Count.
        /// </summary>
        public byte UvCount { get; set; }

        /// <summary>
        /// Use NTB flag.
        /// GX_NBT = 1, GX_NRM = 0
        /// </summary>
        public byte UseNBT { get; set; }

        /// <summary>
        /// Primitive list offset.
        /// Relative to batches offset.
        /// </summary>
        public uint PrimitiveOffset { get; set; }

        /// <summary>
        /// Unknown 1. (Padding)
        /// </summary>
        public int[] Unknown1 { get; set; }

        /// <summary>
        /// List of primitives.
        /// </summary>
        public List<BinPrimitive> Primitives { get; set; }

        #endregion


        /// <summary>
        /// Read a single batch from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="batchesOffset">Offset to batches.</param>
        public BinBatch(DhBinaryReader br, long batchesOffset)
        {
            // Read face count.
            FaceCount = br.ReadU16();

            // Read primitive list size.
            ListSize = (ushort)(br.ReadS16() << 5);

            // Read vertex attributes.
            VertexAttributes = (BinBatchAttributes)br.ReadU32();

            // Read UseNormals flag.
            UseNormals = br.Read();

            // Read Position Winding.
            Positions = br.Read();

            // Read UV Count.
            UvCount = br.Read();

            // Read UseNBT flag.
            UseNBT = br.Read();

            // Read Primitive offset.
            PrimitiveOffset = br.ReadU32();

            // Define array to hold Unknown 1 values.
            Unknown1 = new int[2];

            // Loop through Unknown 1 values.
            for (int i = 0; i < 2; i++)
            {
                // Read current Unknown 1 value.
                Unknown1[i] = br.ReadS32();
            }

            // Save the current position.
            long currentPosition = br.Position();

            // Go to the bin batches offset.
            br.Goto(batchesOffset);

            // Sail to the batches's primitive offset.
            br.Sail(PrimitiveOffset);

            // Define list to hold batch's primitives.
            Primitives = new List<BinPrimitive>();

            // Define int to hold amount of faces read.
            int readFaces = 0;

            // Read primitives until batch's face count has been reached.
            while (readFaces < FaceCount && br.Position() < (batchesOffset + PrimitiveOffset + ListSize))
            {

                // Read primitive.
                BinPrimitive binPrimitive = new BinPrimitive(br, UseNBT, UvCount, VertexAttributes);

                // Add the primitive to the batch's primitives.
                Primitives.Add(binPrimitive);

                // Add primitive's face count to the read faces counter.
                readFaces += binPrimitive.FaceCount;
            }

            // Go to the previously saved offset.
            br.Goto(currentPosition);
        }

        /// <summary>
        /// Write a single batch with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {
            // TODO: Implement this.
        }

    }
}
