using Dolhouse.Binary;
using System.Collections.Generic;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Triangle Group
    /// </summary>
    public class TriangleGroup
    {

        #region Properties

        /// <summary>
        /// Indices of the triangles within this triangle group.
        /// </summary>
        public ushort[] Indices { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty TriangleGroup.
        /// </summary>
        public TriangleGroup()
        {

            // Define a new array to hold triangle indices.
            Indices = new ushort[0];
        }

        /// <summary>
        /// Read a single triangle group from MP.
        /// </summary>
        /// <param name="br">The BinaryReader to read with.</param>
        public TriangleGroup(DhBinaryReader br)
        {

            // Define temporary list of ushorts to hold the indices.
            List<ushort> indices = new List<ushort>();

            // We'll read untill we read 0xFFFF, that means the end of this triangle group.
            while(br.ReadU16() != 0xFFFF)
            {
                // We'll go two bytes back, since we checked for 0xFFFF.
                br.Sail(-2);

                // Read a ushort, and add it to the list of indices.
                indices.Add(br.ReadU16());
            }

            // Set the indices array to the ones we've read.
            Indices = indices.ToArray();
        }

        /// <summary>
        /// Write a single triangle group with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write indices.
            bw.WriteU16s(Indices);

            // Write ushort to define end of this triangle group.
            bw.WriteU16(0xFFFF);
        }
    }
}
