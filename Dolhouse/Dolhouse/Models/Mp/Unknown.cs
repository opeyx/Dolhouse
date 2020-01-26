using Dolhouse.Binary;

namespace Dolhouse.Models.Mp
{

    /// <summary>
    /// Unknownm
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
    }
}
