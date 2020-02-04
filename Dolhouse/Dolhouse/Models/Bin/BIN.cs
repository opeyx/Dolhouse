using Dolhouse.Binary;
using Dolhouse.Image.BTI;
using Dolhouse.Models.GX;
using Dolhouse.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// (Bin)ary Model
    /// Code references, thanks!:
    /// arookas (https://github.com/arookas/Demolisher/)
    /// Sage-of-Mirrors (https://github.com/Sage-of-Mirrors/BooldozerCore/)
    /// </summary>
    public class BIN
    {

        #region Properties

        /// <summary>
        /// Bin version. Should read either 0x01 or 0x02.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Bin model name.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Bin offsets. There's always 21 offsets!
        /// </summary>
        public uint[] Offsets { get; set; }

        /// <summary>
        /// Bin textures.
        /// </summary>
        public List<BTI> Textures { get; set; }

        /// <summary>
        /// Bin materials.
        /// </summary>
        public List<Material> Materials { get; set; }

        /// <summary>
        /// Bin positions.
        /// </summary>
        public List<Vec3> Positions { get; set; }

        /// <summary>
        /// Bin normals.
        /// </summary>
        public List<Vec3> Normals { get; set; }

        /// <summary>
        /// Bin texture coordinates 0.
        /// </summary>
        public List<Vec2> TextureCoordinates0 { get; set; }

        /// <summary>
        /// Bin texture coordinates 1.
        /// </summary>
        public List<Vec2> TextureCoordinates1 { get; set; }

        /// <summary>
        /// Bin normals.
        /// </summary>
        public List<Shader> Shaders { get; set; }

        /// <summary>
        /// Bin batches.
        /// </summary>
        public List<Batch> Batches { get; set; }

        /// <summary>
        /// Bin graph objects.
        /// </summary>
        public List<GraphObject> GraphObjects { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty BIN.
        /// </summary>
        public BIN()
        {
            Version = 2;
            ModelName = "dolhouse-x";
            Offsets = new uint[21];
            Textures = new List<BTI>();
            Materials = new List<Material>();
            Positions = new List<Vec3>();
            Normals = new List<Vec3>();
            TextureCoordinates0 = new List<Vec2>();
            TextureCoordinates1 = new List<Vec2>();
            Shaders = new List<Shader>();
            Batches = new List<Batch>();
            GraphObjects = new List<GraphObject>();
        }

        /// <summary>
        /// Reads BIN from a stream.
        /// </summary>
        /// <param name="stream">The stream containing the BIN data.</param>
        public BIN(Stream stream)
        {
            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read bin version.
            Version = br.Read();

            // Make sure the bin version is either 0x01 or 0x02.
            if (Version == 0x00 || Version > 0x02 )
            { throw new Exception(string.Format("[BIN] {0} is not a valid version!", Version.ToString())); }

            // Read bin model name.
            ModelName = br.ReadFixedStr(11);

            // Define a new list to hold the bin's offsets.
            Offsets = br.ReadU32s(21);


            // Go to the bin graph object offset.
            br.Goto(Offsets[12]);

            // Get first graph object, then all of it's attached graph objects.
            GraphObjects = GetGraphObjects(br, 0);


            // Make sure bin batches has a offset.
            if (Offsets[11] != 0)
            {

                // Go to the bin batches offset.
                br.Goto(Offsets[11]);

                // Define a new list to hold the bin's batches.
                Batches = new List<Batch>();

                // Loop through batches.
                for (int i = 0; i < GetBatchCount(); i++)
                {

                    // Read a batch and add it to the batch list.
                    Batches.Add(new Batch(br, Offsets[11]));
                }
            }


            // Make sure bin shaders has a offset.
            if (Offsets[10] != 0)
            {

                // Go to the bin shaders offset.
                br.Goto(Offsets[10]);

                // Define a new list to hold the bin's shaders.
                Shaders = new List<Shader>();

                // Loop through shaders.
                for (int i = 0; i < GetShaderCount(); i++)
                {

                    // Read a shader and add it to the shader list.
                    Shaders.Add(new Shader(br));
                }
            }


            // Make sure bin texture coordinates 1 has a offset.
            if (Offsets[7] != 0)
            {

                // Go to the bin texture coordinates 1 offset.
                br.Goto(Offsets[7]);

                // Define a new list to hold the bin's texture coordinates 1.
                TextureCoordinates1 = new List<Vec2>();

                // Loop through texture coordinates 1.
                for (int i = 0; i < GetTexCoordinate1Count(); i++)
                {

                    // Read a bin texture coordinates and add it to the texture coordinates 1 list.
                    TextureCoordinates1.Add(br.ReadVec2());
                }
            }


            // Make sure bin texture coordinates 0 has a offset.
            if (Offsets[6] != 0)
            {

                // Go to the bin texture coordinates 0 offset.
                br.Goto(Offsets[6]);

                // Define a new list to hold the bin's texture coordinates 0.
                TextureCoordinates0 = new List<Vec2>();

                // Loop through texture coordinates 0.
                for (int i = 0; i < GetTexCoordinate0Count(); i++)
                {

                    // Read a bin texture coordinates 0 and add it to the texture coordinates 0 list.
                    TextureCoordinates0.Add(br.ReadVec2());
                }
            }


            // WE'RE SKIPPING COLOR0 AND COLOR1! (Offset5 and Offset4)


            // Make sure bin normals has a offset.
            if (Offsets[3] != 0)
            {

                // Go to the bin normals offset.
                br.Goto(Offsets[3]);

                // Define a new list to hold the bin's normals.
                Normals = new List<Vec3>();

                // Loop through normals.
                for (int i = 0; i < GetNormalCount(); i++)
                {

                    // Read a bin normal and add it to the normals list.
                    Normals.Add(br.ReadVec3());
                }
            }


            // Make sure bin positions has a offset.
            if (Offsets[2] != 0)
            {

                // Go to the bin positions offset.
                br.Goto(Offsets[2]);

                // Define a new list to hold the bin's positions.
                Positions = new List<Vec3>();

                // Loop through positions.
                for (int i = 0; i < GetPositionCount(); i++)
                {

                    // Read a bin position and add it to the positions list.
                    Positions.Add(new Vec3(br.ReadF16() / 256.0f, br.ReadF16() / 256.0f, br.ReadF16() / 256.0f));
                }
            }


            // Make sure bin materials has a offset.
            if (Offsets[1] != 0)
            {

                // Go to the bin materials offset.
                br.Goto(Offsets[1]);
                
                // Define a new list to hold the bin's materials.
                Materials = new List<Material>();

                // Loop through materials.
                for (int i = 0; i < GetMaterialCount(); i++)
                {

                    // Read a bin material and add it to the materials list.
                    Materials.Add(new Material(br));
                }
            }


            // Make sure bin textures has a offset.
            if(Offsets[0] != 0)
            {

                // Go to the bin textures offset.
                br.Goto(Offsets[0]);

                // Define a new list to hold the bin's textures.
                Textures = new List<BTI>();

                // Loop through textures.
                for (int i = 0; i < GetTextureCount(); i++)
                {

                    // Read a bin textures and add it to the textures list.
                    Textures.Add(new BTI(br, Offsets[0]));
                }
            }
        }

        /// <summary>
        /// Creates a stream from this BIN.
        /// </summary>
        /// <returns>The BIN as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our BIN data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Define a buffer to store our offsets.
            uint[] offsets = new uint[21];

            // Write version.
            bw.Write(Version);

            // Write model Name.
            bw.WriteFixedStr(ModelName, 11);

            // Write offsets.
            bw.WriteU32s(Offsets);

            // Make sure bin has textures.
            if (Textures.Count > 0)
            {

                // Set textures offset.
                offsets[0] = (uint)bw.Position();

                // Define a array to temporarily 
                uint[] textureDataOffsets = new uint[Textures.Count];

                // Write texture headers. (CALCULATED)
                bw.Write(new byte[Textures.Count * 0x0C]);

                // Pad to nearest whole 32.
                bw.WritePadding32();

                // Loop through textures to write texture data.
                for (int i = 0; i < textureDataOffsets.Length; i++)
                {

                    // Get actual offset of texture data.
                    textureDataOffsets[i] = (uint)bw.Position() - offsets[0];

                    // Write texture data.
                    bw.Write(Textures[i].Data);
                }

                // Store this so we can resume after writing the texture headers.
                long currentOffset = (uint)bw.Position();

                // Pad to nearest whole 32.
                bw.WritePadding32();

                // Goto textures offset.
                bw.Goto(offsets[0]);

                // Loop through textures to write texture headers.
                for (int i = 0; i < Textures.Count; i++)
                {

                    // Write texture width.
                    bw.WriteU16(Textures[i].Width);

                    // Write texture height.
                    bw.WriteU16(Textures[i].Height);

                    // Write texture format.
                    bw.Write((byte)Textures[i].Format);

                    // Write texture alpha flag.
                    bw.Write(Textures[i].AlphaFlag);

                    // Write padding.
                    bw.WriteU16(0);

                    // Write texture dataoffset.
                    bw.WriteU32(textureDataOffsets[i]);
                }

                // Goto resume point.
                bw.Goto(currentOffset);

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // Make sure bin has materials.
            if (Materials.Count > 0)
            {

                // Set materials offset.
                offsets[1] = (uint)bw.Position();

                // Loop through materials.
                for (int i = 0; i < Materials.Count; i++)
                {

                    // Write material.
                    Materials[i].Write(bw);
                }

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // Make sure bin has positions.
            if (Positions.Count > 0)
            {

                // Set positions offset.
                offsets[2] = (uint)bw.Position();

                // Loop through positions.
                for (int i = 0; i < Positions.Count; i++)
                {

                    // Write position.
                    bw.WriteS16s(new short[] { (short)(Positions[i].X * 256.0f), (short)(Positions[i].Y * 256.0f), (short)(Positions[i].Z * 256.0f) });
                }

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // Make sure bin has normals.
            if (Normals.Count > 0)
            {
                // Set normals offset.
                offsets[3] = (uint)bw.Position();

                // Loop through normals.
                for (int i = 0; i < Normals.Count; i++)
                {

                    // Write normal.
                    bw.WriteVec3(Normals[i]);
                }

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // SKIP COLOR0
            offsets[4] = (uint)0;

            // SKIP COLOR1
            offsets[5] = (uint)0;

            // Make sure bin has texture coordinates 0.
            if (TextureCoordinates0.Count > 0)
            {
                // Set texture coordinates 0 offset.
                offsets[6] = (uint)bw.Position();

                // Loop through texture coordinates 0.
                for (int i = 0; i < TextureCoordinates0.Count; i++)
                {

                    // Write texture coordinate 0.
                    bw.WriteVec2(TextureCoordinates0[i]);
                }

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // SKIP TEXTURE COORDINATES 1
            offsets[7] = (uint)0;

            // SKIP TEXTURE COORDINATES 2 (?)
            offsets[8] = (uint)0;

            // SKIP TEXTURE COORDINATES 3 (?)
            offsets[9] = (uint)0;

            // Make sure bin has shaders.
            if (Shaders.Count > 0)
            {
                // Set shaders offset.
                offsets[10] = (uint)bw.Position();

                // Loop through shaders.
                for (int i = 0; i < Shaders.Count; i++)
                {

                    // Write shader.
                    Shaders[i].Write(bw);
                }

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // Make sure bin has batches.
            if (Batches.Count > 0)
            {

                // Set batches offset.
                offsets[11] = (uint)bw.Position();

                // Loop through batches.
                for (int i = 0; i < Batches.Count; i++)
                {

                    // Write batch headers.
                    Batches[i].Write(bw);
                }

                // Pad to nearest whole 32.
                bw.WritePadding32();

                // We need to store this stuff somewhere
                long[] listStarts = new long[Batches.Count];
                long[] listEnds = new long[Batches.Count];

                // Loop through batches. (Write primitives)
                for (int i = 0; i < Batches.Count; i++)
                {

                    // We'll store this offset for later.
                    listStarts[i] = bw.Position();

                    // Loop through primitives.
                    for (int y = 0; y < Batches[i].Primitives.Count; y++)
                    {

                        // Write primitive.
                        Batches[i].Primitives[y].Write(bw, Batches[i].VertexAttributes);
                    }

                    // We'll store this offset for later.
                    listEnds[i] = bw.Position();
                }

                // This offset is where we'll continue writing from.
                long currentOffset = bw.Position();

                // Loop through batches. (Write offsets)
                for (int i = 0; i < Batches.Count; i++)
                {

                    // Goto current batch's offset.
                    bw.Goto(offsets[11] + i * 24);

                    // Skip 2 bytes.
                    bw.Sail(2);

                    // Write list size represented as 32 byte blocks.
                    bw.WriteS16((short)(Math.Ceiling((float)(listEnds[i] - listStarts[i]) / 32)));

                    // Skip 8 bytes.
                    bw.Sail(8);

                    // Write primitive list offset.
                    bw.WriteU32((uint)(listStarts[i] - offsets[11]));
                }

                // Goto continue point we saved earlier.
                bw.Goto(currentOffset);

                // Pad to nearest whole 32.
                bw.WritePadding32();
            }

            // Make sure bin has graphobjects.
            if (GraphObjects.Count > 0)
            {

                // Set graphObjects offset.
                offsets[12] = (uint)bw.Position();

                // Loop through graphObjects.
                for (int i = 0; i < GraphObjects.Count; i++)
                {

                    // Write graphObject headers.
                    GraphObjects[i].Write(bw);
                }

                // Pad to nearest whole 16.
                bw.WritePadding16();

                // Array to hold graphobject's parts offset.
                long[] graphObjectsPartsOffsets = new long[GraphObjects.Count];

                // Loop through graphObjects. (Write parts)
                for (int i = 0; i < GraphObjects.Count; i++)
                {

                    // Store this graphobject's part offset.
                    graphObjectsPartsOffsets[i] = bw.Position();

                    // Loop through graphobject's parts.
                    for (int y = 0; y < GraphObjects[i].Parts.Count; y++)
                    {

                        // Write graphobject's parts.
                        GraphObjects[i].Parts[y].Write(bw);
                    }
                }

                // This offset is where we'll continue writing from.
                long currentOffset = bw.Position();

                // Loop through graphObjects. (Write offsets)
                for (int i = 0; i < GraphObjects.Count; i++)
                {

                    // Goto current graphobject's offset.
                    bw.Goto(offsets[12] + (i * 140));

                    // Skip 80 bytes.
                    bw.Sail(80);

                    // Write graphobject's part offset.
                    bw.WriteU32((uint)(graphObjectsPartsOffsets[i] - offsets[12]));
                }

                // Goto continue point we saved earlier.
                bw.Goto(currentOffset);
            }

            // Goto offsets section.
            bw.Goto(12);

            // Write offsets.
            bw.WriteU32s(offsets);

            // Goto end of file.
            bw.Back(0);

            // Pad to nearest whole 32.
            bw.WritePadding32();

            // Return the bin as a stream.
            return stream;
        }

        public List<GraphObject> GetGraphObjects(DhBinaryReader br, int index)
        {

            // Save current offset.
            long currentPosition = br.Position();

            // Goto a specific graph object offset based on it's index.
            br.Goto(Offsets[12] + (0x8C * index));

            // Initialize the graph object at specific index.
            GraphObject graphObject = new GraphObject(br, Offsets[12]);

            // Goto previously saved offset.
            br.Goto(currentPosition);

            // Initialize a list to hold all graph objects.
            List<GraphObject> graphObjects = new List<GraphObject>();

            // Add first graph object to list of graph objects.
            graphObjects.Add(graphObject);

            // Check if graph object specifies a child graph object.
            if (graphObject.ChildIndex >= 0)
            {

                // Add the child graph object (And it's child/next graph objects.)
                graphObjects.AddRange(GetGraphObjects(br, graphObject.ChildIndex));
            }

            // Check if graph object specifies the next graph object.
            if (graphObject.NextIndex >= 0)
            {

                // Add the next graph object (And it's child/next graph objects.)
                graphObjects.AddRange(GetGraphObjects(br, graphObject.NextIndex));
            }

            // Return the list of graph objects.
            return graphObjects;
        }

        /// <summary>
        /// Calculates the amount of Textures in bin.
        /// </summary>
        /// <returns>Total amount of Textures in bin.</returns>
        public int GetTextureCount()
        {
            short textureCount = 0;

            for (int i = 0; i < Materials.Count; i++)
            {
                if (Materials[i].Index > textureCount)
                {
                    textureCount = Materials[i].Index;
                }
            }

            return (textureCount + 1);
        }

        /// <summary>
        /// Calculates the amount of Materials in bin.
        /// </summary>
        /// <returns>Total amount of Materials in bin.</returns>
        public int GetMaterialCount()
        {
            short materialCount = -1;

            for (int i = 0; i < Shaders.Count; i++)
            {
                for (int y = 0; y < Shaders[i].MaterialIndices.Length; y++)
                {
                    if (Shaders[i].MaterialIndices[y] > materialCount)
                    {
                        materialCount = Shaders[i].MaterialIndices[y];
                    }
                }
            }
            
            return (materialCount + 1);
        }

        /// <summary>
        /// Calculates the amount of Vertices in bin.
        /// </summary>
        /// <returns>Total amount of Vertices in bin.</returns>
        public uint GetPositionCount()
        {
            uint positionCount = 0;

            if (Offsets[2] != 0)
            {
                for (int i = 3; i < Offsets.Length; i++)
                {
                    if (Offsets[i] > 0)
                    {
                        positionCount = (Offsets[i] - Offsets[2]) / 6;
                        break;
                    }
                }
            }

            return positionCount;
        }

        /// <summary>
        /// Calculates the amount of Normals in bin.
        /// </summary>
        /// <returns>Total amount of Normals in bin.</returns>
        public uint GetNormalCount()
        {
            uint normalCount = 0;

            if(Offsets[3] != 0)
            {
                for(int i = 4; i < Offsets.Length; i++)
                {
                    if(Offsets[i] > 0)
                    {
                        normalCount = (Offsets[i] - Offsets[3]) / 12;
                        break;
                    }
                }
            }

            return normalCount;
        }

        /// <summary>
        /// Calculates the amount of TextureCoordinate0s in bin.
        /// </summary>
        /// <returns>Total amount of TextureCoordinate0s in bin.</returns>
        public uint GetTexCoordinate0Count()
        {
            uint texCoord0Count = 0;

            if (Offsets[6] != 0)
            {
                for (int i = 7; i < Offsets.Length; i++)
                {
                    if (Offsets[i] > 0)
                    {
                        texCoord0Count = (Offsets[i] - Offsets[6]) / 8;
                        break;
                    }
                }
            }

            return texCoord0Count;
        }

        /// <summary>
        /// Calculates the amount of TextureCoordinate1s in bin.
        /// </summary>
        /// <returns>Total amount of TextureCoordinate1s in bin.</returns>
        public uint GetTexCoordinate1Count()
        {
            uint texCoord1Count = 0;

            if (Offsets[7] != 0)
            {
                for (int i = 8; i < Offsets.Length; i++)
                {
                    if (Offsets[i] > 0)
                    {
                        texCoord1Count = (Offsets[i] - Offsets[7]) / 8;
                        break;
                    }
                }
            }

            return texCoord1Count;
        }

        /// <summary>
        /// Calculates the amount of Shaders in bin.
        /// </summary>
        /// <returns>Total amount of Shaders in bin.</returns>
        public int GetShaderCount()
        {
            short shaderCount = -1;

            for (int i = 0; i < GraphObjects.Count; i++)
            {
                for (int y = 0; y < GraphObjects[i].Parts.Count; y++)
                {
                    if (GraphObjects[i].Parts[y].ShaderIndex > shaderCount)
                    {
                        shaderCount = GraphObjects[i].Parts[y].ShaderIndex;
                    }
                }
            }

            return (shaderCount + 1);
        }

        /// <summary>
        /// Calculates the amount of Batches in bin.
        /// </summary>
        /// <returns>Total amount of Batches in bin.</returns>
        public int GetBatchCount()
        {
            short batchCount = -1;

            for (int i = 0; i < GraphObjects.Count; i++)
            {
                for (int y = 0; y < GraphObjects[i].Parts.Count; y++)
                {
                    if (GraphObjects[i].Parts[y].BatchIndex > batchCount)
                    {
                        batchCount = GraphObjects[i].Parts[y].BatchIndex;
                    }
                }
            }

            return (batchCount + 1);
        }
    }
}
