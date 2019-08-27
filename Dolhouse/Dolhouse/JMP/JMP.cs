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


        /// <summary>
        /// Creates a stream from this JMP.
        /// </summary>
        /// <returns>The JMP as a stream.</returns>
        public Stream Write()
        {
            Stream stream = new MemoryStream();

            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write JMP's Header
            bw.WriteU32((uint)Entries.Count);
            bw.WriteU32((uint)Fields.Count);
            bw.WriteU32(EntryOffset);
            bw.WriteU32(EntrySize);

            // Write JMP's Fields
            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Write(bw);
            }

            // Seek to beginning of file.
            bw.Goto(0);

            // Write JMP's Entries
            for (int i = 0; i < Entries.Count; i++)
            {
                Entries[i].Write(bw, Fields);
            }

            // Returns the JMP as a stream.
            return stream;
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


        /// <summary>
        /// Write a single field to stream.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write the field's hash.
            bw.WriteU32(Hash);

            // Write the field's bitmask.
            bw.WriteU32(Bitmask);

            // Write the field's offset.
            bw.WriteU16(Offset);
            
            // Write the field's shift.
            bw.WriteS8(Shift);

            // Write the field's type.
            bw.WriteU8((byte)Type);
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

            // Save the current position.
            long currentPosition = br.Position();

            // Define a new object array to hold entry's values.
            Values = new object[fields.Count];

            // Loop through each field in the JMP.
            for (int i = 0; i < fields.Count; i++)
            {
                // Seek from the current position to value's offset in the entry.
                br.Sail(fields[i].Offset);

                // Define a new object to hold our data.
                object value;

                // Check which type the current value is.
                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        // Read the data as a integer.
                        value = ((br.ReadS32() & fields[i].Bitmask) >> fields[i].Shift);
                        break;
                    case JFieldType.STRING:
                        // Read the data as a string.
                        value = null; // TODO: Fix this.
                        break;
                    case JFieldType.FLOAT:
                        // Read the data as a float32.
                        value = (float)(Math.Round(br.ReadF32(), 6));
                        break;
                    default:
                        // Something went horribly wrong.
                        throw new InvalidDataException();
                }
                // Set the value of this entry's value's data to the value we just read.
                Values[i] = value;

                // Seek back to the position we saved earlier.
                br.Goto(currentPosition);
            }
        }


        /// <summary>
        /// Write a single entry to stream.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw, List<JField> fields)
        {
            // Save the current position.
            long currentPosition = bw.Position();

            // Loop through each value in the entry.
            for (int i = 0; i < Values.Length; i++)
            {
                // Seek from the current position to value's offset in the entry.
                bw.Sail(fields[i].Offset);

                // Check which type the current value is.
                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        // Write the value as a integer.
                        bw.WriteS32(int.Parse(Values[i].ToString()));
                        break;
                    case JFieldType.STRING:
                        // Write the value as a string.
                        bw.WritePadding(32); // TODO: Fix this.
                        break;
                    case JFieldType.FLOAT:
                        // Write the value as a float32.
                        bw.WriteF32((float)Values[i]);
                        break;
                    default:
                        // Something went horribly wrong.
                        throw new InvalidDataException();
                }

                // Seek back to the position we saved earlier.
                bw.Goto(currentPosition);
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
