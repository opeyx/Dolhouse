using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Position
    /// </summary>
    public class BinPosition
    {

        #region Properties

        /// <summary>
        /// X-Coordinate.
        /// </summary>
        public short X { get; set; }

        /// <summary>
        /// Y-Coordinate.
        /// </summary>
        public short Y { get; set; }

        /// <summary>
        /// Z-Coordinate.
        /// </summary>
        public short Z { get; set; }

        #endregion


        /// <summary>
        /// Read a single position from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinPosition(DhBinaryReader br)
        {

            // Read X-Coordinate.
            X = br.ReadS16();

            // Read Y-Coordinate.
            Y = br.ReadS16();

            // Read Z-Coordinate.
            Z = br.ReadS16();
        }

        /// <summary>
        /// Write a single material with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write X-Coordinate.
            bw.WriteS16(X);

            // Write Y-Coordinate.
            bw.WriteS16(Y);

            // Write Z-Coordinate.
            bw.WriteS16(Z);
        }
    }
}
