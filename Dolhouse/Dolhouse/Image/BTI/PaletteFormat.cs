namespace Dolhouse.Image.BTI
{

    /// <summary>
    /// PaletteFormat specifies how the data within the palette is stored. An
    /// image uses a single palette (except CMPR which defines its own
    /// mini-palettes within the Image data). Only C4, C8, and C14X2 use
    /// palettes. For all other formats the type and count is zero.
    /// ---------------------------------------------------------------
    /// Full credits to Sage-Of-Mirrors:
    /// https://github.com/Sage-of-Mirrors/BooldozerCore/blob/master/BooldozerCore/src/Models/Materials/BinaryTextureImage.cs
    /// </summary>
    public enum PaletteFormat
    {
        IA8 = 0x00,
        RGB565 = 0x01,
        RGB5A3 = 0x02,
    }
}
