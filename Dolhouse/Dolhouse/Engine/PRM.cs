using Dolhouse.Binary;
using Dolhouse.Properties;
using Dolhouse.Type;
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

        /// <summary>
        /// List of entries stored in PRM.
        /// </summary>
        public List<PrmEntry> Entries { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty PRM.
        /// </summary>
        public PRM()
        {

            // Define a new list to hold the parameter entries.
            Entries = new List<PrmEntry>();
        }

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
        public PrmType Type { get; set; }

        #endregion


        /// <summary>
        /// Create a new empty PrmEntry.
        /// </summary>
        public PrmEntry()
        {

            // Set Hash.
            Hash = 0;

            // Set NameLength.
            NameLength = 0;

            // Set Name.
            Name = "";

            // Set ValueLength.
            ValueLength = 1;

            // Set Value.
            Value = 0;

            // Set Type.
            Type = PrmType.UNKNOWN;
        }

        /// <summary>
        /// Create a new PrmEntry from pre-defined values.
        /// </summary>
        /// <param name="hash">Entry Hash.</param>
        /// <param name="nameLength">Entry Name Length.</param>
        /// <param name="name">Entry Name.</param>
        /// <param name="valueLength">Entry Value Length.</param>
        /// <param name="value">Entry Value.</param>
        /// <param name="type">Entry Type.</param>
        public PrmEntry(ushort hash, ushort nameLength, string name, uint valueLength, object value, PrmType type)
        {

            // Set Hash.
            Hash = hash;

            // Set NameLength.
            NameLength = nameLength;

            // Set Name.
            Name = name;

            // Set ValueLength.
            ValueLength = valueLength;

            // Set Value.
            Value = value;

            // Set Type.
            Type = type;
        }

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
                case PrmType.BYTE:

                    // Read Value as a byte.
                    Value = br.Read();
                    break;
                case PrmType.SHORT:

                    // Read Value as a short.
                    Value = br.ReadS16();
                    break;
                case PrmType.INT:

                    // Read Value as a int.
                    Value = br.ReadS32();
                    break;
                case PrmType.FLOAT:

                    // Read Value as a float.
                    Value = br.ReadF32();
                    break;
                case PrmType.RGBA:

                    // Read Value as a Clr4.
                    Value = br.ReadClr4();
                    break;
                case PrmType.VECTOR3:

                    // Read Value as a Vector3.
                    Value = br.ReadVec3();
                    break;
                default:
                    throw new NotImplementedException("PRM parameter entry type is unknown!");
            }
        }

        /// <summary>
        /// Write a PrmEntry with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
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

            // Check Type.
            switch (Type)
            {
                case PrmType.BYTE:

                    // Write Value as a byte.
                    bw.Write((byte)Value);
                    break;
                case PrmType.SHORT:

                    // Write Value as a short.
                    bw.WriteS16((short)Value);
                    break;
                case PrmType.INT:

                    // Write Value as a int.
                    bw.WriteS32((int)Value);
                    break;
                case PrmType.FLOAT:

                    // Write Value as a float.
                    bw.WriteF32((float)Value);
                    break;
                case PrmType.RGBA:

                    // Write Value as a Clr4.
                    bw.WriteClr4((Clr4)Value);
                    break;
                case PrmType.VECTOR3:

                    // Write Value as a Vector3.
                    bw.WriteVec3((Vec3)Value);
                    break;
                default:
                    throw new NotImplementedException("PRM parameter entry type is unknown!");
            }
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
        public static PrmType HashToType(ushort hash)
        {
            // Attempt to resolve the hash into a known type.
            if (EntryDictionary.TryGetValue(hash, out byte entryType))
            {
                // Type was resolved, return correct entry type.
                return (PrmType)entryType;
            }
            else
            {
                // Type could not be resolved, return type as unknown.
                return PrmType.UNKNOWN;
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
                    // Skip the current line.
                    continue;
                }

                // Split the current prm line by char ','.
                string[] entryDetails = prmLines[i].Split(',');

                // Add the current prm line's hash and type to the entryTypes dictionary.
                entryTypes.Add(ushort.Parse(entryDetails[0]), byte.Parse(entryDetails[2]));
            }

            // Return the entryTypes dictionary.
            return entryTypes;
        }
    }


    /// <summary>
    /// Parameter Entry Type
    /// </summary>
    public enum PrmType
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
