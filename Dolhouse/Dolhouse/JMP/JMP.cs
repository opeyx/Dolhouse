using Dolhouse.Binary;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.JMP
{
    /// <summary>
    /// (J)System (M)ap (P)roperties
    /// </summary>
    public class JMP
    {

        #region Properties

        /// <summary>
        /// Offset to where the entries are listed in JMP.
        /// </summary>
        public uint EntryOffset { get; set; }

        /// <summary>
        /// The size of each entry in JMP.
        /// </summary>
        public uint EntrySize { get; set; }

        /// <summary>
        /// List of fields stored in JMP.
        /// </summary>
        public List<JField> Fields { get; set; }

        /// <summary>
        /// List of entries stored in JMP.
        /// </summary>
        public List<JEntry> Entries { get; set; }

        #endregion

        /// <summary>
        /// Reads JMP from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the JMP data.</param>
        public JMP(Stream stream)
        {
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);
            
            // Read JMP's Header
            uint entryCount = br.ReadU32();
            uint fieldCount = br.ReadU32();
            EntryOffset = br.ReadU32();
            EntrySize = br.ReadU32();

            // Read JMP's Fields
            Fields = new List<JField>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new JField(br));
            }

            // Seek to beginning of file.
            br.Goto(0);

            // Read JMP's Entries
            Entries = new List<JEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                br.Goto(EntryOffset + (i * EntrySize));
                Entries.Add(new JEntry(br, Fields));
            }
        }
    }

    /// <summary>
    /// JMP Field
    /// </summary>
    public class JField
    {

        #region Field Properties

        /// <summary>
        /// Field Name (TODO: Fix this)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Field Hash
        /// </summary>
        public uint Hash { get; set; }

        /// <summary>
        /// Field Bitmask
        /// </summary>
        public uint Bitmask { get; set; }

        /// <summary>
        /// Field Offset
        /// </summary>
        public ushort Offset { get; set; }

        /// <summary>
        /// Field Shift
        /// </summary>
        public sbyte Shift { get; set; }

        /// <summary>
        /// Field Type
        /// </summary>
        public JFieldType Type { get; set; }

        #endregion

        /// <summary>
        /// Read a single field from JMP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public JField(DhBinaryReader br)
        {
            Hash = br.ReadU32();
            Bitmask = br.ReadU32();
            Offset = br.ReadU16();
            Shift = br.ReadS8();
            Type = (JFieldType)br.ReadU8();
            Name = "?";
        }
    }

    /// <summary>
    /// JMP Entry
    /// </summary>
    public class JEntry
    {
        /// <summary>
        /// The values stored within this entry.
        /// </summary>
        public object[] Values { get; set; }

        /// <summary>
        /// Read a single entry from JMP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="fields">List of fields in JMP.</param>
        public JEntry(DhBinaryReader br, List<JField> fields)
        {
            long currentPosition = br.Position();
            Values = new object[fields.Count];

            for (int i = 0; i < fields.Count; i++)
            {
                br.Sail(fields[i].Offset);

                object value;
                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        value = ((br.ReadS32() & fields[i].Bitmask) >> fields[i].Shift);
                        break;
                    case JFieldType.STRING:
                        value = null;
                        break;
                    case JFieldType.FLOAT:
                        value = (float)(Math.Round(br.ReadF32(), 6));
                        break;
                    default:
                        throw new InvalidDataException();
                }
                Values[i] = value;

                br.Goto(currentPosition);
            }
        }
    }

    /// <summary>
    /// JField Type
    /// </summary>
    public enum JFieldType
    {
        INTEGER,
        STRING,
        FLOAT
    }
}
