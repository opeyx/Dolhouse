using System;

namespace Dolhouse.Models.GX
{

    /// <summary>
    /// GX Attributes
    /// </summary>
    [Flags]
    public enum Attributes : uint
    {
        PosNormMatrix = 1 << 0,
        Tex0Matrix = 1 << 1,
        Tex1Matrix = 1 << 2,
        Tex2Matrix = 1 << 3,
        Tex3Matrix = 1 << 4,
        Tex4Matrix = 1 << 5,
        Tex5Matrix = 1 << 6,
        Tex6Matrix = 1 << 7,
        Tex7Matrix = 1 << 8,
        Position = 1 << 9,
        Normal = 1 << 10,
        Color0 = 1 << 11,
        Color1 = 1 << 12,
        TexCoord0 = 1 << 13,
        TexCoord1 = 1 << 14,
        TexCoord2 = 1 << 15,
        TexCoord3 = 1 << 16,
        TexCoord4 = 1 << 17,
        TexCoord5 = 1 << 18,
        TexCoord6 = 1 << 19,
        TexCoord7 = 1 << 20,
        PositionMatrixArray = 1 << 21,
        NormalMatrixArray = 1 << 22,
        TexMatrixArray = 1 << 23,
        LightArray = 1 << 24,
        NormalBinormalTangent = 1 << 25,
    }
}
