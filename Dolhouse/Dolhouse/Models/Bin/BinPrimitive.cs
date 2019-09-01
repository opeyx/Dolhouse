using Dolhouse.Binary;
using System.Collections.Generic;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Primitive
    /// </summary>
    public class BinPrimitive
    {

        #region Properties

        /// <summary>
        /// Primitive Type. (Same as BMD)
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// Number of vertices.
        /// </summary>
        public short VerticeCount { get; set; }

        /// <summary>
        /// Number of faces total. (Calculated)
        /// </summary>
        public int FaceCount { get { return (VerticeCount - 2); } }

        /// <summary>
        /// List of vertices.
        /// </summary>
        public List<BinPrimitiveVertex> Vertices { get; set; }

        #endregion


        /// <summary>
        /// Read a single primitive from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinPrimitive(DhBinaryReader br, byte useNBT, int uvCount, BinBatchAttributes attributes)
        {

            // Read Primitive Type.
            Type = br.Read();

            // Read number of vertices.
            VerticeCount = br.ReadS16();

            // Define ColorIndex array to hold primitive vertices.
            Vertices = new List<BinPrimitiveVertex>();

            // Loop through primitive vertices.
            for(int i = 0; i < VerticeCount; i++)
            {
                // Read a primitive vertex and add it to the list.
                Vertices.Add(new BinPrimitiveVertex(br, useNBT, uvCount, attributes));
            }
        }

        /// <summary>
        /// Write a single primitive vertex with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {
            // Write Primitive type.
            bw.Write(Type);

            // Write number of primitive vertices.
            bw.WriteS16((short)Vertices.Count);

            // Loop through primitive vertices.
            for(int i = 0; i < Vertices.Count; i++)
            {
                // Write primitive vertex.
                Vertices[i].Write(bw);
            }
        }
    }
}
