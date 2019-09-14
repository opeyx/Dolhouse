using Dolhouse.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dolhouse.Archive
{

    /// <summary>
    /// RARC Archive
    /// </summary>
    public class RARC
    {

        #region Properties
        
        /// <summary>
        /// RARC Header.
        /// </summary>
        public RARCHeader Header { get; set; }

        /// <summary>
        /// RARC Info Block.
        /// </summary>
        public RARCInfoBlock InfoBlock { get; set; }

        /// <summary>
        /// List of nodes stored in RARC.
        /// </summary>
        public List<RARCNode> Nodes { get; set; }

        #endregion


        /// <summary>
        /// Reads RARC from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the RARC data.</param>
        public RARC(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read node's header.
            Header = new RARCHeader(br);

            // Read node's info block.
            InfoBlock = new RARCInfoBlock(br);

            // Define a new list to hold the RARC's nodes.
            Nodes = new List<RARCNode>();

            // Loop through RARC's nodes.
            for (uint i = 0; i < InfoBlock.NodeCount; i++)
            {

                // Read node and add it to the list of nodes.
                Nodes.Add(new RARCNode(br, Header, InfoBlock));
            }
        }
    }

    /// <summary>
    /// RARC Header
    /// </summary>
    public class RARCHeader
    {

        #region Properties
        
        /// <summary>
        /// RARC Magic.
        /// </summary>
        public string Magic { get; set; }

        /// <summary>
        /// Total length of file.
        /// </summary>
        public uint FileLength { get; set; }

        /// <summary>
        /// Total length of header.
        /// </summary>
        public uint HeaderLength { get; set; }

        /// <summary>
        /// Offset to data.
        /// </summary>
        public uint DataOffset { get; }

        /// <summary>
        /// Total length of data.
        /// </summary>
        public uint DataLength { get; set; }

        /// <summary>
        /// Unknown 1. (Same as DataLength?)
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2.
        /// </summary>
        public uint Unknown2 { get; set; }

        /// <summary>
        /// Unknown 3.
        /// </summary>
        public uint Unknown3 { get; set; }

        #endregion


        /// <summary>
        /// Read RARC Header.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public RARCHeader(DhBinaryReader br)
        {

            // Read magic.
            Magic = br.ReadStr(4);

            // Check magic and throw an exception if invalid.
            if (Magic != "RARC") throw new InvalidDataException("No valid RARC signature was found!");

            // Read total file length.
            FileLength = br.ReadU32();

            // Read length of header.
            HeaderLength = br.ReadU32();

            // Read offset to data offset. (Relative to RARCHeader)
            DataOffset = br.ReadU32() + 0x20;

            // Read length of data.
            DataLength = br.ReadU32();

            // Read unknown 1.
            Unknown1 = br.ReadU32();

            // Read unknown 2.
            Unknown2 = br.ReadU32();

            // Read unknown 3.
            Unknown3 = br.ReadU32();
        }
    }

    /// <summary>
    /// RARC Info Block
    /// </summary>
    public class RARCInfoBlock
    {

        #region Properties
        
        /// <summary>
        /// Total amount of nodes.
        /// </summary>
        public uint NodeCount { get; set; }
        
        /// <summary>
        /// Offset to the first node.
        /// </summary>
        public uint FirstNodeOffset { get; set; }

        /// <summary>
        /// Total amount of directories.
        /// </summary>
        public uint DirectoryCount { get; set; }

        /// <summary>
        /// Offset to the first directory.
        /// </summary>
        public uint FirstDirectoryOffset { get; set; }
        
        /// <summary>
        /// Total length of StringTable.
        /// </summary>
        public uint StringTableLength { get; set; }

        /// <summary>
        /// Offset to the string table.
        /// </summary>
        public uint StringTableOffset { get; set; }

        /// <summary>
        /// The amount of directories that are files.
        /// </summary>
        public ushort DirectoryFileCount { get; set; }

        /// <summary>
        /// Unknown 1. (Always 0?)
        /// </summary>
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2. (Always 0?)
        /// </summary>
        public uint Unknown2 { get; set; }

        #endregion

        /// <summary>
        /// Read RARC InfoBlock.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public RARCInfoBlock(DhBinaryReader br)
        {

            // Read amount of nodes.
            NodeCount = br.ReadU32();

            // Read offset to first node. (Relative to InfoBlock)
            FirstNodeOffset = br.ReadU32() + 0x20;

            // Read amount of entries.
            DirectoryCount = br.ReadU32();

            // Read offset to first directory. (Relative to InfoBlock)
            FirstDirectoryOffset = br.ReadU32() + 0x20;

            // Read total length of string table.
            StringTableLength = br.ReadU32();

            // Read offset to string table. (Relative to InfoBlock)
            StringTableOffset = br.ReadU32() + 0x20;

            // Read amount of directories that are files.
            DirectoryFileCount = br.ReadU16();

            // Read unknown 1.
            Unknown1 = br.ReadU16();

            // Read unknown 2.
            Unknown2 = br.ReadU32();
        }
    }

    
    /// <summary>
    /// RARC Node
    /// </summary>
    public class RARCNode
    {

        #region Properties
        
        /// <summary>
        /// Node indentifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Node's name offset in string table.
        /// </summary>
        public uint NameOffset { get; set; }

        /// <summary>
        /// Node's name hash.
        /// </summary>
        public ushort Hash { get; set; }

        /// <summary>
        /// Node's amount of directories.
        /// </summary>
        public ushort DirectoryCount { get; set; }

        /// <summary>
        /// Index of node's first directory.
        /// </summary>
        public uint FirstDirectoryIndex { get; set; }

        /// <summary>
        /// List of directories in node.
        /// </summary>
        public List<RARCDirectory> Directories { get; set; }

        /// <summary>
        /// Node's name.
        /// </summary>
        public string Name { get; set; }

        #endregion


        /// <summary>
        /// Read a RARCNode from RARC.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="header">The RARC's Header.</param>
        /// <param name="infoBlock">The RARC's InfoBlock.</param>
        public RARCNode(DhBinaryReader br, RARCHeader header, RARCInfoBlock infoBlock)
        {

            // Read node's identifier.
            Id = br.ReadStr(4);

            // Read node's name offset in string table.
            NameOffset = br.ReadU32();

            // Read node's name hash.
            Hash = br.ReadU16();

            // Read node's amount of directories.
            DirectoryCount = br.ReadU16();

            // Read node's first directory's index.
            FirstDirectoryIndex = br.ReadU32();

            // Save the current position offset.
            long currentPosition = br.Position();

            // Define a new list to hold the RARCNode's directories.
            Directories = new List<RARCDirectory>();

            // Loop through RARCNode's directories.
            for (int i = 0; i < DirectoryCount; i++)
            {
                // Read directory and add it to the node's list of directories.
                Directories.Add(new RARCDirectory(br, header, infoBlock));
            }

            // Go to the string table offset + the current node's name offset.
            br.Goto(infoBlock.StringTableOffset + NameOffset);

            // Read the node's name as a null-terminated string.
            Name = br.ReadStr();

            // Go back to the position we previously saved.
            br.Goto(currentPosition);
        }
    }


    /// <summary>
    /// RARC Directory
    /// </summary>
    public class RARCDirectory
    {

        #region Properties

        /// <summary>
        /// Directory's index.
        /// </summary>
        public ushort Index { get; set; }

        /// <summary>
        /// Directory's hash.
        /// </summary>
        public ushort Hash { get; set; }

        /// <summary>
        /// Directory's type.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Directory's name offset.
        /// </summary>
        public ushort NameOffset { get; set; }

        /// <summary>
        /// Directory's data offset (if file) / node index (if directory).
        /// </summary>
        public uint DataOffsetOrNodeIndex { get; set; } // relative to RarcHeader.DataOffset
        
        /// <summary>
        /// Directory's data length (if file) / 0 (if directory).
        /// </summary>
        public uint DataLength { get; set; }

        /// <summary>
        /// Unknown 1 (Always 0?)
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Directory's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Directory's data.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Flag to determine if directory is a folder.
        /// </summary>
        public bool IsFolder { get; set; }

        #endregion


        /// <summary>
        /// Read a Directory from RARC.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="header">The RARC's Header.</param>
        /// <param name="infoBlock">The RARC's InfoBlock.</param>
        public RARCDirectory(DhBinaryReader br, RARCHeader header, RARCInfoBlock infoBlock)
        {

            // Read directory's index.
            Index = br.ReadU16();

            // Read directory's hash.
            Hash = br.ReadU16();

            // Read directory's type.
            Type = br.ReadU16();

            // Read directory's name offset.
            NameOffset = br.ReadU16();

            // Read directory's data offset / node index.
            DataOffsetOrNodeIndex = br.ReadU32();

            // Read directory's data length / 0.
            DataLength = br.ReadU32();

            // Read unknown 1.
            Unknown1 = br.ReadU32();

            // Save the current position offset.
            long currentPosition = br.Position();

            // Go to the string table offset + the current node's name offset.
            br.Goto(infoBlock.StringTableOffset + NameOffset);

            // Read the node's name as a null-terminated string.
            Name = br.ReadStr();

            // Determine if this Directory is a folder.
            IsFolder = (Type == 0x1100 && Index != 0xFFFF) ? false : true;

            // Check if directory is NOT a folder.
            if (!IsFolder)
            {
                // Go to data offset + current directory's data offset.
                br.Goto(header.DataOffset + DataOffsetOrNodeIndex);

                // Read current directory's data.
                Data = br.Read((int)DataLength);
            }

            // Go back to the position we previously saved.
            br.Goto(currentPosition);
        }
    }


    /// <summary>
    /// RARC Utilities
    /// </summary>
    public class RARCUtils
    {

        /// <summary>
        /// RARC Name Hashing Method.
        /// Full credits to Wexos:
        /// http://wiki.tockdom.com/w/index.php?title=RARC_(File_Format)&oldid=117549
        /// </summary>
        /// <param name="name">Name to hash.</param>
        /// <returns>Hashed hame as a unsigned short.</returns>
        public static ushort Hash(string name)
        {

            // Define variable to hold our hash value.
            ushort Hash = 0;

            // Loop through each character of our name.
            for (int i = 0; i < name.Length; i++)
            {
                // Multiple the hash value by 3.
                Hash *= 3;

                // Add the current character's value to the  hash value.
                Hash += Encoding.ASCII.GetBytes(new char[1] { name[i] })[0];
            }

            // Return the calculated hash.
            return Hash;
        }
    }
}
