using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Texture Coordinate
    /// </summary>
    public class BinTextureCoordinate
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

        #endregion


        /// <summary>
        /// Read a single texture coordinate from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BinTextureCoordinate(DhBinaryReader br)
        {

            // Read X-Coordinate.
            X = br.ReadF32();

            // Read Y-Coordinate.
            Y = br.ReadF32();
        }

        /// <summary>
        /// Write a single texture coordinate with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write X-Coordinate.
            bw.WriteF32(X);

            // Write Y-Coordinate.
            bw.WriteF32(Y);
        }
    }
}
