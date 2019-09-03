using Dolhouse.Binary;
using OpenTK;
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
        public Vector3 GridScale { get; set; }

        /// <summary>
        /// Minimum Bounds.
        /// </summary>
        public Vector3 MinimumBounds { get; set; }

        /// <summary>
        /// Axis Lengths.
        /// </summary>
        public Vector3 AxisLengths { get; set; }

        /// <summary>
        /// Offsets.
        /// </summary>
        public int[] Offsets { get; set; }

        /// <summary>
        /// List of vertices.
        /// </summary>
        public List<Vector3> Vertices { get; set; }

        /// <summary>
        /// List of normals / edge tangents.
        /// </summary>
        public List<Vector3> Normals { get; set; }

        /// <summary>
        /// List of triangle data.
        /// </summary>
        public List<MpTriangleData> TriangleData { get; set; }

        /// <summary>
        /// List of grid indices.
        /// </summary>
        public List<MpGridIndex> GridIndices { get; set; }

        #endregion

        /// <summary>
        /// Reads MP from a stream.
        /// </summary>
        /// <param name="stream">The stream containing the MP data.</param>
        public MP(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read Grid Scale.
            GridScale = new Vector3(br.ReadF32(), br.ReadF32(), br.ReadF32());

            // Read Minimum Bounds.
            MinimumBounds = new Vector3(br.ReadF32(), br.ReadF32(), br.ReadF32());

            // Read Axis Lengths.
            AxisLengths = new Vector3(br.ReadF32(), br.ReadF32(), br.ReadF32());

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
            Vertices = new List<Vector3>();

            // Loop through vertices.
            for (int i = 0; i < verticeCount; i++)
            {
                // Read vertex and add it to the vertice list.
                Vertices.Add(new Vector3(br.ReadF32(), br.ReadF32(), br.ReadF32()));
            }


            // Go to mp's normals offset.
            br.Goto(Offsets[1]);

            // Calculate amount of normals.
            int normalCount = (Offsets[2] - Offsets[1]) / 12;

            // Define new list to hold normals.
            Normals = new List<Vector3>();

            // Loop through normals.
            for (int i = 0; i < verticeCount; i++)
            {
                // Read normal and add it to the normal list.
                Normals.Add(new Vector3(br.ReadF32(), br.ReadF32(), br.ReadF32()));
            }


            // Go to mp's triangle data offset.
            br.Goto(Offsets[2]);

            // Calculate amount of normals.
            int triangleDataCount = (Offsets[3] - Offsets[2]) / 24;

            // Define new list to hold normals.
            TriangleData = new List<MpTriangleData>();

            // Loop through normals.
            for (int i = 0; i < triangleDataCount; i++)
            {
                // Read normal and add it to the normal list.
                TriangleData.Add(new MpTriangleData(br));
            }


            // Go to mp's triangle group offset.
            br.Goto(Offsets[3]);

            // TODO: Implement reading Triangle Group data.


            // Go to mp's grid index offset.
            br.Goto(Offsets[4]);

            // Calculate amount of grid index entries.
            int gridIndexCount = (Offsets[4] - Offsets[3]) / 8;

            // Define new list to hold grid indices.
            GridIndices = new List<MpGridIndex>();

            // Loop through grid indices.
            for (int i = 0; i < gridIndexCount; i++)
            {
                // Read grid index and add it to the grid indices list.
                GridIndices.Add(new MpGridIndex(br));
            }


            // Go to mp's unknown offset.
            br.Goto(Offsets[5]);

            // TODO: Implement reading Unknown data.
        }

        /// <summary>
        /// Creates a stream from this MP. TODO: Complete writing.
        /// </summary>
        /// <returns>The MP as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our MP data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            return stream;
        }

        /// <summary>
        /// Creates a Wavefront Obj from this BIN.
        /// TODO: Fix this. (It does not properly export MP as OBJ)
        /// </summary>
        /// <returns>The BIN as a Wavefront Obj.</returns>
        public string ToObj()
        {

            // Define a string to hold output.
            string objString = "# This file was generated by Dolhouse.\r\n\r\n";

            // Write Vertices.
            objString += "# Vertices\r\n";
            for (int i = 0; i < Vertices.Count; i++)
            {
                objString += "v " + Vertices[i].X.ToString() + " " + Vertices[i].Y.ToString() + " " + Vertices[i].Z.ToString() + "\r\n";
            }

            // Write some spacing.
            objString += "\r\n";

            // Write Normals.
            objString += "# Normals\r\n";
            for (int i = 0; i < Normals.Count; i++)
            {
                objString += "vn " + Normals[i].X.ToString("n6") + " " + Normals[i].Y.ToString("n6") + " " + Normals[i].Z.ToString("n6") + "\r\n";
            }

            // Write some spacing.
            objString += "\r\n";

            // Write Faces.
            objString += "# Faces\r\n";
            for(int i = 0; i < (TriangleData.Count); i++)
            {
                objString += "f " + TriangleData[i].VertexIndices[0].ToString() + "//" + TriangleData[i].EdgeTangentIndices[0].ToString();
                objString += " " + TriangleData[i].VertexIndices[1].ToString() + "//" + TriangleData[i].EdgeTangentIndices[1].ToString();
                objString += " " + TriangleData[i].VertexIndices[2].ToString() + "//" + TriangleData[i].EdgeTangentIndices[2].ToString() + "\r\n";
            }

            // Return the MP as a WaveFront Obj.
            return objString;
        }
    }
}
