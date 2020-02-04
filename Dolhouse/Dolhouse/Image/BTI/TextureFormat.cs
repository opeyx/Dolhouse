namespace Dolhouse.Image.BTI
{

    /// <summary>
    /// ImageFormat specifies how the data within the image is encoded.
    /// Included is a chart of how many bits per pixel there are, 
    /// the width/height of each block, how many bytes long the
    /// actual block is, and a description of the type of data stored.
    /// ---------------------------------------------------------------
    /// Full credits to Sage-Of-Mirrors:
    /// https://github.com/Sage-of-Mirrors/BooldozerCore/blob/master/BooldozerCore/src/Models/Materials/BinaryTextureImage.cs
    /// </summary>
    public enum TextureFormat
    {
        //Bits per Pixel | Block Width | Block Height | Block Size | Type / Description
        I4 = 0x00,      // 4 | 8 | 8 | 32 | grey
        I8 = 0x01,      // 8 | 8 | 8 | 32 | grey
        IA4 = 0x02,     // 8 | 8 | 4 | 32 | grey + alpha
        IA8 = 0x03,     //16 | 4 | 4 | 32 | grey + alpha
        RGB565 = 0x04,  //16 | 4 | 4 | 32 | color
        RGB5A3 = 0x05,  //16 | 4 | 4 | 32 | color + alpha
        RGBA32 = 0x06,  //32 | 4 | 4 | 64 | color + alpha
        C4 = 0x08,      //4 | 8 | 8 | 32 | palette choices (IA8, RGB565, RGB5A3)
        C8 = 0x09,      //8, 8, 4, 32 | palette choices (IA8, RGB565, RGB5A3)
        C14X2 = 0x0A,   //16 (14 used) | 4 | 4 | 32 | palette (IA8, RGB565, RGB5A3)
        CMPR = 0x0E,    //4 | 8 | 8 | 32 | mini palettes in each block, RGB565 or transparent.
    }
}
