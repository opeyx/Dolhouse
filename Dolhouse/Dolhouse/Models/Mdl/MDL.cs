using Dolhouse.Binary;
using Dolhouse.Image.BTI;
using Dolhouse.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Models.Mdl
{
    // Many thanks to KillzXGaming for his research / code reference:
    // https://github.com/KillzXGaming/LM_Research/wiki/MDL
    // https://github.com/KillzXGaming/Toolbox.Reborn/tree/master/Plugins/GCNLibrary/LM/MDL
    public class MDL
    {
        public MDLHeader Header { get; set; }
        public List<Vec3> Positions { get; set; }
        public List<Vec3> Normals { get; set; }
        public List<Clr4> Colors { get; set; }
        public List<Vec2> TextureCoordinates { get; set; }
        public List<MDLNode> Nodes { get; set; }
        public List<MDLDrawElement> DrawElements { get; set; }
        public List<MDLShape> Shapes { get; set; }
        public List<MDLShapePacket> Packets { get; set; }
        public List<BTI> Textures { get; set; }
        public List<MDLSampler> Samplers { get; set; }
        public List<MDLMaterial> Materials { get; set; }
        public Mat4[] Matrices { get; set; }

        public MDL(byte[] data)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(data, DhEndian.Big);

            Header = new MDLHeader(br);

            br.Goto(Header.MatricesOffset);
            Matrices = new Mat4[Header.JointCount + Header.WeightCount];
            for (int i = 0; i < Matrices.Length; i++)
                Matrices[i] = Mat4.Identity;

            for (int i = 0; i < Header.JointCount; i++)
            {
                float[] matrix = br.ReadF32s(12);

                Matrices[i] = new Mat4()
                {
                    Row1 = new Vec4(matrix[0], matrix[1], matrix[2], matrix[3]),
                    Row2 = new Vec4(matrix[4], matrix[5], matrix[6], matrix[7]),
                    Row3 = new Vec4(matrix[8], matrix[9], matrix[10], matrix[11]),
                    Row4 = new Vec4(0, 0, 0, 1),
                };
            }

            br.Goto(Header.PositionOffset);
            Positions = br.ReadVec3s(Header.PositionCount).ToList();

            br.Goto(Header.NormalOffset);
            Normals = br.ReadVec3s(Header.NormalCount).ToList();

            br.Goto(Header.ColorOffset);
            Colors = br.ReadClr4s(Header.ColorCount).ToList();

            br.Goto(Header.TextureCoordinateOffset);
            TextureCoordinates = br.ReadVec2s(Header.TextureCoordinateCount).ToList();

            br.Goto(Header.PacketOffset);
            Packets = new List<MDLShapePacket>();
            for (int i = 0; i < Header.PacketCount; i++)
            {
                Packets.Add(new MDLShapePacket(br));
            }

            br.Goto(Header.ShapeOffset);
            Shapes = new List<MDLShape>();
            for (int i = 0; i < Header.ShapeCount; i++)
            {
                Shapes.Add(new MDLShape(br, Packets));
            }

            foreach (MDLShape shape in Shapes)
            {
                ushort[] matrixIndices = new ushort[10];

                foreach (MDLShapePacket packet in shape.Packets)
                {
                    for (int m = 0; m < packet.MatrixCount; m++)
                    {
                        if (packet.MatrixIndices[m] == ushort.MaxValue) continue;
                        matrixIndices[m] = packet.MatrixIndices[m];
                    }

                    DhBinaryReader reader = new DhBinaryReader(packet.Data, DhEndian.Big);

                    while (reader.Position() < packet.DataSize)
                    {
                        ShapePacketData packetData = new ShapePacketData();
                        packetData.PrimitiveType = reader.ReadU8();

                        if (packetData.PrimitiveType == 0x00)
                            continue;

                        packetData.VertexCount = reader.ReadU16();
                        for (int i = 0; i < packetData.VertexCount; i++)
                        {
                            MDLVertex vertex = new MDLVertex(reader, Header, shape.UseNbt);
                            if(vertex.MatrixIndex != -1)
                            {
                                vertex.MatrixDataIndex = matrixIndices[(vertex.MatrixIndex / 3)];
                            }
                            packetData.Vertices.Add(vertex);
                        }
                        packet.PacketData.Add(packetData);
                    }
                }
            }

            br.Goto(Header.TextureLocationOffset);
            Textures = new List<BTI>();
            for (int i = 0; i < Header.TextureCount; i++)
            {
                uint offset = br.ReadU32();
                long currentPosition = br.Position();
                br.Goto(offset);
                BTI bti = new BTI(br, offset, Header.TextureLocationOffset);
                Textures.Add(bti);
                br.Goto(currentPosition);
            }

            br.Goto(Header.SamplerOffset);
            Samplers = new List<MDLSampler>();
            for (int i = 0; i < Header.SamplerCount; i++)
            {
                Samplers.Add(new MDLSampler(br));
            }

            br.Goto(Header.MaterialOffset);
            Materials = new List<MDLMaterial>();
            for (int i = 0; i < Header.MaterialCount; i++)
            {
                Materials.Add(new MDLMaterial(br, Samplers));
            }

            br.Goto(Header.DrawElementOffset);
            DrawElements = new List<MDLDrawElement>();
            for (int i = 0; i < Header.DrawElementCount; i++)
            {
                DrawElements.Add(new MDLDrawElement(br, Materials, Shapes));
            }

            br.Goto(Header.NodeOffset);
            Nodes = new List<MDLNode>();
            for (int i = 0; i < Header.NodeCount; i++)
            {
                Nodes.Add(new MDLNode(br, DrawElements));
            }
        }
    }

    public class MDLMaterial
    {
        public Clr4 DiffuseColor { get; set; }
        public short Unknown1 { get; set; }
        public byte AlphaFlag { get; set; }
        public byte TevStageCount { get; set; }
        public byte Unknown2 { get; set; }
        public List<MDLTevStage> TevStages { get; set; }

        public MDLMaterial(DhBinaryReader br, List<MDLSampler> samplers)
        {
            DiffuseColor = br.ReadClr4();
            Unknown1 = br.ReadS16();
            AlphaFlag = br.ReadU8();
            TevStageCount = br.ReadU8();
            Unknown2 = br.ReadU8();
            br.Skip(23);
            TevStages = new List<MDLTevStage>();
            for (int i = 0; i < 8; i++)
            {
                TevStages.Add(new MDLTevStage(br));
            }
        }
    }

    public class MDLTevStage
    {
        public short Unknown1 { get; set; }
        public short SamplerIndex { get; set; }
        public float[] Unknown2 { get; set; }

        public MDLTevStage(DhBinaryReader br)
        {
            Unknown1 = br.ReadS16();
            SamplerIndex = br.ReadS16();
            Unknown2 = br.ReadF32s(7);
        }
    }

    public class MDLSampler
    {
        public ushort TextureIndex { get; set; }
        public ushort UnknownIndex { get; set; }
        public WrapMode WrapU { get; set; }
        public WrapMode WrapV { get; set; }
        public byte MinFilter { get; set; }
        public byte MagFilter { get; set; }

        public MDLSampler(DhBinaryReader br)
        {
            TextureIndex = br.ReadU16();
            UnknownIndex = br.ReadU16();
            WrapU = (WrapMode)br.ReadU8();
            WrapV = (WrapMode)br.ReadU8();
            MinFilter = br.ReadU8();
            MagFilter = br.ReadU8();
        }
    }

    public class MDLNode
    {
        public ushort MatrixIndex { get; set; }
        public ushort ChildIndex { get; set; } // relative to this node's index
        public ushort SiblingIndex { get; set; } // relative to this node's index
        public short Padding1 { get; set; }
        public ushort DrawElementCount { get; set; }
        public ushort DrawElementBeginIndex { get; set; }
        public int Padding2 { get; set; }
        public List<MDLDrawElement> DrawElements { get; set; }

        public MDLNode(DhBinaryReader br, List<MDLDrawElement> drawElements)
        {
            MatrixIndex = br.ReadU16();
            ChildIndex = br.ReadU16();
            SiblingIndex = br.ReadU16();
            Padding1 = br.ReadS16();
            DrawElementCount = br.ReadU16();
            DrawElementBeginIndex = br.ReadU16();
            Padding2 = br.ReadS32();
            DrawElements = drawElements.GetRange(DrawElementBeginIndex, DrawElementCount);
        }
    }

    public class MDLDrawElement
    {
        public short MaterialIndex { get; set; }
        public short ShapeIndex { get; set; }
        public MDLMaterial Material { get; set; }
        public MDLShape Shape { get; set; }

        public MDLDrawElement(DhBinaryReader br, List<MDLMaterial> materials, List<MDLShape> shapes)
        {
            MaterialIndex = br.ReadS16();
            ShapeIndex = br.ReadS16();

            Material = materials[MaterialIndex];
            Shape = shapes[ShapeIndex];
        }
    }

    public class MDLShape
    {
        public byte NormalFlag { get; set; }
        public byte Unknown1 { get; set; }
        public byte SurfaceFlag { get; set; }
        public byte Unknown2 { get; set; }
        public ushort PacketCount { get; set; }
        public ushort PacketBeginIndex { get; set; }
        public List<MDLShapePacket> Packets { get; set; }
        public bool UseNbt { get; set; }

        public MDLShape(DhBinaryReader br, List<MDLShapePacket> packets)
        {
            NormalFlag = br.ReadU8();
            Unknown1 = br.ReadU8();
            SurfaceFlag = br.ReadU8();
            Unknown2 = br.ReadU8();
            PacketCount = br.ReadU16();
            PacketBeginIndex = br.ReadU16();
            Packets = packets.GetRange(PacketBeginIndex, PacketCount);
            UseNbt = (NormalFlag > 0);
        }
    }

    public class MDLShapePacket
    {
        public uint DataOffset { get; set; }
        public uint DataSize { get; set; }
        public short Unknown { get; set; }
        public ushort MatrixCount { get; set; }
        public ushort[] MatrixIndices { get; set; }
        public byte[] Data { get; set; }
        public List<ShapePacketData> PacketData;

        public MDLShapePacket(DhBinaryReader br)
        {
            DataOffset = br.ReadU32();
            DataSize = br.ReadU32();
            Unknown = br.ReadS16();
            MatrixCount = br.ReadU16();
            MatrixIndices = br.ReadU16s(10);
            PacketData = new List<ShapePacketData>();
            Data = br.ReadAt(DataOffset, (int)DataSize);
        }
    }

    public class ShapePacketData
    {
        public byte PrimitiveType { get; set; }
        public ushort VertexCount { get; set; }
        public List<MDLVertex> Vertices;

        public ShapePacketData()
        {
            Vertices = new List<MDLVertex>();
        }
    }

    public class MDLVertex
    {
        public ushort MatrixDataIndex { get; set; }
        public sbyte MatrixIndex { get; set; }
        public sbyte Tex0MatrixIndex { get; set; }
        public sbyte Tex1MatrixIndex { get; set; }
        public ushort PositionIndex { get; set; }
        public ushort? NormalIndex { get; set; }
        public ushort? TangentIndex { get; set; }
        public ushort? BiTangentIndex { get; set; }
        public ushort? ColorIndex { get; set; }
        public ushort? TexCoordIndex { get; set; }

        public MDLVertex(DhBinaryReader br, MDLHeader header, bool useNbt)
        {
            MatrixIndex = br.ReadS8();
            Tex0MatrixIndex = br.ReadS8();
            Tex1MatrixIndex = br.ReadS8();
            PositionIndex = br.ReadU16();
            if (header.NormalCount > 0)
            {
                NormalIndex = br.ReadU16();
            }
            if (useNbt)
            {
                //TangentIndex = br.ReadU16();
                //BiTangentIndex = br.ReadU16();
            }
            if (header.ColorCount > 0)
            {
                ColorIndex = br.ReadU16();
            }
            if (header.TextureCoordinateCount > 0)
            {
                TexCoordIndex = br.ReadU16();
            }
        }

        public static int GetLength(MDLHeader header, bool useNbt)
        {
            int length = 5;

            if(header.NormalCount > 0)
            {
                length += 2;
            }
            if (useNbt)
            {
                //length += 4;
            }
            if (header.ColorCount > 0)
            {
                length += 2;
            }
            if (header.TextureCoordinateCount > 0)
            {
                length += 2;
            }

            return length;
        }
    }

    public class MDLHeader
    {
        public uint Magic { get; set; }
        public ushort FaceCount { get; set; }
        public short Padding { get; set; }
        public ushort NodeCount { get; set; }
        public ushort PacketCount { get; set; }
        public ushort WeightCount { get; set; }
        public ushort JointCount { get; set; }
        public ushort PositionCount { get; set; }
        public ushort NormalCount { get; set; }
        public ushort ColorCount { get; set; }
        public ushort TextureCoordinateCount { get; set; }
        public long Padding2 { get; set; }
        public ushort TextureCount { get; set; }
        public short Padding3 { get; set; }
        public ushort SamplerCount { get; set; }
        public ushort DrawElementCount { get; set; }
        public ushort MaterialCount { get; set; }
        public ushort ShapeCount { get; set; }
        public int Padding4 { get; set; }
        public uint NodeOffset { get; set; }
        public uint PacketOffset { get; set; }
        public uint MatricesOffset { get; set; }
        public uint WeightOffset { get; set; }
        public uint JointOffset { get; set; }
        public uint WeightCountTableOffset { get; set; }
        public uint PositionOffset { get; set; }
        public uint NormalOffset { get; set; }
        public uint ColorOffset { get; set; }
        public uint TextureCoordinateOffset { get; set; }
        public long Padding5 { get; set; }
        public uint TextureLocationOffset { get; set; }
        public int Padding6 { get; set; }
        public uint MaterialOffset { get; set; }
        public uint SamplerOffset { get; set; }
        public uint ShapeOffset { get; set; }
        public uint DrawElementOffset { get; set; }
        public long Padding7 { get; set; }

        public MDLHeader(DhBinaryReader br)
        {

            // magic
            Magic = br.ReadU32();

            // counts
            FaceCount = br.ReadU16();
            Padding = br.ReadS16();
            NodeCount = br.ReadU16();
            PacketCount = br.ReadU16();
            WeightCount = br.ReadU16();
            JointCount = br.ReadU16();
            PositionCount = br.ReadU16();
            NormalCount = br.ReadU16();
            ColorCount = br.ReadU16();
            TextureCoordinateCount = br.ReadU16();
            Padding2 = br.ReadS64();
            TextureCount = br.ReadU16();
            Padding3 = br.ReadS16();
            SamplerCount = br.ReadU16();
            DrawElementCount = br.ReadU16();
            MaterialCount = br.ReadU16();
            ShapeCount = br.ReadU16();
            Padding4 = br.ReadS32();

            // offsets
            NodeOffset = br.ReadU32();
            PacketOffset = br.ReadU32();
            MatricesOffset = br.ReadU32();
            WeightOffset = br.ReadU32();
            JointOffset = br.ReadU32();
            WeightCountTableOffset = br.ReadU32();
            PositionOffset = br.ReadU32();
            NormalOffset = br.ReadU32();
            ColorOffset = br.ReadU32();
            TextureCoordinateOffset = br.ReadU32();
            Padding5 = br.ReadS64();
            TextureLocationOffset = br.ReadU32();
            Padding6 = br.ReadS32();
            MaterialOffset = br.ReadU32();
            SamplerOffset = br.ReadU32();
            ShapeOffset = br.ReadU32();
            DrawElementOffset = br.ReadU32();
            Padding7 = br.ReadS64();
        }
    }
}
