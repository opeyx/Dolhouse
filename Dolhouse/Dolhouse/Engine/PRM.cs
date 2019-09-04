using Dolhouse.Binary;
using Dolhouse.Properties;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolhouse.Engine
{
    /// <summary>
    /// (P)a(r)a(m)eter
    /// </summary>
    public class PRM
    {

        #region Properties
        
        public List<PrmEntry> Entries { get; set; }

        #endregion


        /// <summary>
        /// Reads PRM from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the PRM data.</param>
        public PRM(Stream stream)
        {
            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read PRM's parameter entry count.
            uint entryCount = br.ReadU32();

            // Define a new list to hold the parameter entries.
            Entries = new List<PrmEntry>();

            // Loop through the parameter entries.
            for (int i = 0; i < entryCount; i++)
            {

                // Add the read parameter entry to the Entries list.
                Entries.Add(new PrmEntry(br));
            }
        }

        /// <summary>
        /// Creates a stream from this PRM.
        /// </summary>
        /// <returns>The PRM as a stream.</returns>
        public Stream Write()
        {
            // Define a stream to hold our PRM data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write parameter entry count.
            bw.WriteU32((uint)Entries.Count);

            // Loop through parameter entries.
            for (int i = 0; i < Entries.Count; i++)
            {
                // Write a parameter entry.
                Entries[i].Write(bw);
            }

            // Returns the PRM as a stream.
            return stream;
        }
    }

    public class PrmEntry
    {

        #region Properties
        
        /// <summary>
        /// Entry's hash.
        /// </summary>
        public ushort Hash { get; set; }

        /// <summary>
        /// Entry's name's length.
        /// </summary>
        public ushort NameLength { get; set; }

        /// <summary>
        /// Entry's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Entry value's length.
        /// </summary>
        public uint ValueLength { get; set; }

        /// <summary>
        /// Entry's value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Entry's resolved type.
        /// </summary>
        public PrmEntryType Type { get; set; }

        #endregion

        /// <summary>
        /// Read a PrmEntry from PRM.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public PrmEntry(DhBinaryReader br)
        {

            // Read Hash.
            Hash = br.ReadU16();

            // Read NameLength.
            NameLength = br.ReadU16();

            // Read Name.
            Name = br.ReadStr(NameLength);

            // Read ValueLength.
            ValueLength = br.ReadU32();

            // Resolve Type from Hash.
            Type = PRMUtils.HashToType(Hash);

            // Check Type.
            switch (Type)
            {
                case PrmEntryType.BYTE:

                    // Read Value as a byte.
                    Value = br.Read();
                    break;
                case PrmEntryType.SHORT:

                    // Read Value as a short.
                    Value = br.ReadS16();
                    break;
                case PrmEntryType.INT:

                    // Read Value as a int.
                    Value = br.ReadS32();
                    break;
                case PrmEntryType.FLOAT:

                    // Read Value as a float.
                    Value = br.ReadF32();
                    break;
                case PrmEntryType.RGBA:

                    // Read Value as a RGBA.
                    Value = br.ReadS32();
                    break;
                case PrmEntryType.VECTOR3:

                    // Read Value as a Vector3.
                    Value = new Vector3(br.ReadF32(), br.ReadF32(), br.ReadF32());
                    break;
                default:

                    // Read Value as a int. (Unknown)
                    Value = br.ReadS32();
                    break;
            }
        }

        /// <summary>
        /// Write a PrmEntry with specified Binary Writer.
        /// </summary>
        public void Write(DhBinaryWriter bw)
        {
            // Write Hash.
            bw.WriteU16(Hash);

            // Write NameLength.
            bw.WriteU16(NameLength);

            // Write Name.
            bw.WriteStr(Name);

            // Write ValueLength.
            bw.WriteU32(ValueLength);

            // Write Value.
            bw.Write(BitConverter.GetBytes((int)Value));
        }
    }

    /// <summary>
    /// JMP Utilities
    /// </summary>
    public static class PRMUtils
    {

        #region Properties

        /// <summary>
        /// Dictionary to hold entry hashes and types.
        /// </summary>
        private static Dictionary<ushort, byte> EntryDictionary = EntryHashes();

        #endregion


        /// <summary>
        /// Attempt to find entry type from the entry hash.
        /// </summary>
        /// <param name="hash">Hash for the entry you want the type for.</param>
        /// <returns>The type for this entry.</returns>
        public static PrmEntryType HashToType(ushort hash)
        {
            // Attempt to resolve the hash into a known type.
            if (EntryDictionary.TryGetValue(hash, out byte entryType))
            {
                // Type was resolved, return correct entry type.
                return (PrmEntryType)entryType;
            }
            else
            {
                // Type could not be resolved, return type as unknown.
                return PrmEntryType.UNKNOWN;
            }
        }


        /// <summary>
        /// Generates a dictionary from the internal prm.txt file.
        /// </summary>
        /// <returns>A dictionary of the entry hash and type.</returns>
        private static Dictionary<ushort, byte> EntryHashes()
        {

            // Read the internal prm file, split it by default delimiters.
            string[] lines = Resources.prm.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            // Define a temporary dictionary to hold our entry types and types.
            Dictionary<ushort, byte> entryTypes = new Dictionary<ushort, byte>();

            // Define a temporary list to hold our prm file lines, aswell as removing duplicates.
            List<string> prmLines = lines.Distinct().ToList();

            // Loop through each of the lines.
            for (int i = 0; i < prmLines.Count; i++)
            {
                // Check if the current line is empty or starts with a # (comment)
                if (string.IsNullOrWhiteSpace(prmLines[i]) || prmLines[i].StartsWith("#"))
                {
                    // Skip hashing the current line.
                    continue;
                }

                string[] entryDetails = prmLines[i].Split(',');

                // Hash the current line and add it plus the field name to the entry types dictionary.
                entryTypes.Add(ushort.Parse(entryDetails[0]), byte.Parse(entryDetails[2]));
            }

            // Return the entryTypes dictionary.
            return entryTypes;
        }
    }

    public enum PrmEntryType
    {
        BYTE = 1,
        SHORT = 2,
        INT = 4,
        FLOAT = 6,
        RGBA = 8,
        VECTOR3 = 12,
        UNKNOWN = 999
    }
}
