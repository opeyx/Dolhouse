using Dolhouse.Binary;
using Dolhouse.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dolhouse.Archive
{
    /// Many thanks to Sage-Of-Mirrors/LordNed for code reference:
    /// https://github.com/LordNed/WArchive-Tools/
    public static class RARC
    {
        public static VirtDirectory LoadRarc(byte[] data)
        {
            if (Yay0.IsCompressed(data))
            {
                data = Yay0.Decompress(data);
            }

            DhBinaryReader br = new DhBinaryReader(data, DhEndian.Big);

            if (br.ReadStr(4) != "RARC") throw new InvalidDataException("No valid RARC signature was found!");

            List<RarcNode> nodes = new List<RarcNode>();
            List<RarcEntry> entries = new List<RarcEntry>();

            // read header
            // RARC here
            br.Skip(4);
            br.Skip(4);
            var dataOffset = br.ReadU32() + 0x20;
            br.Skip(4);
            br.Skip(4);
            br.Skip(4);
            br.Skip(4);

            // read infoblock
            var NodeCount = br.ReadU32();
            var NodeOffset = br.ReadU32() + 0x20;
            var EntryCount = br.ReadU32();
            var EntryOffset = br.ReadU32() + 0x20;
            br.Skip(4);
            var StringTableOffset = br.ReadU32() + 0x20;
            br.Skip(2);
            br.Skip(2);
            br.Skip(4);

            br.Goto(EntryOffset);

            // read entries
            for (int i = 0; i < EntryCount; i++)
            {
                RarcEntry entry = new RarcEntry()
                {
                    Id = br.ReadU16(),
                    NameHash = br.ReadU16(),
                    Type = br.ReadU16(),
                    NameOffset = br.ReadU16(),
                    DataOffset = br.ReadU32(),
                    DataLength = br.ReadU32()
                };

                if (entry.Type == 0x1100)
                {
                    entry.Data = br.ReadAt(dataOffset + entry.DataOffset, (int)entry.DataLength);
                }
                entry.MemoryPointer = br.ReadU32();
                entry.Name = br.ReadStrAt(StringTableOffset + entry.NameOffset);

                entries.Add(entry);
            }

            br.Goto(NodeOffset);

            // read nodes
            for (int i = 0; i < NodeCount; i++)
            {
                RarcNode rarcNode = new RarcNode()
                {
                    Id = br.ReadStr(4),
                    NameOffset = br.ReadU32(),
                    NameHash = br.ReadU16(),
                    EntryCount = br.ReadU16(),
                    FirstEntryIndex = br.ReadU32()
                };

                rarcNode.Name = br.ReadStrAt(StringTableOffset + rarcNode.NameOffset);
                rarcNode.Entries = entries.GetRange((int)rarcNode.FirstEntryIndex, (int)rarcNode.EntryCount);

                nodes.Add(rarcNode);
            }

            List<VirtDirectory> virtDirectories = new List<VirtDirectory>(nodes.Count);
            foreach (RarcNode rarcNode in nodes)
            {
                virtDirectories.Add(new VirtDirectory(rarcNode.Name, Guid.Empty));
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                RarcNode rarcNode = nodes[i];

                for (int y = 0; y < nodes[i].Entries.Count; y++)
                {

                    if (rarcNode.Entries[y].Name == "." || rarcNode.Entries[y].Name == "..")
                        continue;

                    if (rarcNode.Entries[y].Type == (ushort)NodeType.Directory)
                    {
                        var virtDirectory = virtDirectories[(int)rarcNode.Entries[y].DataOffset];
                        virtDirectory.ParentGuid = virtDirectories[i].Guid;
                        virtDirectory.Name = rarcNode.Entries[y].Name;

                        virtDirectories[i].Children.Add(virtDirectory);
                    }
                    else
                    {
                        VirtFile virtFile = new VirtFile(rarcNode.Entries[y].Name, virtDirectories[i].Guid, rarcNode.Entries[y].Data);
                        virtDirectories[i].Children.Add(virtFile);
                    }
                }
            }

            return virtDirectories.Count > 0 ? virtDirectories[0] : null;
        }
    }

    public class RarcNode
    {
        public string Id { get; set; }
        public uint NameOffset { get; set; }
        public ushort NameHash { get; set; }
        public ushort EntryCount { get; set; }
        public uint FirstEntryIndex { get; set; }
        public string Name { get; set; }
        public List<RarcEntry> Entries { get; set; }
    }

    public class RarcEntry
    {
        public ushort Id { get; set; }
        public ushort NameHash { get; set; }
        public ushort Type { get; set; }
        public ushort NameOffset { get; set; }
        public uint DataOffset { get; set; } // If this is a directory, the is the index of the directory's node. If it's a file, it's the offset to the file's data. 
        public uint DataLength { get; set; }
        public uint MemoryPointer { get; set; }

        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
