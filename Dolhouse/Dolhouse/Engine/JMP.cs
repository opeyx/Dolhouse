using Dolhouse.Binary;
using Dolhouse.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (J)System (M)ap (P)roperties
    /// </summary>
    [Serializable]
    public class JMP
    {

        #region Properties

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

            Fields = new List<JField>();
            Entries = new List<JEntry>();
        }


        /// <summary>
        /// Initialize a new JMP with fields.
        /// </summary>
        public JMP(List<JField> fields)
        {

            Fields = new List<JField>();
            Entries = new List<JEntry>();

            foreach (JField field in fields)
            {
                Fields.Add(field.Clone());
            }
        }

        /// <summary>
        /// Reads JMP from a byte array.
        /// </summary>
        /// <param name="stream">The byte array containing the JMP data.</param>
        public JMP(byte[] data)
        {

            DhBinaryReader br = new DhBinaryReader(data, DhEndian.Big);
            
            uint entryCount = br.ReadU32();
            uint fieldCount = br.ReadU32();
            uint entryOffset = br.ReadU32();
            uint entrySize = br.ReadU32();

            Fields = new List<JField>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new JField(br));
            }

            br.Goto(0);

            Entries = new List<JEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                br.Goto(entryOffset + (i * entrySize));
                Entries.Add(new JEntry(br, Fields));
            }
        }

        /// <summary>
        /// Creates a byte array from this JMP.
        /// </summary>
        /// <returns>The JMP as a byte array.</returns>
        public byte[] Write()
        {

            MemoryStream stream = new MemoryStream();
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            uint entryOffset = CalculateEntryOffset();
            uint entrySize = CalculateEntrySize();
            bw.WriteU32((uint)Entries.Count);
            bw.WriteU32((uint)Fields.Count);
            bw.WriteU32(entryOffset);
            bw.WriteU32(entrySize);

            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Write(bw);
            }

            for (int i = 0; i < Entries.Count; i++)
            {
                bw.Goto(entryOffset + (i * entrySize));
                Entries[i].Write(bw, Fields);
            }

            bw.Back(0);

            bw.WritePadding32('@');

            return stream.ToArray();
        }

        /// <summary>
        /// Helper method for getting byte count of entry.
        /// </summary>
        /// <returns>The offset to entries.</returns>
        public uint CalculateEntryOffset()
        {

            if (Fields == null || Fields.Count == 0)
            {
                return 16;
            }

            return (uint)(16 + (12 * Fields.Count));
        }

        /// <summary>
        /// Helper method for getting byte count of entry.
        /// </summary>
        /// <returns>The entry's byte count.</returns>
        public uint CalculateEntrySize()
        {
            if (Fields == null || Fields.Count == 0)
            {
                return 0;
            }

            uint size = 0;
            foreach (JField field in Fields)
            {
                uint current = (uint)(field.Offset + GetFieldSize(field));
                if (current > size)
                {
                    size = current;
                }
            }

            return size;
        }

        /// <summary>
        /// Helper method for getting byte count of field.
        /// </summary>
        /// <param name="field">The field to get the size of.</param>
        /// <returns>The field's byte count.</returns>
        public int GetFieldSize(JField field)
        {
            switch (field.Type)
            {
                case JFieldType.INTEGER:
                    return 4;
                case JFieldType.FLOAT:
                    return 4;
                case JFieldType.STRING:
                    return 32;
            }
            return 0;
        }

    }

    /// <summary>
    /// JMP Field
    /// </summary>
    [Serializable]
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

            Hash = 0;
            Bitmask = 0;
            Offset = 0;
            Shift = 0;
            Type = JFieldType.INTEGER;
            Name = "";
        }

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
            Name = JMPUtils.HashToName(Hash);
        }

        /// <summary>
        /// Write a single field to stream.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            bw.WriteU32(Hash);
            bw.WriteU32(Bitmask);
            bw.WriteU16(Offset);
            bw.WriteS8(Shift);
            bw.WriteU8((byte)Type);
        }

        /// <summary>
        /// Make deep clone the current field.
        /// </summary>
        /// <returns>A deep clone of the current field.</returns>
        public JField Clone()
        {

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (JField)formatter.Deserialize(ms);
            }
        }
    }

    /// <summary>
    /// JMP Entry
    /// </summary>
    [Serializable]
    public class JEntry
    {

        #region Properties

        /// <summary>
        /// The values stored within this entry.
        /// </summary>
        public Dictionary<string, object> Values { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty entry.
        /// </summary>
        /// <param name="fields">List of fields in JMP.</param>
        public JEntry(List<JField> fields)
        {

            Values = new Dictionary<string, object>();

            for (int i = 0; i < fields.Count; i++)
            {

                object value;

                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        value = 0;
                        break;
                    case JFieldType.STRING:
                        value = "";
                        break;
                    case JFieldType.FLOAT:
                        value = 0.0f;
                        break;
                    default:
                        throw new InvalidDataException($"{fields[i].Type} is not a valid jmp entry type!");
                }

                Values.Add(fields[i].Name, value);
            }
        }

        /// <summary>
        /// Read a single entry from JMP.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="fields">List of fields in JMP.</param>
        public JEntry(DhBinaryReader br, List<JField> fields)
        {

            long currentPosition = br.Position();

            Values = new Dictionary<string, object>();
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
                        value = br.ReadFixedStr(32);
                        break;
                    case JFieldType.FLOAT:
                        value = br.ReadF32();
                        break;
                    default:
                        throw new InvalidDataException($"{fields[i].Type} is not a valid jmp entry type!");
                }

                Values.Add(fields[i].Name, value);

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

            long currentPosition = bw.Position();

            Dictionary<ushort, uint> buffer = new Dictionary<ushort, uint>(fields.Count);
            for (int i = 0; i < fields.Count; i++)
            {

                bw.Sail(fields[i].Offset);

                switch (fields[i].Type)
                {
                    case JFieldType.INTEGER:
                        int value = Convert.ToInt32(Values.Values.ElementAt(i));

                        if (fields[i].Bitmask == 0xFFFFFFFF)
                        {

                            // not packed, write data directly.
                            bw.WriteS32((value));
                        }
                        else
                        {

                            if (!buffer.ContainsKey(fields[i].Offset))
                            {
                                buffer[fields[i].Offset] = 0u;
                            }

                            // packed, add data to buffer.
                            buffer[fields[i].Offset] |= ((uint)(value << fields[i].Shift) & fields[i].Bitmask);
                        }
                        break;
                    case JFieldType.STRING:
                        bw.WriteFixedStr(Convert.ToString(Values.Values.ElementAt(i)), 32);
                        break;
                    case JFieldType.FLOAT:
                        bw.WriteF32(Convert.ToSingle(Values.Values.ElementAt(i)));
                        break;
                    default:
                        throw new InvalidDataException($"{fields[i].Type} is not a valid jmp entry type!");
                }

                foreach (var data in buffer)
                {

                    bw.Goto(currentPosition + data.Key);
                    bw.WriteU32(data.Value);
                }

                bw.Goto(currentPosition);
            }
        }

        /// <summary>
        /// Make deep clone the current entry.
        /// </summary>
        /// <returns>A deep clone of the current entry.</returns>
        public JEntry Clone()
        {

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (JEntry)formatter.Deserialize(ms);
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

            if (FieldDictionary.TryGetValue(hash, out string fieldName))
            {
                return fieldName;
            }
            else
            {
                return hash.ToString();
            }
        }


        /// <summary>
        /// Generates a dictionary from the internal jmp.txt file.
        /// </summary>
        /// <returns>A dictionary of the field hash and name.</returns>
        private static Dictionary<uint, string> FieldHashes()
        {

            string[] lines = Resources.jmp.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            Dictionary<uint, string> fieldHashes = new Dictionary<uint, string>();
            List<string> fieldNames = lines.Distinct().ToList();
            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(fieldNames[i]) || fieldNames[i].StartsWith("#"))
                    continue;

                fieldHashes.Add(Calculate(fieldNames[i]), fieldNames[i]);
            }

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

            if (data == null)
                throw new ArgumentNullException("data");

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

            if (data == null)
                throw new ArgumentNullException("data");

            var hash = 0u;
            for (var i = 0; i < data.Length; ++i)
            {
                hash <<= 8;
                hash += data[i];
                var r6 = unchecked((uint)((4993ul * hash) >> 32));
                var r0 = unchecked((byte)((((hash - r6) / 2) + r6) >> 24));
                hash -= r0 * 33554393u;
            }

            return hash;
        }
    }
}
