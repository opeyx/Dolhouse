using Dolhouse.Binary;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Grid Index
    /// </summary>
    public class GridIndex
    {

        #region Properties

        /// <summary>
        /// Index of triangle group thats's within grid cell.
        /// Credits: @Sage-Of-Mirrors
        /// </summary>
        public int TotalTriangleGroupIndex { get; set; }

        /// <summary>
        /// Index of triangle group thats's considered floor / walkable.
        /// Credits: @Sage-Of-Mirrors
        /// </summary>
        public int FloorTriangleGroupIndex { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty GridIndex.
        /// </summary>
        public GridIndex()
        {

            // Set TotalTriangleGroupIndex.
            TotalTriangleGroupIndex = 0;

            // Set FloorTriangleGroupIndex.
            FloorTriangleGroupIndex = 0;
        }

        /// <summary>
        /// Read a single grid index from MP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public GridIndex(DhBinaryReader br)
        {

            // Read TotalTriangleGroupIndex.
            TotalTriangleGroupIndex = br.ReadS32();

            // Read FloorTriangleGroupIndex.
            FloorTriangleGroupIndex = br.ReadS32();
        }

        /// <summary>
        /// Write a single grid index with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {
            // Write TotalTriangleGroupIndex.
            bw.WriteS32(TotalTriangleGroupIndex);

            // Write FloorTriangleGroupIndex.
            bw.WriteS32(FloorTriangleGroupIndex);
        }
    }
}
