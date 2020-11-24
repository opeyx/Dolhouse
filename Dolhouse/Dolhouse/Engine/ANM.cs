using Dolhouse.Binary;
using System.IO;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (A)(N)i(M)ation
    /// TODO: Further reasearch, complete the file.
    /// </summary>
    public class ANM
    {

        #region Properties

        /// <summary>
        /// ANM version.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Flag to determine if
        /// animation should loop.
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// Unknown 1 (Padding?)
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// Amount of keyframes.
        /// </summary>
        public uint KeyFrameCount { get; set; }

        /// <summary>
        /// Offset to keyframes. (Absolute)
        /// </summary>
        public uint KeyFrameOffset { get; set; }

        /// <summary>
        /// Unknown 2. (Offset?)
        /// </summary>
        public int Unknown2 { get; set; }

        /// <summary>
        /// Unknown 3. (Offset?)
        /// </summary>
        public int Unknown3 { get; set; }

        /// <summary>
        /// Unknown 4. (Header size?)
        /// </summary>
        public int Unknown4 { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty ANM.
        /// </summary>
        public ANM()
        {
            // Set ANM's Header.
            Version = 2;
            Loop = false;
            Unknown1 = 0;
            KeyFrameCount = 0;
            KeyFrameOffset = 0;
            Unknown2 = 0;
            Unknown3 = 0;
            Unknown4 = 24; // Seems to always be 24 (dec).

            /*
                Keyframes - Interpolation type list:
                https://ia800802.us.archive.org/9/items/GCN_SDK_Documentation/Game%20Engine%20Programming.pdf
            */
        }

        /// <summary>
        /// Reads ANM from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the ANM data.</param>
        public ANM(Stream stream)
        {
            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);
            
            // Reader ANM's Header.
            Version = br.ReadU8();
            Loop = br.ReadBool8();
            Unknown1 = br.ReadS16();
            KeyFrameCount = br.ReadU32();
            KeyFrameOffset = br.ReadU32();
            Unknown2 = br.ReadS32();
            Unknown3 = br.ReadS32();
            Unknown4 = br.ReadS32();

            /*
                Keyframes - Interpolation type list:
                https://ia800802.us.archive.org/9/items/GCN_SDK_Documentation/Game%20Engine%20Programming.pdf
            */
        }

    }
}
