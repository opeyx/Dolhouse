using Dolhouse.Binary;
using Dolhouse.Type;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// (M)a(p) Collision
    /// </summary>
    public class MP
    {

        #region Properties

        /// <summary>
        /// Grid scale.
        /// </summary>
        public Vec3 GridScale { get; set; }

        /// <summary>
        /// Minimum Bounds.
        /// </summary>
        public Vec3 MinimumBounds { get; set; }

        /// <summary>
        /// Axis Lengths.
        /// </summary>
        public Vec3 AxisLengths { get; set; }

        /// <summary>
        /// Offsets.
        /// </summary>
        public int[] Offsets { get; set; }

        /// <summary>
        /// List of vertices.
        /// </summary>
        public List<Vec3> Vertices { get; set; }

        /// <summary>
        /// List of normals / edge tangents.
        /// </summary>
        public List<Vec3> Normals { get; set; }

        /// <summary>
        /// List of triangle data.
        /// </summary>
        public List<TriangleData> TriangleData { get; set; }

        /// <summary>
        /// List of triangle groups.
        /// </summary>
        public List<TriangleGroup> TriangleGroups { get; set; }

        /// <summary>
        /// List of grid indices.
        /// </summary>
        public List<GridIndex> GridIndices { get; set; }

        /// <summary>
        /// List of unknown data. (3 bytes each)
        /// </summary>
        public List<Unknown> Unknowns { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty MP.
        /// </summary>
        public MP()
        {

            // Set Grid Scale.
            GridScale = new Vec3();

            // Set Minimum Bounds.
            MinimumBounds = new Vec3();

            // Set Axis Lengths.
            AxisLengths = new Vec3();

            // Define new array to hold offsets.
            Offsets = new int[7];

            // Define new list to hold vertices.
            Vertices = new List<Vec3>();

            // Define new list to hold normals.
            Normals = new List<Vec3>();

            // Define new list to hold triangle data.
            TriangleData = new List<TriangleData>();

            // Define new list to hold triangle groups.
            TriangleGroups = new List<TriangleGroup>();

            // Define new list to hold grid indices.
            GridIndices = new List<GridIndex>();

            // Define new list to hold unknown data.
            Unknowns = new List<Unknown>();
        }

        /// <summary>
        /// Reads MP from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the MP data.</param>
        public MP(byte[] data)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(data, DhEndian.Big);

            // Read Grid Scale.
            GridScale = br.ReadVec3();

            // Read Minimum Bounds.
            MinimumBounds = br.ReadVec3();

            // Read Axis Lengths.
            AxisLengths = br.ReadVec3();

            // Define new array to hold offsets.
            Offsets = new int[7];

            // Loop through our offsets.
            for (int i = 0; i < 7; i++)
            {
                // Read offset and store it to the offsets array.
                Offsets[i] = br.ReadS32();
            }


            // Go to mp's vertex data offset.
            br.Goto(Offsets[0]);

            // Calculate amount of vertices.
            int verticeCount = (Offsets[1] - Offsets[0]) / 12;

            // Define new list to hold vertices.
            Vertices = new List<Vec3>();

            // Loop through vertices.
            for (int i = 0; i < verticeCount; i++)
            {
                // Read vertex and add it to the list of vertices.
                Vertices.Add(br.ReadVec3());
            }


            // Go to mp's normals offset.
            br.Goto(Offsets[1]);

            // Calculate amount of normals.
            int normalCount = (Offsets[2] - Offsets[1]) / 12;

            // Define new list to hold normals.
            Normals = new List<Vec3>();

            // Loop through normals.
            for (int i = 0; i < normalCount; i++)
            {
                // Read normal and add it to the normal list.
                Normals.Add(br.ReadVec3());
            }


            // Go to mp's triangle data offset.
            br.Goto(Offsets[2]);

            // Calculate amount of triangles.
            int triangleDataCount = (Offsets[3] - Offsets[2]) / 24;

            // Define new list to hold triangle data.
            TriangleData = new List<TriangleData>();

            // Loop through triangles.
            for (int i = 0; i < triangleDataCount; i++)
            {
                // Read triangle and add it to the triangle data list.
                TriangleData.Add(new TriangleData(br));
            }


            // Go to mp's triangle group offset.
            br.Goto(Offsets[3]);

            // Define new list to hold triangle groups.
            TriangleGroups = new List<TriangleGroup>();

            // Make sure first ushort is 0xFFFF.
            if(br.ReadU16() != 0xFFFF)
            {
                throw new FormatException("Start of triangle groups section was not 0xFFFF!");
            }

            // We'll read triangle groups as long as we're not entering the next section.
            while(br.Position() < Offsets[4] - 2)
            {

                // Read group and add it to the groups list.
                TriangleGroups.Add(new TriangleGroup(br));
            }

            // Make sure last ushort is 0xFFFF.
            if (br.ReadU16() != 0xFFFF)
            {
                throw new FormatException("End of triangle groups section was not 0xFFFF!");
            }

            // Go to mp's grid index offset.
            br.Goto(Offsets[4]);

            // Calculate amount of grid index entries. (Offset 5 is a duplicate of offset 4)
            int gridIndexCount = (Offsets[6] - Offsets[4]) / 8;

            // Define new list to hold grid indices.
            GridIndices = new List<GridIndex>();

            // Loop through grid indices.
            for (int i = 0; i < gridIndexCount; i++)
            {
                GridIndices.Add(new GridIndex(br));
            }


            // Go to mp's unknown offset.
            br.Goto(Offsets[6]);

            // Define new list to hold unknown data.
            Unknowns = new List<Unknown>();

            // We'll read unknowns as long as we're not reading padding EOF.
            while (br.Position() < br.GetStream().Length)
            {

                // Check if next byte is 0xFF. (More data)
                if (br.Read() == 0xFF)
                {

                    // Jump back a byte, since we checked for data.
                    br.Sail(-1);

                    // Read unknown and add it to the unknowns list.
                    Unknowns.Add(new Unknown(br));
                }
                else
                {

                    // We've reached padding, stop reading.
                    break;
                }
            }
        }

        /// <summary>
        /// Creates a byte array from this MP. TODO: Complete writing.
        /// </summary>
        /// <returns>The MP as a byte array.</returns>
        public byte[] Write()
        {

            // Define a stream to hold our MP data.
            MemoryStream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Define a buffer to store our offsets.
            uint[] offsets = new uint[7];

            // Write Grid Scale.
            bw.WriteVec3(GridScale);

            // Write Minimum Bounds.
            bw.WriteVec3(MinimumBounds);

            // Write Axis Lengths.
            bw.WriteVec3(AxisLengths);

            // Write offsets. (CALCULATED)
            bw.WriteU32s(offsets);

            // Set vertices offset.
            offsets[0] = (uint)bw.Position();

            // Loop through vertices.
            for (int i = 0; i < Vertices.Count; i++)
            {

                // Write vertex.
                bw.WriteVec3(Vertices[i]);
            }

            // Set normals offset.
            offsets[1] = (uint)bw.Position();

            // Loop through normals.
            for (int i = 0; i < Normals.Count; i++)
            {

                // Write normals.
                bw.WriteVec3(Normals[i]);
            }

            // Set triangle data offset.
            offsets[2] = (uint)bw.Position();

            // Loop through triangle data.
            for (int i = 0; i < TriangleData.Count; i++)
            {

                // Write triangle data.
                TriangleData[i].Write(bw);
            }

            // Set triangle groups offset.
            offsets[3] = (uint)bw.Position();

            // Write ushort to define beginning of triangle groups section.
            bw.WriteU16(0xFFFF);

            // Loop through triangle groups.
            for (int i = 0; i < TriangleGroups.Count; i++)
            {

                // Write triangle group.
                TriangleGroups[i].Write(bw);
            }

            // Write ushort to define ending of triangle groups section.
            bw.WriteU16(0xFFFF);

            // Set grid indices offset.
            offsets[4] = (uint)bw.Position();

            // Set duplicated grid indices offset.
            offsets[5] = (uint)bw.Position();

            // Loop through grid indices.
            for (int i = 0; i < GridIndices.Count; i++)
            {

                // Write grid index.
                GridIndices[i].Write(bw);
            }

            // Set unknown data offset.
            offsets[6] = (uint)bw.Position();

            // Loop through unknown data. (3 bytes each)
            for (int i = 0; i < Unknowns.Count; i++)
            {

                // Write unknown data.
                Unknowns[i].Write(bw);
            }

            // Pad to nearest whole 32.
            bw.WritePadding32('@');

            // Goto offsets section.
            bw.Goto(36);

            // Write offsets.
            bw.WriteU32s(offsets);

            // Return the MP as a byte array.
            return stream.ToArray();
        }
    }
}
