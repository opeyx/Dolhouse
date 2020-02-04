namespace Dolhouse.Binary
{
    
    /// <summary>
    /// Endinaness
    /// </summary>
    public enum DhEndian
    {
        Little,
        Big
    }

    /// <summary>
    /// Small helper class for swapping endianess.
    /// ---------------------------------------------------------------
    /// Full credits to lioncash:
    /// https://github.com/lioncash/GameFormatReader/blob/master/GameFormatReader/Common/EndianUtils.cs
    /// </summary>
    public static class DhEndianUtils
    {
        public static short Swap(this short x)
        {
            return (short)Swap((ushort)x);
        }
        public static ushort Swap(this ushort x)
        {
            return (ushort)((x & 0x00FF) << 8 | (x & 0xFF00) >> 8);
        }
        public static int Swap(this int x)
        {
            return (int)Swap((uint)x);
        }
        public static uint Swap(this uint x)
        {
            return ((x & 0x000000FF) << 24) | ((x & 0x0000FF00) << 8) |
                   ((x & 0x00FF0000) >> 8) | ((x & 0xFF000000) >> 24);
        }
        public static long Swap(this long x)
        {
            return (long)Swap((ulong)x);
        }
        public static ulong Swap(this ulong x)
        {
            return (x & 0x00000000000000FFUL) << 56 | (x & 0x000000000000FF00UL) << 40 |
                   (x & 0x0000000000FF0000UL) << 24 | (x & 0x00000000FF000000UL) << 8 |
                   (x & 0x000000FF00000000UL) >> 8 | (x & 0x0000FF0000000000UL) >> 24 |
                   (x & 0x00FF000000000000UL) >> 40 | (x & 0xFF00000000000000UL) >> 56;
        }
    }
}
