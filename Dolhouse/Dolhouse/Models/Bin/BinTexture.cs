using Dolhouse.Binary;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Bin Texture
    /// </summary>
    public class BinTexture
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
        public byte Format { get; set; }

        /// <summary>
        /// Unknown 1. Always either 0x20 or 0x10, rarely 0x08 (Flags?)
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// Unknown 2. Always 0, padding?
        /// </summary>
        public ushort Unknown2 { get; set; }

        /// <summary>
        /// Texture Data Offset. Offset to texture padding. (Relative to texture header list)
        /// </summary>
        public uint DataOffset { get; set; }

        /// <summary>
        /// Texture Data. Raw BTI data will be stored here.
        /// </summary>
        public byte[] Data { get; set; }

        #endregion


        /// <summary>
        /// Read a single texture from BIN.
        /// </summary>
        /// <param name="br">Binary Reader to use.</param>
        /// <param name="texturesOffset">Offset to textures.</param>
        public BinTexture(DhBinaryReader br, long texturesOffset)
        {
            // Read texture width.
            Width = br.ReadU16();

            // Read texture height.
            Height = br.ReadU16();

            // Read texture format.
            Format = br.ReadU8();

            // Read texture unknown 1. (Flags?)
            Unknown1 = br.ReadU8();

            // Read texture unknown 2. (Padding)
            Unknown2 = br.ReadU16();

            // Read texture data offset.
            DataOffset = br.ReadU32();

            // Save the current position.
            long currentPosition = br.Position();

            // Go to the bin textures offset.
            br.Goto(texturesOffset);

            // Sail to the texture's data offset.
            br.Sail(DataOffset);

            // Read the texture's raw data.
            Data = br.Read((Width * Height) / 2);

            // Go to the previously saved offset.
            br.Goto(currentPosition);
        }

        /// <summary>
        /// Write a single texture with specified Binary Writer. TODO: Fix this.
        /// </summary>
        /// <param name="bw">Binary Writer to use.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write texture width.
            bw.WriteU16(Width);

            // Write texture width.
            bw.WriteU16(Height);

            // Write texture format.
            bw.WriteU8(Format);

            // Write texture unknown 1. (Flags?)
            bw.WriteU8(Unknown1);

            // Write texture unknown 2. (Padding)
            bw.WriteU16(Unknown2);

            // Write texture data offset.
            bw.WriteU32(DataOffset);
        }
    }
}
