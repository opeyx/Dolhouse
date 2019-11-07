using Dolhouse.Type;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dolhouse.Binary
{

    /// <summary>
    /// Custom Binary Reader
    /// </summary>
    public class DhBinaryReader
    {

        #region Properties

        /// <summary>
        /// The underlying BinaryReader.
        /// </summary>
        private BinaryReader Reader;
        
        /// <summary>
        /// The endian to use.
        /// </summary>
        private DhEndian Endian;
        
        /// <summary>
        /// The encoding to use.
        /// </summary>
        private Encoding Encoding;

        /// <summary>
        /// The position of the anchor.
        /// </summary>
        private List<long> AnchorOffsets = new List<long>();

        #endregion


        #region Constructors

        /// <summary>
        /// Init Binary Reader.
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        public DhBinaryReader(Stream stream, DhEndian endian)
        {
            Reader = new BinaryReader(stream, Encoding.UTF8);
            Endian = endian;
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Init Binary Reader. (Custom Encoding)
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        public DhBinaryReader(Stream stream, DhEndian endian, Encoding encoding)
        {
            Reader = new BinaryReader(stream, encoding);
            Endian = endian;
            Encoding = encoding;
        }

        #endregion


        #region Operations

        /// <summary>
        /// Sets the reader's endian to the one specified.
        /// <param name="endian">Endian to change to.</param>
        /// </summary>
        public void SetEndian(DhEndian endian)
        {
            Endian = endian;
        }

        /// <summary>
        /// Read a single byte.
        /// </summary>
        /// <returns>The byte you read.</returns>
        public byte Read()
        {
            return Reader.ReadByte();
        }

        /// <summary>
        /// Reads a specified number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>Array of bytes read.</returns>
        public byte[] Read(int count)
        {
            byte[] data = Reader.ReadBytes(count);
            if (Endian == DhEndian.Big) { Array.Reverse(data); } // TODO: Fix this.
            return data;
        }

        /// <summary>
        /// Retrieve Binary Reader's Basestream.
        /// </summary>
        /// <returns>The reader's basestream.</returns>
        public Stream GetStream()
        {
            return Reader.BaseStream;
        }

        #endregion


        #region Byte / sbyte

        /// <summary>
        /// Read unsigned byte.
        /// </summary>
        /// <returns>The read unsigned byte.</returns>
        public byte ReadU8()
        {
            return Read();
        }

        /// <summary>
        /// Read array of unsigned bytes.
        /// </summary>
        /// <returns>The read array of unsigned bytes.</returns>
        public byte[] ReadU8s(int count)
        {
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = Read();
            }
            return data;
        }

        /// <summary>
        /// Read signed byte.
        /// </summary>
        /// <returns>The read signed byte.</returns>
        public sbyte ReadS8()
        {
            return (sbyte)Read();
        }

        /// <summary>
        /// Read array of signed bytes.
        /// </summary>
        /// <returns>The read array of signed bytes.</returns>
        public sbyte[] ReadS8s(int count)
        {
            sbyte[] data = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = (sbyte)Read();
            }
            return data;
        }

        #endregion


        #region Ushort / short

        /// <summary>
        /// Read unsigned short.
        /// </summary>
        /// <returns>The read unsigned short.</returns>
        public ushort ReadU16()
        {
            return BitConverter.ToUInt16(Read(2), 0);
        }

        /// <summary>
        /// Read array of unsigned shorts.
        /// </summary>
        /// <returns>The read array of unsigned shorts.</returns>
        public ushort[] ReadU16s(int count)
        {
            ushort[] data = new ushort[count];
            for(int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToUInt16(Read(2), 0);
            }
            return data;
        }

        /// <summary>
        /// Read signed short.
        /// </summary>
        /// <returns>The read signed short.</returns>
        public short ReadS16()
        {
            return BitConverter.ToInt16(Read(2), 0);
        }

        /// <summary>
        /// Read array of signed shorts.
        /// </summary>
        /// <returns>The read array of signed shorts.</returns>
        public short[] ReadS16s(int count)
        {
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToInt16(Read(2), 0);
            }
            return data;
        }

        #endregion


        #region Uint / int

        /// <summary>
        /// Read unsigned integer.
        /// </summary>
        /// <returns>The read unsigned integer.</returns>
        public uint ReadU32()
        {
            return BitConverter.ToUInt32(Read(4), 0);
        }

        /// <summary>
        /// Read array of unsigned integers.
        /// </summary>
        /// <returns>The read array of unsigned integers.</returns>
        public uint[] ReadU32s(int count)
        {
            uint[] data = new uint[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToUInt32(Read(4), 0);
            }
            return data;
        }

        /// <summary>
        /// Read signed integer.
        /// </summary>
        /// <returns>The read signed integer.</returns>
        public int ReadS32()
        {
            return BitConverter.ToInt32(Read(4), 0);
        }

        /// <summary>
        /// Read array of signed integers.
        /// </summary>
        /// <returns>The read array of signed integers.</returns>
        public int[] ReadS32s(int count)
        {
            int[] data = new int[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToInt32(Read(4), 0);
            }
            return data;
        }

        #endregion


        #region Ulong / long

        /// <summary>
        /// Read unsigned long.
        /// </summary>
        /// <returns>The read unsigned long.</returns>
        public ulong ReadU64()
        {
            return BitConverter.ToUInt64(Read(8), 0);
        }

        /// <summary>
        /// Read array of unsigned longs.
        /// </summary>
        /// <returns>The read array of unsigned longs.</returns>
        public ulong[] ReadU64s(int count)
        {
            ulong[] data = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToUInt64(Read(8), 0);
            }
            return data;
        }

        /// <summary>
        /// Read signed long.
        /// </summary>
        /// <returns>The read signed long.</returns>
        public long ReadS64()
        {
            return BitConverter.ToInt64(Read(8), 0);
        }

        /// <summary>
        /// Read array of signed longs.
        /// </summary>
        /// <returns>The read array of signed longs.</returns>
        public long[] ReadS64s(int count)
        {
            long[] data = new long[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToInt64(Read(8), 0);
            }
            return data;
        }

        #endregion


        #region Float16 / float32 / float64

        /// <summary>
        /// Read 16-bit float.
        /// </summary>
        /// <returns>The read 16-bit float.</returns>
        public short ReadF16()
        {
            return BitConverter.ToInt16(Read(2), 0);
        }

        /// <summary>
        /// Read array of 16-bit floats.
        /// </summary>
        /// <returns>The read array of 16-bit floats.</returns>
        public short[] ReadF16s(int count)
        {
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToInt16(Read(2), 0);
            }
            return data;
        }

        /// <summary>
        /// Read 32-bit float.
        /// </summary>
        /// <returns>The read 32-bit float.</returns>
        public float ReadF32()
        {
            return BitConverter.ToSingle(Read(4), 0);
        }

        /// <summary>
        /// Read array of 32-bit floats.
        /// </summary>
        /// <returns>The read array of 32-bit floats.</returns>
        public float[] ReadF32s(int count)
        {
            float[] data = new float[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToSingle(Read(4), 0);
            }
            return data;
        }

        /// <summary>
        /// Read 64-bit float.
        /// </summary>
        /// <returns>The read 64-bit float.</returns>
        public double ReadF64()
        {
            return BitConverter.ToDouble(Read(8), 0);
        }

        /// <summary>
        /// Read array of 64-bit floats.
        /// </summary>
        /// <returns>The read array of 64-bit floats.</returns>
        public double[] ReadF64s(int count)
        {
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = BitConverter.ToDouble(Read(8), 0);
            }
            return data;
        }

        #endregion


        #region Char

        /// <summary>
        /// Read a char.
        /// </summary>
        /// <returns>The read char.</returns>
        public char ReadChar()
        {
            return (char)Read();
        }

        /// <summary>
        /// Reads a specified number of chars.
        /// </summary>
        /// <param name="count">The number of chars to read.</param>
        /// <returns>Array of chars read.</returns>
        public char[] ReadChars(int count)
        {
            return Encoding.GetChars(Read(count));
        }

        #endregion


        #region String

        /// <summary>
        /// Read string from stream. (Null-Terminated)
        /// </summary>
        /// <returns>Null-Terminated string.</returns>
        public string ReadStr()
        {
            List<char> chars = new List<char>();
            while (Peek() != 0)
            {
                chars.Add((char)Read());
            }
            return new string(chars.ToArray());
        }

        /// <summary>
        /// Read string of specific length.
        /// </summary>
        /// <param name="count">The string length to read.</param>
        /// <returns>String that was read.</returns>
        public string ReadStr(int count)
        {
            List<char> chars = new List<char>();
            for(int i = 0; i < count; i++)
            {
                chars.Add((char)Read());
            }
            return Encoding.GetString(Encoding.GetBytes(chars.ToArray()));
        }

        /// <summary>
        /// Read 32 byte long string from stream. (Return first NT part only)
        /// </summary>
        /// <returns>First Null-Terminated part of string.</returns>
        public string ReadStr32()
        {
            List<char> chars = new List<char>();
            int pos = 0;
            while (Peek() != 0)
            {
                chars.Add((char)Read());
                pos++;
            }
            Skip(32 - pos);
            return new string(chars.ToArray());
        }

        #endregion


        #region Vectors

        /// <summary>
        /// Read Vector2 from stream.
        /// </summary>
        /// <returns>Vector2 that was read.</returns>
        public Vec2 ReadVec2()
        {
            return new Vec2(ReadF32(), ReadF32());
        }

        /// <summary>
        /// Read Vector3 from stream.
        /// </summary>
        /// <returns>Vector3 that was read.</returns>
        public Vec3 ReadVec3()
        {
            return new Vec3(ReadF32(), ReadF32(), ReadF32());
        }

        #endregion


        #region Colors

        /// <summary>
        /// Read Rgba from stream.
        /// </summary>
        /// <returns>Rgba that was read.</returns>
        public Clr4 ReadRgba()
        {
            return new Clr4(Read(), Read(), Read(), Read());
        }

        #endregion


        #region Peek (WORK IN PROGRESS)

        /// <summary>
        /// Peek at next byte.
        /// </summary>
        /// <returns>The byte peek'ed at.</returns>
        private byte Peek()
        {
            byte value = Read();
            Sail(-1);
            return value;
        }

        /// /// <summary>
        /// Peek at specified number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to peek at.</param>
        /// <returns>Array of bytes peeked at.</returns>
        private byte[] Peek(int count)
        {
            byte[] data = Read(count);
            Sail(-count);
            return data;
        }

        #endregion


        #region Skip

        /// <summary>
        /// Skips a single byte.
        /// </summary>
        public void Skip()
        {
            Read();
        }

        /// <summary>
        /// Skips a specified number of bytes.
        /// </summary>
        public void Skip(int count)
        {
            Read(count);
        }

        #endregion


        #region Seek

        /// <summary>
        /// Retrives the reader's current position.
        /// </summary>
        /// <returns>The current position offset in the stream.</returns>
        public long Position()
        {
            return Reader.BaseStream.Position;
        }

        /// <summary>
        /// Saves the reader's current position into the achor offsets list at the
        /// specified index.
        /// </summary>
        public void SaveOffset(int index)
        {
            AnchorOffsets[index] = Reader.BaseStream.Position;
        }

        /// <summary>
        /// Sets the reader's current position to the value stored with the offset stored
        /// in the achor offsets list at the specified index.
        /// </summary>
        public void LoadOffset(int index)
        {
            Goto(AnchorOffsets[index]);
        }

        /// <summary>
        /// Jumps to a offset relative to the beginning of the file.
        /// </summary>
        /// <param name="offset">The offset you wish to seek to - relative to the beginning of the file.</param>
        public void Goto(long offset)
        {
            Reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        /// <summary>
        /// Jumps to a offset relative to the current position in the file.
        /// </summary>
        /// <param name="offset">The offset you wish to seek to - relative to the current position in the file.</param>
        public void Sail(long offset)
        {
            Reader.BaseStream.Seek(offset, SeekOrigin.Current);
        }

        /// <summary>
        /// Jumps to a offset relative to the end of the file.
        /// </summary>
        /// <param name="offset">The offset you wish to seek to - relative to the end of the file.</param>
        public void Back(long offset)
        {
            Reader.BaseStream.Seek(offset, SeekOrigin.End);
        }

        #endregion
    }
}
