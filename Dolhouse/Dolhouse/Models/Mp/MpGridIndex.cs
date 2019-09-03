using Dolhouse.Binary;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Grid Index
    /// </summary>
    public class MpGridIndex
    {

        #region Properties

        public int TotalTriangleGroupIndex { get; set; }
        public int FloorTriangleGroupIndex { get; set; }

        #endregion


        /// <summary>
        /// Read a single grid index from MP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public MpGridIndex(DhBinaryReader br)
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
