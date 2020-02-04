using Dolhouse.Binary;
using Dolhouse.Image.BTI;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Texture
    /// (THIS CLASS CURRENTLY NOT IN USE, FOR TEXTURES,
    /// THE BTI CLASS IS USED DIRECTLY INSTEAD)
    /// </summary>
    public class UnusedTexture
    {
        #region Properties

        /// <summary>
        /// Texture Width. Always power of 2.
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// Texture Height. Always power of 2.
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// Texture Format. Same as BTI/TPL; usually S3TC1.
        /// </summary>
        public TextureFormat Format { get; set; }

        /// <summary>
        /// Texture Alpha Flag (Unsure.)
        /// </summary>
        public byte AlphaFlag { get; set; }

        /// <summary>
        /// Unknown 1. (Padding?)
        /// </summary>
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Texture Data Offset. Offset to texture padding. (Relative to texture header list)
        /// </summary>
        public uint DataOffset { get; set; }

        /// <summary>
        /// Texture Data.
        /// </summary>
        public byte[] Data { get; set; }

        #endregion


        /// <summary>
        /// Read a single texture from BIN.
        /// </summary>
        /// <param name="br">The BinaryReader to read with..</param>
        public UnusedTexture(DhBinaryReader br, uint textureOffset)
        {
            // Read texture width.
            Width = br.ReadU16();

            // Read texture height.
            Height = br.ReadU16();

            // Read texture format.
            Format = (TextureFormat)br.Read();

            // Read texture alpha flag (Unsure, but seems logical)
            AlphaFlag = br.Read();

            // Read texture unknown 1. (Padding?)
            Unknown1 = 0;

            // Read texture data offset.
            DataOffset = br.ReadU32();
        }

        /// <summary>
        /// Generate a BinTexture from a BTI.
        /// </summary>
        /// <param name="bti">BTI to load as a BinTexture.</param>
        public UnusedTexture(BTI bti)
        {
            // Set texture width.
            Width = bti.Width;

            // Set texture height.
            Height = bti.Height;

            // Set texture format.
            Format = bti.Format;

            // Set texture alpha flag (Unsure, but seems logical)
            AlphaFlag = bti.AlphaFlag;

            // Set texture unknown 1. (Padding?)
            Unknown1 = 0;

            // Set texture data offset.
            DataOffset = bti.DataOffset;
        }

        /// <summary>
        /// Write a single texture with specified Binary Writer.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write texture width.
            bw.WriteU16(Width);

            // Write texture width.
            bw.WriteU16(Height);

            // Write texture format.
            bw.WriteU8((byte)Format);

            // Write texture unknown 1. (Flags?)
            bw.WriteU8(AlphaFlag);

            // Write texture unknown 2. (Padding)
            bw.WriteU16(Unknown1);

            // Write texture data offset.
            bw.WriteU32(DataOffset);
        }
    }
}
