using Dolhouse.Binary;
using Dolhouse.Type;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (T)i(M)ing (B)inary
    /// </summary>
    public class TMB
    {

        #region Properties

        /// <summary>
        /// Version. (Assumption!)
        /// </summary>
        public ushort Version { get; set; }

        /// <summary>
        /// Hash. (Assumption!)
        /// </summary>
        public ushort Hash { get; set; }

        /// <summary>
        /// Offset to info data.
        /// </summary>
        public uint InfoOffset { get; set; }

        /// <summary>
        /// List of entries.
        /// </summary>
        public List<Vec3> Entries { get; set; }

        /// <summary>
        /// Name. (For internal use?)
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Entry count.
        /// </summary>
        public uint EntryCount;

        /// <summary>
        /// Float count. (Assumption!)
        /// </summary>
        public uint FloatCount { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty TMB.
        /// </summary>
        public TMB()
        {

            // Set version.
            Version = 1;

            // Set hash.
            Hash = 0;

            // Set info offset.
            InfoOffset = 8;

            // Set name.
            Name = "";

            // Set entry count.
            EntryCount = 0;

            // Set float count.
            FloatCount = 3;

            // Define a new list to hold the entries.
            Entries = new List<Vec3>();
        }

        /// <summary>
        /// Reads TMB from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the TMB data.</param>
        public TMB(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read version.
            Version = br.ReadU16();

            // Read hash.
            Hash = br.ReadU16();

            // Read info offset.
            InfoOffset = br.ReadU32();

            // Read name.
            Name = br.ReadFixedStrAt(InfoOffset, 28);

            // Read entry count.
            EntryCount = br.ReadU32At(InfoOffset + 28);

            // Read float count.
            FloatCount = br.ReadU32At(InfoOffset + 32);

            // Read entries.
            Entries = br.ReadVec3s((int)EntryCount).ToList();
        }

        /// <summary>
        /// Creates a stream from this TMB.
        /// </summary>
        /// <returns>The TMB as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our PRM data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write version.
            bw.WriteU16(Version);

            // Write hash.
            bw.WriteU16(Hash);

            // Write info offset.
            bw.WriteU32(InfoOffset);

            // Write entries.
            bw.WriteVec3(Entries.ToArray());

            // Write name.
            bw.WriteFixedStr(Name, 28);

            // Write entry count.
            bw.WriteU32((uint)Entries.Count);

            // Write float count.
            bw.WriteU32(FloatCount);

            // Write padding.
            bw.WritePadding32();

            // Returns the TMB as a stream.
            return stream;
        }
    }
}
