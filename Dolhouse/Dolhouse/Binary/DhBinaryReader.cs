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
        /// Read a single byte at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the bytes at.</param>
        /// <returns>The byte you read.</returns>
        public byte ReadAt(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            byte data = Read();
            Goto(currentPosition);
            return data;
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
        /// Reads a specified number of bytes at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the bytes at.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>Array of bytes read.</returns>
        public byte[] ReadAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            byte[] data = Read(count);
            Goto(currentPosition);
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
        /// Read unsigned byte at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the byte at.</param>
        /// <returns>The read unsigned byte.</returns>
        public byte ReadU8At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            byte data = ReadU8();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of unsigned bytes.
        /// </summary>
        /// <param name="count">Amount of unsigned bytes to read.</param>
        /// <returns>The read array of unsigned bytes.</returns>
        public byte[] ReadU8s(int count)
        {
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU8();
            }
            return data;
        }

        /// <summary>
        /// Read array of unsigned bytes at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned bytes at.</param>
        /// <param name="count">Amount of unsigned bytes to read.</param>
        /// <returns>The read array of unsigned bytes.</returns>
        public byte[] ReadU8sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU8();
            }
            Goto(currentPosition);
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
        /// Read signed byte at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed byte at.</param>
        /// <returns>The read signed byte.</returns>
        public sbyte ReadS8At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            sbyte data = ReadS8();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of signed bytes.
        /// </summary>
        /// <param name="count">Amount of signed bytes to read.</param>
        /// <returns>The read array of signed bytes.</returns>
        public sbyte[] ReadS8s(int count)
        {
            sbyte[] data = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS8();
            }
            return data;
        }

        /// <summary>
        /// Read array of signed bytes at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed bytes at.</param>
        /// <param name="count">Amount of signed bytes to read.</param>
        /// <returns>The read array of signed bytes.</returns>
        public sbyte[] ReadS8sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            sbyte[] data = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS8();
            }
            Goto(currentPosition);
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
        /// Read unsigned short at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned short at.</param>
        /// <returns>The read unsigned short.</returns>
        public ushort ReadU16At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            ushort data = ReadU16();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of unsigned shorts.
        /// </summary>
        /// <param name="count">Amount of unsigned shorts to read.</param>
        /// <returns>The read array of unsigned shorts.</returns>
        public ushort[] ReadU16s(int count)
        {
            ushort[] data = new ushort[count];
            for(int i = 0; i < count; i++)
            {
                data[i] = ReadU16();
            }
            return data;
        }

        /// <summary>
        /// Read array of unsigned shorts at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned shorts at.</param>
        /// <param name="count">Amount of unsigned shorts to read.</param>
        /// <returns>The read array of unsigned shorts.</returns>
        public ushort[] ReadU16sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            ushort[] data = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU16();
            }
            Goto(currentPosition);
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
        /// Read signed short at at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed short at.</param>
        /// <returns>The read signed short.</returns>
        public short ReadS16At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            short data = ReadS16();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of signed shorts.
        /// </summary>
        /// <param name="count">Amount of signed shorts to read.</param>
        /// <returns>The read array of signed shorts.</returns>
        public short[] ReadS16s(int count)
        {
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS16();
            }
            return data;
        }

        /// <summary>
        /// Read array of signed shorts.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed shorts at.</param>
        /// <param name="count">Amount of signed shorts to read.</param>
        /// <returns>The read array of signed shorts.</returns>
        public short[] ReadS16sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS16();
            }
            Goto(currentPosition);
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
        /// Read unsigned integer at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned integer at.</param>
        /// <returns>The read unsigned integer.</returns>
        public uint ReadU32At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            uint data = ReadU32();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of unsigned integers.
        /// </summary>
        /// <param name="count">Amount of unsigned integers to read.</param>
        /// <returns>The read array of unsigned integers.</returns>
        public uint[] ReadU32s(int count)
        {
            uint[] data = new uint[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU32();
            }
            return data;
        }

        /// <summary>
        /// Read array of unsigned integers at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned integers at.</param>
        /// <param name="count">Amount of unsigned integers to read.</param>
        /// <returns>The read array of unsigned integers.</returns>
        public uint[] ReadU32sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            uint[] data = new uint[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU32();
            }
            Goto(currentPosition);
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
        /// Read signed integer at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed integer at.</param>
        /// <returns>The read signed integer.</returns>
        public int ReadS32At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            int data = ReadS32();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of signed integers.
        /// </summary>
        /// <param name="count">Amount of signed integers to read.</param>
        /// <returns>The read array of signed integers.</returns>
        public int[] ReadS32s(int count)
        {
            int[] data = new int[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS32();
            }
            return data;
        }

        /// <summary>
        /// Read array of signed integers at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed integers at.</param>
        /// <param name="count">Amount of signed integers to read.</param>
        /// <returns>The read array of signed integers.</returns>
        public int[] ReadS32sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            int[] data = new int[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS32();
            }
            Goto(currentPosition);
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
        /// Read unsigned long at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned long at.</param>
        /// <returns>The read unsigned long.</returns>
        public ulong ReadU64At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            ulong data = ReadU64();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of unsigned longs.
        /// </summary>
        /// <param name="count">Amount of unsigned longs to read.</param>
        /// <returns>The read array of unsigned longs.</returns>
        public ulong[] ReadU64s(int count)
        {
            ulong[] data = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU64();
            }
            return data;
        }

        /// <summary>
        /// Read array of unsigned longs at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the unsigned longs at.</param>
        /// <param name="count">Amount of unsigned longs to read.</param>
        /// <returns>The read array of unsigned longs.</returns>
        public ulong[] ReadU64sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            ulong[] data = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadU64();
            }
            Goto(currentPosition);
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
        /// Read signed long at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed long at.</param>
        /// <returns>The read signed long.</returns>
        public long ReadS64At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            long data = ReadS64();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of signed longs.
        /// </summary>
        /// <param name="count">Amount of signed longs to read.</param>
        /// <returns>The read array of signed longs.</returns>
        public long[] ReadS64s(int count)
        {
            long[] data = new long[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS64();
            }
            return data;
        }

        /// <summary>
        /// Read array of signed longs.
        /// </summary>
        /// <param name="offset">The absolute offset to read the signed longs at.</param>
        /// <param name="count">Amount of signed longs to read.</param>
        /// <returns>The read array of signed longs.</returns>
        public long[] ReadS64sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            long[] data = new long[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS64();
            }
            Goto(currentPosition);
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
            return ReadS16();
        }

        /// <summary>
        /// Read 16-bit float at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the 16-bit float at.</param>
        /// <returns>The read 16-bit float.</returns>
        public short ReadF16At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            short data = ReadS16();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of 16-bit floats.
        /// </summary>
        /// <param name="count">Amount of 16-bit floats to read.</param>
        /// <returns>The read array of 16-bit floats.</returns>
        public short[] ReadF16s(int count)
        {
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS16();
            }
            return data;
        }

        /// <summary>
        /// Read array of 16-bit floats at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the 16-bit floats at.</param>
        /// <param name="count">Amount of 16-bit floats to read.</param>
        /// <returns>The read array of 16-bit floats.</returns>
        public short[] ReadF16sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadS16();
            }
            Goto(currentPosition);
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
        /// Read 32-bit float at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the 32-bit float at.</param>
        /// <returns>The read 32-bit float.</returns>
        public float ReadF32At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            float data = ReadF32();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of 32-bit floats.
        /// </summary>
        /// <param name="count">Amount of 32-bit floats to read.</param>
        /// <returns>The read array of 32-bit floats.</returns>
        public float[] ReadF32s(int count)
        {
            float[] data = new float[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadF32();
            }
            return data;
        }

        /// <summary>
        /// Read array of 32-bit floats at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the 32-bit floats at.</param>
        /// <param name="count">Amount of 32-bit floats to read.</param>
        /// <returns>The read array of 32-bit floats.</returns>
        public float[] ReadF32sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            float[] data = new float[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadF32();
            }
            Goto(currentPosition);
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
        /// Read 64-bit float at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the 64-bit float at.</param>
        /// <returns>The read 64-bit float.</returns>
        public double ReadF64At(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            double data = ReadF64();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of 64-bit floats.
        /// </summary>
        /// <param name="count">Amount of 64-bit floats to read.</param>
        /// <returns>The read array of 64-bit floats.</returns>
        public double[] ReadF64s(int count)
        {
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadF64();
            }
            return data;
        }

        /// <summary>
        /// Read array of 64-bit floats at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the 64-bit floats at.</param>
        /// <param name="count">Amount of 64-bit floats to read.</param>
        /// <returns>The read array of 64-bit floats.</returns>
        public double[] ReadF64sAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadF64();
            }
            Goto(currentPosition);
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
        /// Read a char at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the char at.</param>
        /// <returns>The read char.</returns>
        public char ReadCharAt(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            char data = (char)Read();
            Goto(currentPosition);
            return data;
        }

        /// <summary>
        /// Read array of chars.
        /// </summary>
        /// <param name="count">Amount of chars to read.</param>
        /// <returns>The read array of chars.</returns>
        public char[] ReadChars(int count)
        {
            return Encoding.GetChars(Read(count));
        }

        /// <summary>
        /// Read array of chars at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the chars at.</param>
        /// <param name="count">Amount of chars to read.</param>
        /// <returns>The read array of chars.</returns>
        public char[] ReadCharsAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            char[] data = ReadChars(count);
            Goto(currentPosition);
            return data;
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
        /// Read string from stream at absolute offset. (Null-Terminated)
        /// </summary>
        /// <param name="offset">The absolute offset to read the string at.</param>
        /// <returns>Null-Terminated string.</returns>
        public string ReadStrAt(long offset)
        {
            long currentPosition = Position();
            Goto(offset);
            string result = ReadStr();
            Goto(currentPosition);
            return result;
        }

        /// <summary>
        /// Read string of specific length from stream.
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
        /// Read string of specific length from stream at absolute offset.
        /// </summary>
        /// <param name="offset">The absolute offset to read the string at.</param>
        /// <param name="count">The fixed string length to read.</param>
        /// <returns>String that was read.</returns>
        public string ReadStrAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            string result = ReadStr(count);
            Goto(currentPosition);
            return result;
        }

        /// <summary>
        /// Read string of fixed length from stream. (Return first NT part only)
        /// </summary>
        /// <param name="count">The string length to read.</param>
        /// <returns>First Null-Terminated part of string.</returns>
        public string ReadFixedStr(int count)
        {
            List<char> chars = new List<char>();
            int pos = 0;
            while (Peek() != 0)
            {
                chars.Add((char)Read());
                pos++;
            }
            if (chars.Count > count)
                throw new FormatException();
            Skip(count - pos);
            return new string(chars.ToArray());
        }

        /// <summary>
        /// Read string of fixed length from stream at absolute offset. (Return first NT part only)
        /// </summary>
        /// <param name="offset">The absolute offset to read the string at.</param>
        /// <param name="count">The fixed string length to read.</param>
        /// <returns>First Null-Terminated part of string.</returns>
        public string ReadFixedStrAt(long offset, int count)
        {
            long currentPosition = Position();
            Goto(offset);
            string result = ReadFixedStr(count);
            Goto(currentPosition);
            return result;
        }

        #endregion


        #region Vectors

        /// <summary>
        /// Read Vector2.
        /// </summary>
        /// <returns>The read Vector2.</returns>
        public Vec2 ReadVec2()
        {
            return new Vec2(ReadF32(), ReadF32());
        }

        /// <summary>
        /// Read array of Vector2.
        /// </summary>
        /// <param name="count">Amount of Vector2s to read.</param>
        /// <returns>The read array of Vector2s.</returns>
        public Vec2[] ReadVec2s(int count)
        {
            Vec2[] data = new Vec2[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadVec2();
            }
            return data;
        }

        /// <summary>
        /// Read Vector3.
        /// </summary>
        /// <returns>The read Vector3.</returns>
        public Vec3 ReadVec3()
        {
            return new Vec3(ReadF32(), ReadF32(), ReadF32());
        }

        /// <summary>
        /// Read array of Vector3.
        /// </summary>
        /// <param name="count">Amount of Vector3s to read.</param>
        /// <returns>The read array of Vector3s.</returns>
        public Vec3[] ReadVec3s(int count)
        {
            Vec3[] data = new Vec3[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadVec3();
            }
            return data;
        }

        /// <summary>
        /// Read Vector4.
        /// </summary>
        /// <returns>The read Vector4.</returns>
        public Vec4 ReadVec4()
        {
            return new Vec4(ReadF32(), ReadF32(), ReadF32(), ReadF32());
        }

        /// <summary>
        /// Read array of Vector4.
        /// </summary>
        /// <param name="count">Amount of Vector4s to read.</param>
        /// <returns>The read array of Vector4s.</returns>
        public Vec4[] ReadVec4s(int count)
        {
            Vec4[] data = new Vec4[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadVec4();
            }
            return data;
        }

        #endregion


        #region Matrices

        /// <summary>
        /// Read Mat4.
        /// </summary>
        /// <returns>The read Mat4.</returns>
        public Mat4 ReadMat4()
        {
            return new Mat4(ReadVec4(), ReadVec4(), ReadVec4(), ReadVec4());
        }

        /// <summary>
        /// Read array of Mat4.
        /// </summary>
        /// <param name="count">Amount of Mat4s to read.</param>
        /// <returns>The read array of Mat4s.</returns>
        public Mat4[] ReadMat4s(int count)
        {
            Mat4[] data = new Mat4[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadMat4();
            }
            return data;
        }

        #endregion


        #region Colors

        /// <summary>
        /// Read Clr4 from stream.
        /// </summary>
        /// <returns>The read Clr4.</returns>
        public Clr4 ReadClr4()
        {
            return new Clr4(Read(), Read(), Read(), Read());
        }

        /// <summary>
        /// Read array of Clr4s.
        /// </summary>
        /// <param name="count">Amount of Clr4s to read.</param>
        /// <returns>The read array of Clr4s.</returns>
        public Clr4[] ReadClr4s(int count)
        {
            Clr4[] data = new Clr4[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = ReadClr4();
            }
            return data;
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
