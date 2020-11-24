using System;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Archive
{
    public class VirtNode
    {
        public Guid Guid { get; set; }
        public Guid ParentGuid { get; set; }
        public string Name { get; set; }
        public NodeType Type { get; set; }

        public VirtNode(string name, NodeType type, Guid parentGuid)
        {
            Guid = Guid.NewGuid();
            Name = name;
            Type = type;
            ParentGuid = parentGuid;
        }
    }

    public class VirtDirectory : VirtNode
    {
        public List<VirtNode> Children { get; set; }

        public VirtDirectory(string path) : base(Path.GetFileName(path), NodeType.Directory, Guid.Empty)
        {
            Children = new List<VirtNode>();

            string[] files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            string[] directories = Directory.GetDirectories(path, "**", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                Children.Add(new VirtFile(file)
                {
                    ParentGuid = Guid
                });
            }

            foreach (string directory in directories)
            {
                Children.Add(new VirtDirectory(directory)
                {
                    ParentGuid = Guid
                });
            }
        }

        public VirtDirectory(string name, Guid parentGuid) : base(Path.GetFileName(name), NodeType.Directory, parentGuid)
        {
            Children = new List<VirtNode>();
        }

        public VirtDirectory(string name, Guid parentGuid, List<VirtNode> children) : base(Path.GetFileName(name), NodeType.Directory, parentGuid)
        {
            Children = children;
        }

        public VirtNode GetNodeByGuid(Guid guid, bool recursive = false)
        {
            foreach (VirtNode node in GetNodes(recursive))
            {
                if (node.Guid == guid)
                {
                    return node;
                }
            }
            return null;
        }

        public VirtFile GetFileByPath(string path, bool recursive = true)
        {
            // TODO - Complete this!
            string[] names = path.Split('\\');

            if (names.Length < 1)
            {
                foreach (VirtFile file in GetFiles())
                {

                }
                return null;
            }

            foreach (VirtFile file in GetNodes(recursive))
            {
                if ((file.Name + file.Extension) == path)
                {
                    return file;
                }
                else if (file.Name == path)
                {
                    return file;
                }
            }
            return null;
        }

        public VirtFile GetFileByName(string name, bool recursive = true)
        {
            foreach (VirtFile file in GetFiles(recursive))
            {
                if ((file.Name + file.Extension) == name)
                {
                    return file;
                }
                else if (file.Name == name)
                {
                    return file;
                }
            }
            return null;
        }

        public VirtDirectory GetDirectoryByName(string name, bool recursive = true)
        {
            foreach (VirtDirectory directory in GetDirectories(recursive))
            {
                if (directory.Name == name)
                {
                    return directory;
                }
            }
            return null;
        }

        public List<VirtFile> GetFilesByExtension(string extension, bool recursive = true)
        {
            List<VirtFile> files = new List<VirtFile>();

            if (extension.Length == 0)
            {
                return files;
            }

            if (!extension.StartsWith("."))
            {
                extension = extension.Insert(0, ".");
            }

            foreach (VirtFile file in GetFiles(recursive))
            {
                if (file.Extension == extension)
                {
                    files.Add(file);
                }
            }
            return files;
        }

        public void RemoveFileByName(string name, bool recursive = false)
        {
            var result = GetFileByName(name, recursive);
            if (result != null)
            {
                Children.Remove(result);
            }
            else
            {
                throw new InvalidOperationException($"File '{name}' does not exist!");
            }
        }

        public void RemoveDirectoryByName(string name, bool recursive = false)
        {
            var result = GetDirectoryByName(name, recursive);
            if (result != null)
            {
                Children.Remove(result);
            }
            else
            {
                throw new InvalidOperationException($"Directory '{name}' does not exist!");
            }
        }

        public List<VirtNode> GetNodes(bool recursive = false)
        {
            List<VirtNode> nodes = new List<VirtNode>();
            foreach (VirtNode node in Children)
            {
                nodes.Add(node);
                if (node.Type == NodeType.Directory)
                {
                    if (recursive)
                    {
                        nodes.AddRange(((VirtDirectory)node).GetNodes(recursive));
                    }
                }
            }
            return nodes;
        }

        public List<VirtFile> GetFiles(bool recursive = false)
        {
            List<VirtFile> files = new List<VirtFile>();
            foreach (VirtNode node in GetNodes(recursive))
            {
                if (node.Type == NodeType.File)
                {
                    files.Add((VirtFile)node);
                }
            }
            return files;
        }

        public List<VirtDirectory> GetDirectories(bool recursive = false)
        {
            List<VirtDirectory> directories = new List<VirtDirectory>();
            foreach (VirtNode node in GetNodes(recursive))
            {
                if (node.Type == NodeType.Directory)
                {
                    directories.Add((VirtDirectory)node);
                }
            }
            return directories;
        }

        public void Export(string path)
        {
            string directoryPath = $"{path}\\{Name}";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (VirtNode node in Children)
            {
                switch (node.Type)
                {
                    case NodeType.File:
                        ((VirtFile)node).Export(directoryPath);
                        break;
                    case NodeType.Directory:
                        ((VirtDirectory)node).Export(directoryPath);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }

    public class VirtFile : VirtNode
    {
        public string Extension { get; set; }
        public byte[] Data { get; set; }

        public VirtFile(string path) : base(Path.GetFileNameWithoutExtension(path), NodeType.File, Guid.Empty)
        {
            Extension = Path.GetExtension(path);
            Data = File.ReadAllBytes(path);
        }

        public VirtFile(string name, Guid parentGuid, byte[] data) : base(Path.GetFileNameWithoutExtension(name), NodeType.File, parentGuid)
        {
            Extension = Path.GetExtension(name);
            Data = data;
        }

        public void Export(string path)
        {
            string filePath = $"{path}\\{Name}{Extension}";
            byte[] reverse = Data;
            Array.Reverse(reverse);
            File.WriteAllBytes(filePath, reverse);
        }
    }

    public static class VirtUtils
    {
        public static VirtFile GetFileByPath(VirtDirectory directory, string path)
        {
            // TODO - Add funtionality..
            return new VirtFile("test", Guid.Empty, new byte[2]);
        }

        public static string GetFullNodePath(VirtDirectory directory, VirtNode node)
        {
            string result = "";
            // TODO - Add funtionality..
            return result;
        }
    }

    public enum NodeType
    {
        None,
        Directory = 512,
        File = 4352
    }
}
