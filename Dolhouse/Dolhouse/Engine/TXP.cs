using Dolhouse.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (T)e(X)ture (P)attern 
    /// </summary>
    public class TXP 
    {

        #region Properties

        /// <summary>
        /// Unknown 1.
        /// <summary>
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2.
        /// <summary>
        public ushort Unknown2 { get; set; }

        /// <summary>
        /// Entry Count.
        /// <summary>
        public ushort EntryCount { get; set; }

        /// <summary>
        /// KeyFrame Count.
        /// <summary>
        public ushort KeyFrameCount { get; set; }

        /// <summary>
        /// KeyFrame Offset.
        /// <summary>
        public uint KeyFrameOffset { get; set; }

        /// <summary>
        /// List of entries.
        /// <summary>
        public List<TxpEntry> Entries { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty TXP.
        /// </summary>
        public TXP()
        {

            // Set Unknown 1.
            Unknown1 = 1;

            // Set Unknown 2.
            Unknown2 = 0;

            // Set Entry Count.
            EntryCount = 1;

            // Set Keyframe Count.
            KeyFrameCount = 1;

            // Read Keyframe Offset.
            KeyFrameOffset = 24;

            // Define a new list to hold the TXP's entries.
            Entries = new List<TxpEntry>();

            // Add a TXP entry.
            Entries.Add(new TxpEntry());
        }

        /// <summary>
        /// Reads TXP from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the TXP data.</param>
        public TXP(Stream stream)
        {

            // Define a new binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read Unknown 1.
            Unknown1 = br.ReadU16();

            // Read Unknown 2.
            Unknown2 = br.ReadU16();

            // Read Entry Count.
            EntryCount = br.ReadU16();

            // Read Keyframe Count.
            KeyFrameCount = br.ReadU16();

            // Read Keyframe Offset.
            KeyFrameOffset = br.ReadU32();

            // Initialize the list to hold our entries.
            Entries = new List<TxpEntry>();

            // Loop through entries.
            for(int i = 0; i < EntryCount; i++)
            {

                // Read entry.
                Entries.Add(new TxpEntry(br, KeyFrameCount));
            }
        }

        /// <summary>
        /// Creates a stream from this TXP.
        /// </summary>
        /// <returns>The TXP as a stream.</returns>
        public Stream Write()
        {
            // Buffer for new TXP File
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write Unknown 1.
            bw.WriteU16(Unknown1);

            // Write Unknown 2.
            bw.WriteU16(Unknown2);

            // Write Entry Count.
            bw.WriteU16((ushort)Entries.Count);

            // Check if Entry Count is greater than 0.
            if (Entries.Count > 0)
            {
                // Write Keyframe Count.
                bw.WriteU16((ushort)Entries[0].Indices.Count);
            }
            else
            {
                // Write 0.
                bw.WriteU16(0);
            }

            // Write Keyframe Offset.
            bw.WriteU32((uint)(12 + (Entries.Count * 12)));

            // Loop through entries.
            for(int i = 0; i < Entries.Count; i++)
            {

                // Write entry header.
                Entries[i].WriteHeader(bw);
            }

            // Define a list to hold the offsets for each keyframe offsets.
            List<uint> indicesOffsets = new List<uint>();

            // Loop through entries.
            for (int i = 0; i < Entries.Count; i++)
            {

                // Save the current offset
                indicesOffsets.Add((uint)bw.Position());

                // Write entry indices.
                Entries[i].WriteIndices(bw);
            }

            // Loop through entries.
            for (int i = 0; i < Entries.Count; i++)
            {

                // Go to current entry's indices offset value.
                bw.Goto(0x0C + (i * 12) + 8);

                // Write indices offset.
                bw.WriteU32(indicesOffsets[i]);
            }

            // Return to end of file.
            bw.Back(0);

            // Write padding to nearest whole 32 bytes.
            bw.WritePadding32();

            // Return the TXP as a stream
            return stream;
        }
    }    

    /// <summary>
    /// TXP Entry
    /// </summary>
    public class TxpEntry
    {

        #region Properties

        /// <summary>
        /// Unknown 1.
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// Material Index.
        /// </summary>
        public ushort MaterialIndex { get; set; }

        /// <summary>
        /// Unknown 2.
        /// </summary>
        public int Unknown2 { get; set; }

        /// <summary>
        /// Indices Offset.
        /// </summary>
        public uint IndicesOffset { get; set; }

        /// <summary>
        /// MDL Texture Objects Indices.
        /// </summary>
        public List<ushort> Indices { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty TxpEntry.
        /// </summary>
        public TxpEntry()
        {

            // Set Unknown 1.
            Unknown1 = 0;

            // Set Material Index.
            MaterialIndex = 0;

            // Set Unknown 2.
            Unknown2 = 0;

            // Set Indices Offset.
            IndicesOffset = 24;

            // Set Indices.
            Indices = new List<ushort>();

            // Add index.
            Indices.Add(0);
        }

        /// <summary>
        /// Read a single entry in TXP.
        /// </summary>
        /// <param name="br">The binaryreader to read with.</param>
        /// <param name="keyFrameCount">The amount of keyframes in each entry.</param>
        public TxpEntry(DhBinaryReader br, ushort keyFrameCount)
        {

            // Read Unknown 1.
            Unknown1 = br.ReadS16();
            
            //Read Material Index.
            MaterialIndex = br.ReadU16();

            // Read Unknown 2.
            Unknown2 = br.ReadS32();

            // Read Indices Offset.
            IndicesOffset  = br.ReadU32();

            // Read Indices.
            Indices = br.ReadU16sAt(IndicesOffset, keyFrameCount).ToList();
        }

        /// <summary>
        /// Write a single entry header.
        /// </summary>
        /// <param name="bw">The binarywriter to write with.</param>
        public void WriteHeader(DhBinaryWriter bw)
        {

            // Write Unknown 1.
            bw.WriteS16(Unknown1);

            // Write Material Index.
            bw.WriteU16(MaterialIndex);

            // Write Unknown 2.
            bw.WriteS32(Unknown2);

            // Write Indices Offset.
            bw.WriteU32(IndicesOffset);
        }

        /// <summary>
        /// Write entry data.
        /// </summary>
        /// <param name="bw">The binarywriter to write with.</param>
        public void WriteIndices(DhBinaryWriter bw)
        {

            // Loop through indices.
            for (int i = 0; i < Indices.Count; i++)
            {

                // Write index.
                bw.WriteU16(Indices[i]);
            }
        }
    }
}