using Dolhouse.Binary;
using Dolhouse.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolhouse.Engine
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
        /// Initialize a new empty JMP.
        /// </summary>
        public JMP()
        {

            // Set JMP's Header
            EntryOffset = 0;
            EntrySize = 0;

            // Define a new list to hold the JMP's Fields.
            Fields = new List<JField>();

            // Define a new list to hold the JMP's Entries.
            Entries = new List<JEntry>();
        }

        /// <summary>
        /// Reads JMP from a byte array.
        /// </summary>
        /// <param name="stream">The byte array containing the JMP data.</param>
        public JMP(byte[] data)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(data, DhEndian.Big);
            
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
        /// Creates a byte array from this JMP.
        /// </summary>
        /// <returns>The JMP as a byte array.</returns>
        public byte[] Write()
        {

            // Define a stream to hold our JMP data.
            MemoryStream stream = new MemoryStream();

            // Define a binary writer to write with.
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
                bw.Goto(EntryOffset + (i * EntrySize));
                Entries[i].Write(bw, Fields);
            }

            // Back up from the end of the file.
            bw.Back(0);

            // Pad file with @'s to nearest whole 32 bytes.
            bw.WritePadding32('@');

            // Returns the JMP as a byte array.
            return stream.ToArray();
        }
    }

    /// <summary>
    /// JMP Field
    /// </summary>
    public class JField
    {

        #region Field Properties

        /// <summary>
        /// Field Name
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
        /// Initialize a new empty field.
        /// </summary>
        public JField()
        {

            // Set field's hash.
            Hash = 0;

            // Set field's bitmask.
            Bitmask = 0;

            // Set field's offset.
            Offset = 0;

            // Set field's shift.
            Shift = 0;

            // Set field's type.
            Type = JFieldType.INTEGER;

            // Set field's name.
            Name = "";
        }

        /// <summary>
        /// Read a single field from JMP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public JField(DhBinaryReader br)
        {

            // Read field's hash.
            Hash = br.ReadU32();

            // Read field's bitmask.
            Bitmask = br.ReadU32();

            // Read field's offset.
            Offset = br.ReadU16();

            // Read field's shift.
            Shift = br.ReadS8();

            // Read field's type.
            Type = (JFieldType)br.ReadU8();

            // Resolve field's hash to get field name.
            Name = JMPUtils.HashToName(Hash);
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

        #region Properties

        /// <summary>
        /// The values stored within this entry.
        /// </summary>
        public object[] Values { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty entry.
        /// </summary>
        /// <param name="fields">List of fields in JMP.</param>
        public JEntry(List<JField> fields)
        {

            // Define a new object array to hold entry's values.
            Values = new object[fields.Count];

            // Loop through each field in the JMP.
            for (int i = 0; i < fields.Count; i++)
            {

                // Define a new object to hold our data.
                object value;

                // Check which type the current value is.
                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        // Set the data as a integer.
                        value = 0;
                        break;
                    case JFieldType.STRING:
                        // Set the data as a 32-byte long string.
                        value = "";
                        break;
                    case JFieldType.FLOAT:
                        // Set the data as a float32.
                        value = 0.0f;
                        break;
                    default:
                        // Something went horribly wrong.
                        throw new InvalidDataException();
                }
                // Set the value of this entry's value's data to the value we just looped through.
                Values[i] = value;
            }
        }

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
                        // Read the data as a 32-byte long string.
                        value = br.ReadFixedStr(32);
                        break;
                    case JFieldType.FLOAT:
                        // Read the data as a float32.
                        value = br.ReadF32();
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
        /// Full credits for the 'packing' snippet goes to arookas:
        /// https://github.com/arookas/jmpman/blob/master/jmpman/jmp.cs
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw, List<JField> fields)
        {

            // Save the current position.
            long currentPosition = bw.Position();

            // Define a buffer to hold packed int values.
            Dictionary<ushort, uint> buffer = new Dictionary<ushort, uint>(fields.Count);

            // Loop through each value in the entry.
            for (int i = 0; i < fields.Count; i++)
            {

                // Seek from the current position to value's offset in the entry.
                bw.Sail(fields[i].Offset);

                // Check which type the current value is.
                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        // Write the value as a integer. TODO: Add pack values.
                        int value = int.Parse(Values[i].ToString());

                        // Check if current field has a bitmask.
                        if(fields[i].Bitmask == 0xFFFFFFFF)
                        {
                            // Value is not packed, write data directly.
                            bw.WriteS32((value));
                        }
                        else
                        {
                            // Value is packed, data will be added to the buffer.
                            if (!buffer.ContainsKey(fields[i].Offset))
                            {
                                // Since no key exists yet, create one.
                                buffer[fields[i].Offset] = 0u;
                            }
                            // Add the packet int value to the buffer.
                            buffer[fields[i].Offset] |= ((uint)(value << fields[i].Shift) & fields[i].Bitmask);
                        }
                        break;
                    case JFieldType.STRING:
                        // Write the value as a string.
                        bw.WriteFixedStr(Values[i].ToString(), 32);
                        break;
                    case JFieldType.FLOAT:
                        // Write the value as a float32.
                        bw.WriteF32(float.Parse(Values[i].ToString()));
                        break;
                    default:
                        // Something went horribly wrong.
                        throw new InvalidDataException();
                }

                // Write out the packed int's buffer.
                foreach (var data in buffer)
                {
                    // 
                    bw.Goto(currentPosition + data.Key);
                    // Write the packed int value.
                    bw.WriteU32(data.Value);
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


    /// <summary>
    /// JMP Utilities
    /// </summary>
    public static class JMPUtils
    {

        #region Properties

        /// <summary>
        /// Dictionary to hold field hashes and names.
        /// </summary>
        private static Dictionary<uint, string> FieldDictionary = FieldHashes();

        #endregion


        /// <summary>
        /// Attempt to find field name from the field hash.
        /// </summary>
        /// <param name="hash">Hash for the field you want the name for.</param>
        /// <returns>The name for this field.</returns>
        public static string HashToName(uint hash)
        {

            // Attempt to resolve the hash into a known name.
            if (FieldDictionary.TryGetValue(hash, out string fieldName))
            {

                // Name was resolved, return correct field name.
                return fieldName;
            }
            else
            {

                // Name could not be resolved, return hash as string.
                return hash.ToString();
            }
        }


        /// <summary>
        /// Generates a dictionary from the internal jmp.txt file.
        /// </summary>
        /// <returns>A dictionary of the field hash and name.</returns>
        private static Dictionary<uint, string> FieldHashes()
        {

            // Read the internal names file, split it by default delimiters.
            string[] lines = Resources.jmp.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            // Define a temporary dictionary to hold our field hashes and names.
            Dictionary<uint, string> fieldHashes = new Dictionary<uint, string>();

            // Define a temporary list to hold our jmp file lines, aswell as removing duplicates.
            List<string> fieldNames = lines.Distinct().ToList();

            // Loop through each of the lines.
            for (int i = 0; i < fieldNames.Count; i++)
            {
                // Check if the current line is empty or starts with a # (comment)
                if (string.IsNullOrWhiteSpace(fieldNames[i]) || fieldNames[i].StartsWith("#"))
                {
                    // Skip hashing the current line.
                    continue;
                }

                // Hash the current line and add it plus the field name to the fieldhashes dictionary.
                fieldHashes.Add(Calculate(fieldNames[i]), fieldNames[i]);
            }

            // Return the fieldHashes dictionary.
            return fieldHashes;
        }


        /// <summary>
        /// Calculate hash from string.
        /// Full credits for this snippet goes to arookas:
        /// https://github.com/arookas/jmpman/blob/master/jmpman/hash.cs
        /// </summary>
        /// <param name="data">String to calculate hash from.</param>
        /// <returns>Calculated string hash.</returns>
        private static uint Calculate(string data)
        {

            // Check if data is equals to null.
            if (data == null)
            {

                // Cannot hash null data, throw exception.
                throw new ArgumentNullException("data");
            }

            // Return the calculated hash from the input data.
            return Calculate(System.Text.Encoding.ASCII.GetBytes(data));
        }

        /// <summary>
        /// Calculate hash from bytes.
        /// Full credits for this snippet goes to arookas:
        /// https://github.com/arookas/jmpman/blob/master/jmpman/hash.cs
        /// </summary>
        /// <param name="data">Byte array to calculate hash from.</param>
        /// <returns>Calculated bytes hash.</returns>
        private static uint Calculate(byte[] data)
        {

            // Check if data is equals to null.
            if (data == null)
            {

                // Cannot hash null data, throw exception.
                throw new ArgumentNullException("data");
            }

            // Define variable to temporary hold our hash.
            var hash = 0u;

            // Loop through our input data.
            for (var i = 0; i < data.Length; ++i)
            {
                hash <<= 8;
                hash += data[i];
                var r6 = unchecked((uint)((4993ul * hash) >> 32));
                var r0 = unchecked((byte)((((hash - r6) / 2) + r6) >> 24));
                hash -= r0 * 33554393u;
            }

            // Return the calculated hash.
            return hash;
        }
    }
}
