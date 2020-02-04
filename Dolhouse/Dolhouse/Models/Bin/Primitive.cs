using Dolhouse.Binary;
using Dolhouse.Models.GX;
using System.Collections.Generic;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Primitive
    /// </summary>
    public class Primitive
    {

        #region Properties

        /// <summary>
        /// Primitive Type.
        /// </summary>
        public PrimitiveType Type { get; set; }

        /// <summary>
        /// Number of vertices.
        /// </summary>
        public short VerticeCount { get; set; }

        /// <summary>
        /// Number of faces total. (Calculated)
        /// </summary>
        public int FaceCount { get { return (Vertices.Count - 2); } }

        /// <summary>
        /// List of vertices.
        /// </summary>
        public List<GX.Vertex> Vertices { get; set; }

        #endregion


        /// <summary>
        /// Read a single primitive from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public Primitive(DhBinaryReader br, Attributes attributes)
        {

            // Read Primitive Type.
            Type = (PrimitiveType)br.ReadU8();

            // Read number of vertices.
            VerticeCount = br.ReadS16();

            // Define new list to hold primitive vertices.
            Vertices = new List<Vertex>();

            // Loop through primitive vertices.
            for(int i = 0; i < VerticeCount; i++)
            {
                // Read a primitive vertex and add it to the list.
                Vertices.Add(new Vertex(br, attributes));
            }
        }

        /// <summary>
        /// Write a single primitive vertex with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw, Attributes attributes)
        {
            // Write Primitive type.
            bw.Write((byte)Type);

            // Write number of primitive vertices.
            bw.WriteS16((short)Vertices.Count);

            // Loop through primitive vertices.
            for(int i = 0; i < Vertices.Count; i++)
            {
                // Write primitive vertex.
                Vertices[i].Write(bw, attributes);
            }
        }
    }
}
