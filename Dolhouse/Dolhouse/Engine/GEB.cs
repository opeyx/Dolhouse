using Dolhouse.Binary;
using OpenTK;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (G)host (E)ntity (B)eacon
    /// TODO: Further reasearch.
    /// </summary>
    public class GEB
    {

        #region Properties

        /// <summary>
        /// List of 'sprites' stored in GEB.
        /// </summary>
        List<GSprite> Sprites { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty GEB.
        /// </summary>
        public GEB()
        {

            // Define a new list to hold the sprite entries.
            Sprites = new List<GSprite>();
        }

        /// <summary>
        /// Reads GEB from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the GEB data.</param>
        public GEB(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read GEB's sprite count.
            uint spriteCount = br.ReadU32();

            // Define a new list to hold the sprite entries.
            Sprites = new List<GSprite>();

            // Loop through the sprite entries.
            for (int i = 0; i < spriteCount; i++)
            {
                // Add the read sprite to the Sprites list.
                Sprites.Add(new GSprite(br));
            }
        }

        /// <summary>
        /// Creates a stream from this GEB.
        /// </summary>
        /// <returns>The GEB as a stream.</returns>
        public Stream Write()
        {
            // Define a stream to hold our GEB data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write GEB's sprite count.
            bw.WriteU32((uint)Sprites.Count);

            // Loop through the sprite entries.
            for (int i = 0; i < Sprites.Count; i++)
            {
                // Write sprite.
                Sprites[i].Write(bw);
            }

            // Returns the GEB as a stream.
            return stream;
        }
    }


    /// <summary>
    /// GEB Sprite
    /// </summary>
    public class GSprite
    {

        #region Properties

        /// <summary>
        /// Unknown 1. Some kind of type?
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2. Some kind of type?
        /// </summary>
        public short Unknown2 { get; set; }

        /// <summary>
        /// Sprite color in RGBA.
        /// </summary>
        public int RGBA { get; set; }

        /// <summary>
        /// A list of 2D points specifying the size of the sprite plane.
        /// </summary>
        public List<SpritePoint> Points { get; set; }

        /// <summary>
        /// Unknown 3. Some of them seem to be padding,
        /// while some seem to be floats with a value of 2.
        /// </summary>
        public int[] Unknown3 { get; set; }

        /// <summary>
        /// Unknown 4. Seems to be a normalized value (?)
        /// </summary>
        public float Unknown4 { get; set; }

        /// <summary>
        /// Unknown 5. Seems to be a normalized value (?)
        /// </summary>
        public float Unknown5 { get; set; }

        /// <summary>
        /// Unknown 6. Maybe a RGBA value (?)
        /// </summary>
        public int Unknown6 { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty GSprite.
        /// </summary>
        public GSprite()
        {

            // Set GSprite's Unknown 1.
            Unknown1 = 0;

            // Set GSprite's Unknown 2.
            Unknown2 = 0;

            // Set GSprite's RGBA.
            RGBA = 0;

            // Define a new list to hold the GSprite's points.
            Points = new List<SpritePoint>();

            // Define a array to hold the unknown values.
            Unknown3 = new int[10];

            // Set GSprite's Unknown 4.
            Unknown4 = 0;

            // Set GSprite's Unknown 5.
            Unknown5 = 0;

            // Set GSprite's Unknown 6.
            Unknown6 = 0;
        }

        /// <summary>
        /// Read a GSprite from GEB.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public GSprite(DhBinaryReader br)
        {

            // Read GSprite's Unknown 1.
            Unknown1 = br.ReadS16();

            // Read GSprite's Unknown 2.
            Unknown2 = br.ReadS16();

            // Read GSprite's RGBA.
            RGBA = br.ReadS32();

            // Define a new list to hold the GSprite's points.
            Points = new List<SpritePoint>();

            // Loop through GSprite's points.
            for (int i = 0; i < 4; i++)
            {
                // Read point and add it to the spritepoint list.
                Points.Add(new SpritePoint(br));
            }

            // Define a array to hold the unknown values.
            Unknown3 = new int[10];

            // Loop through the unknown values.
            for (int i = 0; i < Unknown3.Length; i++)
            {
                // Read the current unknown value.
                Unknown3[i] = br.ReadS32();
            }

            // Read GSprite's Unknown 4.
            Unknown4 = br.ReadF32();

            // Read GSprite's Unknown 5.
            Unknown5 = br.ReadF32();

            // Read GSprite's Unknown 6.
            Unknown6 = br.ReadS32();
        }

        /// <summary>
        /// Write a GSprite with specified Binary Writer.
        /// </summary>
        public void Write(DhBinaryWriter bw)
        {

            // Write GSprite's Unknown 1.
            bw.WriteS16(Unknown1);

            // Write GSprite's Unknown 2.
            bw.WriteS16(Unknown2);

            // Write GSprite's RGBA.
            bw.WriteS32(RGBA);

            // Loop through GSprite's points.
            for (int i = 0; i < Points.Count; i++)
            {
                // Write the current point.
                Points[i].Write(bw);
            }

            // Loop through the GSprite's Unknown 3 values.
            for (int i = 0; i < Unknown3.Length; i++)
            {
                // Write the current GSprite's Unknown 3 value.
                bw.WriteS32(Unknown3[i]);
            }

            // Write GSprite's Unknown 4.
            bw.WriteF32(Unknown4);

            // Write GSprite's Unknown 5.
            bw.WriteF32(Unknown5);

            // Write GSprite's Unknown 6.
            bw.WriteS32(Unknown6);
        }
    }


    /// <summary>
    /// Sprite Point
    /// </summary>
    public class SpritePoint
    {

        #region Properties

        /// <summary>
        /// Point's Position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Unknown1. Seems to just be padding.
        /// </summary>
        public int Unknown1 { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty SpritePoint.
        /// </summary>
        public SpritePoint()
        {

            // Set SpritePoint's Position.
            Position = new Vector2();

            // Set SpritePoint's Unknown 1.
            Unknown1 = 0;
        }

        /// <summary>
        /// Read a SpritePoint from GEB.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        public SpritePoint(DhBinaryReader br)
        {

            // Read SpritePoint's Position.
            Position = new Vector2(br.ReadF32(), br.ReadF32());

            // Read SpritePoint's Unknown 1.
            Unknown1 = br.ReadS32();
        }

        /// <summary>
        /// Write a SpritePoint with specified Binary Writer.
        /// </summary>
        public void Write(DhBinaryWriter bw)
        {

            // Write SpritePoint's X Position.
            bw.WriteF32(Position.X);

            // Write SpritePoint's Y Position.
            bw.WriteF32(Position.Y);

            // Read SpritePoint's Unknown 1.
            bw.WriteS32(Unknown1);
        }
    }
}
