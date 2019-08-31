using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Normal
    /// </summary>
    public class BinNormal
    {

        #region Properties

        /// <summary>
        /// X-Coordinate.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y-Coordinate.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Z-Coordinate.
        /// </summary>
        public float Z { get; set; }

        #endregion


        /// <summary>
        /// Read a single normal from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinNormal(DhBinaryReader br)
        {

            // Read X-Coordinate.
            X = br.ReadF32();

            // Read Y-Coordinate.
            Y = br.ReadF32();

            // Read Z-Coordinate.
            Z = br.ReadF32();
        }

        /// <summary>
        /// Write a single normal with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write X-Coordinate.
            bw.WriteF32(X);

            // Write Y-Coordinate.
            bw.WriteF32(Y);

            // Write Z-Coordinate.
            bw.WriteF32(Z);
        }
    }
}
