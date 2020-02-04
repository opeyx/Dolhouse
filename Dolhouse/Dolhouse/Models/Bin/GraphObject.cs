using Dolhouse.Binary;
using Dolhouse.Type;
using System.Collections.Generic;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// GraphObject.
    /// </summary>
    public class GraphObject
    {

        #region Properties

        /// <summary>
        /// Parent GraphObject's Index.
        /// (-1 == None)
        /// </summary>
        public short ParentIndex { get; set; }

        /// <summary>
        /// Child GraphObject's Index.
        /// (-1 == None)
        /// </summary>
        public short ChildIndex { get; set; }

        /// <summary>
        /// Next GraphObject's Index.
        /// (-1 == None)
        /// </summary>
        public short NextIndex { get; set; }

        /// <summary>
        /// Previous GraphObject's Index.
        /// (-1 == None)
        /// </summary>
        public short PreviousIndex { get; set; }

        /// <summary>
        /// Render Flags.
        /// </summary>
        public RenderFlags RenderFlags { get; set; }

        /// <summary>
        /// Unknown 1. (Padding?)
        /// </summary>
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Scale.
        /// </summary>
        public Vec3 Scale { get; set; }

        /// <summary>
        /// Rotation. (Euler Angle, Degrees)
        /// </summary>
        public Vec3 Rotation { get; set; }

        /// <summary>
        /// Position.
        /// </summary>
        public Vec3 Position { get; set; }

        /// <summary>
        /// Bounding Box Minimum.
        /// </summary>
        public Vec3 BoundingBoxMinimum { get; set; }

        /// <summary>
        /// Bounding Box Maximum.
        /// </summary>
        public Vec3 BoundingBoxMaximum { get; set; }

        /// <summary>
        /// Bounding Sphere Radius
        /// (Unsure, GO is similar to BMD's Shape Data)
        /// </summary>
        public float BoundingSphereRadius { get; set; }

        /// <summary>
        /// Amount of parts.
        /// </summary>
        public short PartCount { get; set; }

        /// <summary>
        /// Unknown 3. (Padding?)
        /// </summary>
        public ushort Unknown3 { get; set; }

        /// <summary>
        /// Part Offset (Relative to GOs Offset)
        /// </summary>
        public int PartOffset { get; set; }

        /// <summary>
        /// Unknown 4. (Padding?)
        /// </summary>
        public uint[] Unknown4 { get; set; }

        /// <summary>
        /// List of parts.
        /// </summary>
        public List<BinGraphObjectPart> Parts { get; set;}

        #endregion


        /// <summary>
        /// Read a single graph object from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="graphObjectOffset">Offset to the graph objects.</param>
        public GraphObject(DhBinaryReader br, uint graphObjectOffset) // 112 bytes long
        {

            // Read Parent Index.
            ParentIndex = br.ReadS16();

            // Read Child Index.
            ChildIndex = br.ReadS16();

            // Read Next Index.
            NextIndex = br.ReadS16();

            // Read Previous Index.
            PreviousIndex = br.ReadS16();

            // Read Render Flags.
            RenderFlags = (RenderFlags)br.ReadU16();

            // Unknown 1. (Padding?)
            Unknown1 = br.ReadU16();

            // Read Scale.
            Scale = br.ReadVec3();

            // Read Rotation.
            Rotation = br.ReadVec3();

            // Read Position
            Position = br.ReadVec3();

            // Read Bounding Box Minimum.
            BoundingBoxMinimum = br.ReadVec3();

            // Read Bounding Box Maximum.
            BoundingBoxMaximum = br.ReadVec3();

            // Read Bounding Sphere Radius.
            BoundingSphereRadius = br.ReadF32();

            // Read Parent Index.
            PartCount = br.ReadS16();

            // Unknown 3. (Padding?)
            Unknown3 = br.ReadU16();

            // Read Parent Index.
            PartOffset = br.ReadS32();

            // Unknown 4. (Padding?)
            Unknown4 = br.ReadU32s(7);

            // Initialize a new list to hold parts.
            Parts = new List<BinGraphObjectPart>();

            // Offset to continue reading from.
            long currentPosition = br.Position();

            // Goto graphobject's parts.
            br.Goto(graphObjectOffset + PartOffset);

            // Loop through parts.
            for(int i = 0; i < PartCount; i++)
            {
                // Read part and add it to the list of parts.
                Parts.Add(new BinGraphObjectPart(br));
            }

            // Go back to return offset we saved earlier.
            br.Goto(currentPosition);
        }

        /// <summary>
        /// Write a single graph object with BinaryWriter.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        /// <param name="graphObjectsOffset">Offset to the graph objects.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write parent graphobject index.
            bw.WriteS16(ParentIndex);

            // Write child graphobject index.
            bw.WriteS16(ChildIndex);

            // Write next graphobject index.
            bw.WriteS16(NextIndex);

            // Write previous graphobject index.
            bw.WriteS16(PreviousIndex);

            // Write renderflags.
            bw.WriteU16((ushort)RenderFlags);

            // Write unknown 1.
            bw.WriteU16(Unknown1);

            // Write scale.
            bw.WriteVec3(Scale);

            // Write rotation.
            bw.WriteVec3(Rotation);

            // Write position.
            bw.WriteVec3(Position);

            // Write bounding box minimum.
            bw.WriteVec3(BoundingBoxMinimum);

            // Write bounding box maximum.
            bw.WriteVec3(BoundingBoxMaximum);

            // Write bounding sphere radius.
            bw.WriteF32(BoundingSphereRadius);

            // Write part count.
            bw.WriteS16((short)Parts.Count);

            // Write unknown 3.
            bw.WriteU16(Unknown3);

            // Write part offset. (CALCULATED)
            bw.WriteU32(0);

            // Write unknown 4.
            bw.WriteU32s(Unknown4);

            // Write unknown 4. TODO - Why does demolisher only read file if padding is written twice??
            bw.WriteU32s(Unknown4);
        }
    }

    /// <summary>
    /// GraphObject Part.
    /// </summary>
    public class BinGraphObjectPart
    {

        /// <summary>
        /// Shader Index.
        /// </summary>
        public short ShaderIndex { get; set; }

        /// <summary>
        /// Batch Index.
        /// </summary>
        public short BatchIndex { get; set; }

        public BinGraphObjectPart(DhBinaryReader br)
        {

            // Read shader index.
            ShaderIndex = br.ReadS16();

            // Read batch index.
            BatchIndex = br.ReadS16();
        }

        public void Write(DhBinaryWriter bw)
        {

            // Write shader index.
            bw.WriteS16(ShaderIndex);


            // Write batch index.
            bw.WriteS16(BatchIndex);
        }
    }
}
