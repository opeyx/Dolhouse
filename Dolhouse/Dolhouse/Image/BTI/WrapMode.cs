namespace Dolhouse.Image.BTI
{

    /// <summary>
    /// Defines how textures handle going out of [0..1] range for texcoords.
    /// ---------------------------------------------------------------
    /// Full credits to Sage-Of-Mirrors:
    /// https://github.com/Sage-of-Mirrors/BooldozerCore/blob/master/BooldozerCore/src/Models/Materials/BinaryTextureImage.cs
    /// </summary>
    public enum WrapMode
    {
        ClampToEdge = 0,
        Repeat = 1,
        MirroredRepeat = 2,
    }
}
