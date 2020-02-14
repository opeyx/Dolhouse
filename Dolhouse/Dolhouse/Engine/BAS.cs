using Dolhouse.Binary;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (B)inary (A)udio (S)equence
    /// </summary>
    public class BAS
    {

        #region Properties

        /// <summary>
        /// The amount of entries in BAS.
        /// </summary>
        public ushort EntryCount { get; set; }

        /// <summary>
        /// Unknown 1. (Soundgroup?)
        /// (Thanks to @XAYRGA)
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// Padding.
        /// </summary>
        public byte[] Padding { get; set; }

        /// <summary>
        /// List of entries stored in BAS.
        /// </summary>
        public List<BasEntry> Entries { get; set; }

        #endregion


        /// <summary>
        /// Reads BAS from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the BAS data.</param>
        public BAS(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read entry count.
            EntryCount = br.ReadU16();

            // Read unknown 1.
            Unknown1 = br.Read();

            // Read padding. (5 byte)
            Padding = br.Read(5);

            // Define a new list to hold the BAS entries.
            Entries = new List<BasEntry>();

            // Loop through the BAS entries.
            for (int i = 0; i < EntryCount; i++)
            {

                // Add the read entry to the Entries list.
                Entries.Add(new BasEntry(br));
            }
        }

        /// <summary>
        /// Creates a stream from this BAS.
        /// </summary>
        /// <returns>The BAS as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our JMP data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write entry count.
            bw.WriteU16(EntryCount);

            // Write unknown 1.
            bw.Write(Unknown1);

            // Write padding.
            bw.Write(Padding);

            // Loop through the BAS entries.
            for (int i = 0; i < Entries.Count; i++)
            {

                // Write entry.
                Entries[i].Write(bw);
            }

            // Returns the BAS as a stream.
            return stream;
        }
    }


    /// <summary>
    /// BAS Entry
    /// </summary>
    public class BasEntry
    {

        #region

        /// <summary>
        /// Id of sound to play.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gain.
        /// (Thanks to @XAYRGA)
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// Delay / Length.
        /// (Thanks to @XAYRGA)
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// Pitch.
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// Unknown 1. (Interval?)
        /// </summary>
        public int Unknown1 { get; set; }


        /// <summary>
        /// Left / right balance.
        /// </summary>
        public byte Balance { get; set; }

        /// <summary>
        /// Unknown 2. (Always 0x00/0x0A)
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <summary>
        /// Unknown 3.
        /// </summary>
        public byte Unknown3 { get; set; }

        /// <summary>
        /// Unknown 4. (Always 0x40)
        /// </summary>
        public byte Unknown4 { get; set; }

        /// <summary>
        /// Padding. (8 bytes)
        /// </summary>
        public uint[] Padding { get; set; }

        #endregion


        /// <summary>
        /// Read a single entry from JMP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public BasEntry(DhBinaryReader br)
        {

            // Read Id.
            Id = br.ReadU32();

            // Read Gain.
            Gain = br.ReadF32();

            // Read Delay / Length.
            Delay = br.ReadU32();

            // Read Pitch.
            Pitch = br.ReadF32();

            // Read Unknown 1.
            Unknown1 = br.ReadS32();

            // Read Balance.
            Balance = br.Read();

            // Read Unknown 2.
            Unknown2 = br.Read();

            // Read Unknown 3.
            Unknown3 = br.Read();

            // Read Unknown 4.
            Unknown4 = br.Read();

            // Read Padding.
            Padding = br.ReadU32s(2);
        }

        /// <summary>
        /// Write a single entry to stream.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write Id.
            bw.WriteU32(Id);

            // Write Gain.
            bw.WriteF32(Gain);

            // Write Delay / Length.
            bw.WriteF32(Delay);

            // Write Pitch.
            bw.WriteF32(Pitch);

            // Write Unknown 1.
            bw.WriteS32(Unknown1);

            // Write Balance.
            bw.Write(Balance);

            // Write Unknown 3.
            bw.Write(Unknown2);

            // Write Unknown 4.
            bw.Write(Unknown3);

            // Write Unknown 5.
            bw.Write(Unknown4);

            // Write Padding 2.
            bw.WriteU32s(Padding);
        }
    }
}
