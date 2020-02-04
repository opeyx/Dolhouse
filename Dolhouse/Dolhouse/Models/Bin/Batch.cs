using Dolhouse.Binary;
using Dolhouse.Models.GX;
using System;
using System.Collections.Generic;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Batch
    /// </summary>
    public class Batch
    {

        #region Properties

        /// <summary>
        /// Number of faces.
        /// </summary>
        public ushort FaceCount { get; set; }

        /// <summary>
        /// Size of the primitive list. (32-byte blocks)
        /// </summary>
        public short ListSize { get; set; }

        /// <summary>
        /// Primitive vertice attributes. (GX.pdf, LSB -> MSB)
        /// </summary>
        public Attributes VertexAttributes { get; set; }
        
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
        public List<Primitive> Primitives { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new batch.
        /// </summary>
        public Batch()
        {
            // Set face count.
            FaceCount = 0;

            // Set primitive list size.
            ListSize = 0;

            // Set vertex attributes.
            VertexAttributes = 0;

            // Set UseNormals flag.
            UseNormals = 0;

            // Set Position Winding.
            Positions = 0;

            // Set UV Count.
            UvCount = 0;

            // Set UseNBT flag.
            UseNBT = 0;

            // Set Primitive offset.
            PrimitiveOffset = 0;

            // Set Unknown 1. (Padding?)
            Unknown1 = new int[2];

            // Define list to hold batch's primitives.
            Primitives = new List<Primitive>();
        }

        /// <summary>
        /// Read a single batch from BIN.
        /// </summary>
        /// <param name="br">The binaryreader to write with.</param>
        /// <param name="batchesOffset">Offset to batches.</param>
        public Batch(DhBinaryReader br, long batchesOffset)
        {
            // Read face count.
            FaceCount = br.ReadU16();

            // Read primitive list size.
            ListSize = br.ReadS16();

            // Read vertex attributes.
            VertexAttributes = (Attributes)br.ReadU32();

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

            // Read Unknown 1. (Padding?)
            Unknown1 = br.ReadS32s(2);

            // Save the current position.
            long currentPosition = br.Position();

            // Go to the batch's primitive offset offset.
            br.Goto(batchesOffset + PrimitiveOffset);

            // Define list to hold batch's primitives.
            Primitives = new List<Primitive>();

            // Define int to keep track of the amount of faces read.
            int readFaces = 0;

            // Read primitives until batch's face count has been reached.
            while ((readFaces < FaceCount) && (br.Position() < (batchesOffset + PrimitiveOffset + (ListSize << 5))))
            {

                // Read primitive.
                Primitive binPrimitive = new Primitive(br, VertexAttributes);

                // Add the primitive to the batch's primitives.
                Primitives.Add(binPrimitive);

                // Add primitive's face count to the read faces counter.
                readFaces += binPrimitive.FaceCount;
            }

            // Go to the previously saved offset.
            br.Goto(currentPosition);
        }

        /// <summary>
        /// Write a single batch.
        /// </summary>
        /// <param name="bw">The binarywriter to write with.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write face count.
            bw.WriteU16(FaceCount);

            // Write list size. (CALCULATED)
            bw.WriteS16(0);

            // Write vertex attributes.
            bw.WriteU32((uint)VertexAttributes);

            // Write normals flag.
            bw.Write(UseNormals);

            // Write position winding flag.
            bw.Write(Positions);

            // Write uv count.
            bw.Write(UvCount);

            // Write NBT flag.
            bw.Write(UseNBT);

            // Write primitive offset. (CALCULATED)
            bw.WriteU32(0);

            // Write unknown 1.
            bw.WriteS32s(Unknown1); // 8 bytes
        }
    }
}
