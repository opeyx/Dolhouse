using System;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// Render Flags
    /// 0x0001 = ??? - 0x0002 = ??? - 0x0004 = invisible (except in GBH view)
    /// 0x0008 = transparent (when luigi is behind it)? - 0x0010 = ??? (makes obj not render at all)
    /// 0x0020 = ??? - 0x0040 = fullbright (ingore lighting) - 0x0080 = transparent/ceiling (used on foyer ceiling)
    /// Credits to: @arookas
    /// </summary>
    [Flags]
    public enum RenderFlags : ushort
    {
        None = 0,
        Opaque = 1 << 0,
        Translucent = 1 << 1,
        BoundingBox = 1 << 2,
        FourthWall = 1 << 3,
        Ceiling = 1 << 4,
        NBT = 1 << 5
    }
}
