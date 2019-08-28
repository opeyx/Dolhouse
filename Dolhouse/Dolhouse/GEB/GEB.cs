using Dolhouse.Binary;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.GEB
{

    /// <summary>
    /// (G)host (E)ntity (B)eacon
    /// TODO: Further reasearch + Document properly.
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
        /// Reads GEB from a data stream.
        /// </summary>
        /// <param name="stream">The stream containing the GEB data.</param>
        public GEB(Stream stream)
        {
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read GEB's 'sprite' count.
            uint spriteCount = br.ReadU32();

            // Read each 'sprite' entry.
            Sprites = new List<GSprite>();
            for(int i = 0; i < spriteCount; i++)
            {
                // Add the read 'sprite' to the 'Sprites' list.
                Sprites.Add(new GSprite(br));
            }
        }

        /// <summary>
        /// Creates a stream from this GEB.
        /// </summary>
        /// <returns>The GEB as a stream.</returns>
        public Stream Write()
        {
            Stream stream = new MemoryStream();

            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write 'sprite' count.
            bw.WriteU32((uint)Sprites.Count);

            // Loop through 'sprite' entries.
            for (int i = 0; i < Sprites.Count; i++)
            {
                // Write 'sprite'.
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
        public short Unknown1 { get; set; }
        public short Unknown2 { get; set; }
        public int RGBA { get; set; }
        public List<SpritePoint> Points { get; set; }
        public int[] Unknown3 { get; set; }
        public float Unknown4 { get; set; }
        public float Unknown5 { get; set; }
        public int Unknown6 { get; set; }

        public GSprite(DhBinaryReader br)
        {
            Unknown1 = br.ReadS16();
            Unknown2 = br.ReadS16();
            RGBA = br.ReadS32();

            Points = new List<SpritePoint>();
            for (int i = 0; i < 4; i++)
            {
                Points.Add(new SpritePoint(br));
            }

            Unknown3 = new int[10];
            for (int i = 0; i < Unknown3.Length; i++)
            {
                Unknown3[i] = br.ReadS32();
            }

            Unknown4 = br.ReadF32();
            Unknown5 = br.ReadF32();
            Unknown6 = br.ReadS32();
        }

        public void Write(DhBinaryWriter bw)
        {
            bw.WriteS16(Unknown1);
            bw.WriteS16(Unknown2);
            bw.WriteS32(RGBA);

            for(int i = 0; i < Points.Count; i++)
            {
                Points[i].Write(bw);
            }

            for (int i = 0; i < Unknown3.Length; i++)
            {
                bw.WriteS32(Unknown3[3]);
            }

            bw.WriteF32(Unknown4);
            bw.WriteF32(Unknown5);
            bw.WriteS32(Unknown6);
        }
    }


    /// <summary>
    /// Sprite Point
    /// </summary>
    public class SpritePoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Unknown1 { get; set; }

        public SpritePoint(DhBinaryReader br)
        {
            X = br.ReadF32();
            Y = br.ReadF32();
            Unknown1 = br.ReadF32();
        }

        public void Write(DhBinaryWriter bw)
        {
            bw.WriteF32(X);
            bw.WriteF32(Y);
            bw.WriteF32(Unknown1);
        }
    }
}
