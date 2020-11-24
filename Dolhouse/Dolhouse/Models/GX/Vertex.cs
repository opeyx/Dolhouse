using Dolhouse.Binary;
using System;
using System.Linq;

namespace Dolhouse.Models.GX
{
    public class Vertex
    {

        #region Properties

        /// <summary>
        /// Array of Indices.
        /// </summary>
        public short?[] Indices { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty Vertex.
        /// </summary>
        public Vertex()
        {

            // Initialize a new empty array to hold the indices.
            Indices = new short?[26];
        }

        /// <summary>
        /// Read a single vertex.
        /// </summary>
        /// <param name="br">The binaryreader to read with.</param>
        public Vertex(DhBinaryReader br, Attributes attributes, bool useNbt)
        {

            // Each vertex will always hold 26 indices. (Null = unused)
            Indices = new short?[26];

            // Get all the attributes from GX attributes.
            var indices = Enum.GetValues(typeof(Attributes));

            // Loop through GX attributes.
            for(int i = 0; i < indices.Length; i++)
            {

                // Make sure this attribute is present.
                if (attributes.HasFlag((Attributes)(1 << i)))
                {
                    // Read the current index.
                    Indices[i] = br.ReadS16();

                    // Check if we're reading normals and nbt is enabled.
                    if(i == 10 && useNbt)
                    {
                        // Set bitangent/tangent indices. (TODO - Fix the indices here...)
                        //Indices[24] = br.ReadS16();
                        //Indices[25] = br.ReadS16();
                        br.Skip(4); // Thanks to Astral-C for the tip of just throwing these away. (Until support is added)
                    }
                }
            }
        }

        /// <summary>
        /// Write a single vertex.
        /// </summary>
        /// <param name="bw">The binarywriter to write with.</param>
        public void Write(DhBinaryWriter bw, Attributes attributes)
        {

            // Loop through indices.
            for(int i = 0; i < Indices.Length; i++)
            {

                // Make sure index is not null.
                if (Indices[i] != null)
                {

                    // Write Index.
                    bw.WriteS16((short)Indices[i]);
                }
            }
        }
    }
}
