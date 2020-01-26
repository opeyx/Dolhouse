using Dolhouse.Binary;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Unknown
    /// </summary>
    public class Unknown
    {
        #region Properties

        /// <summary>
        /// Unknown 1. (3 bytes)
        /// </summary>
        public byte[] Unknown1 { get; set; }

        #endregion


        /// <summary>
        /// Read a single unknown from MP.
        /// </summary>
        /// <param name="br">The BinaryReader to read with.</param>
        public Unknown(DhBinaryReader br)
        {

            // Read unknown 1. (3 bytes)
            Unknown1 = br.Read(3);
        }

        /// <summary>
        /// Write a single unknown with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write unknown 1. (3 bytes)
            bw.Write(Unknown1);
        }
    }
}
