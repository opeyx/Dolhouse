namespace Dolhouse.Image.BTI
{

    /// <summary>
    /// FilterMode specifies what type of filtering the file should use for min/mag.
    /// ---------------------------------------------------------------
    /// Full credits to Sage-Of-Mirrors:
    /// https://github.com/Sage-of-Mirrors/BooldozerCore/blob/master/BooldozerCore/src/Models/Materials/BinaryTextureImage.cs
    /// </summary>
    public enum FilterMode
    {
        /* Valid in both Min and Mag Filter */
        Nearest = 0x0,                  // Point Sampling, No Mipmap
        Linear = 0x1,                   // Bilinear Filtering, No Mipmap

        /* Valid in only Min Filter */
        NearestMipmapNearest = 0x2,     // Point Sampling, Discrete Mipmap
        NearestMipmapLinear = 0x3,      // Bilinear Filtering, Discrete Mipmap
        LinearMipmapNearest = 0x4,      // Point Sampling, Linear MipMap
        LinearMipmapLinear = 0x5        // Trilinear Filtering
    }
}
