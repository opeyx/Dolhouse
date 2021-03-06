﻿using Dolhouse.Type;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dolhouse.Binary
{

    /// <summary>
    /// Custom Binary Writer
    /// </summary>
    public class DhBinaryWriter
    {

        #region Properties

        /// <summary>
        /// The underlying BinaryWriter.
        /// </summary>
        private BinaryWriter Writer;

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
        /// Init Binary Writer (UTF-8 Encoding)
        /// </summary>
        /// <param name="stream">The stream to write data to.</param>
        public DhBinaryWriter(Stream stream, DhEndian endian)
        {
            Writer = new BinaryWriter(stream, Encoding.UTF8);
            Endian = endian;
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Init Binary Writer (Custom Encoding)
        /// </summary>
        /// <param name="stream">The stream to write data to.</param>
        public DhBinaryWriter(Stream stream, DhEndian endian, Encoding encoding)
        {
            Writer = new BinaryWriter(stream, encoding);
            Endian = endian;
            Encoding = encoding;
        }

        #endregion


        #region Operations

        /// <summary>
        /// Sets the writer's endian to the one specified.
        /// <param name="endian">Endian to change to.</param>
        /// </summary>
        public void SetEndian(DhEndian endian)
        {
            Endian = endian;
        }

        /// <summary>
        /// Writes a single byte.
        /// </summary>
        public void Write(byte value)
        {
            Writer.Write(value);
        }

        /// <summary>
        /// Writes a array of bytes.
        /// </summary>
        public void Write(byte[] value)
        {
            Writer.Write(value);
        }

        /// <summary>
        /// Retrieve Binary Writer's Basestream.
        /// </summary>
        /// <returns>The writer's basestream.</returns>
        public Stream GetStream()
        {
            return Writer.BaseStream;
        }

        /// <summary>
        /// Bool to determine if the system endian is
        /// the same as the current endian.
        /// </summary>
        /// <returns>Bool determining if they are the same endian.</returns>
        public bool SameEndian
        {
            get
            {
                return (BitConverter.IsLittleEndian && Endian == DhEndian.Little || !BitConverter.IsLittleEndian && Endian == DhEndian.Big);
            }
        }

        #endregion


        #region Byte / sbyte

        /// <summary>
        /// Write unsigned byte.
        /// </summary>
        public void WriteU8(byte value)
        {
            Writer.Write(value);
        }

        /// <summary>
        /// Write array of unsigned bytes.
        /// </summary>
        /// <param name="value">The array of unsigned bytes to write.</param>
        public void WriteU8s(byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Writer.Write(value[i]);
            }
        }

        /// <summary>
        /// Write signed byte.
        /// </summary>
        public void WriteS8(sbyte value)
        {
            Writer.Write(value);
        }

        /// <summary>
        /// Write array of signed bytes.
        /// </summary>
        /// <param name="value">The array of signed bytes to write.</param>
        public void WriteS8s(sbyte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Writer.Write(value[i]);
            }
        }

        #endregion


        #region Ushort / short

        /// <summary>
        /// Write unsigned short.
        /// </summary>
        public void WriteU16(ushort value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of unsigned shorts.
        /// </summary>
        /// <param name="value">The array of unsigned shorts to write.</param>
        public void WriteU16s(ushort[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        /// <summary>
        /// Write signed short.
        /// </summary>
        public void WriteS16(short value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of signed shorts.
        /// </summary>
        /// <param name="value">The array of signed shorts to write.</param>
        public void WriteS16s(short[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        internal void Write(object unknown2)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Uint / int

        /// <summary>
        /// Write unsigned integer.
        /// </summary>
        public void WriteU32(uint value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of unsigned integers.
        /// </summary>
        /// <param name="value">The array of unsigned integers to write.</param>
        public void WriteU32s(uint[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        /// <summary>
        /// Write signed integer.
        /// </summary>
        /// <param name="value">The array of signed integers to write.</param>
        public void WriteS32(int value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of signed integers.
        /// </summary>
        public void WriteS32s(int[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        #endregion


        #region Ulong / long

        /// <summary>
        /// Write unsigned long.
        /// </summary>
        public void WriteU64(ulong value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of unsigned longs.
        /// </summary>
        /// <param name="value">The array of unsigned longs to write.</param>
        public void WriteU64s(ulong[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        /// <summary>
        /// Write signed long.
        /// </summary>
        public void WriteS64(long value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of signed longs.
        /// </summary>
        /// <param name="value">The array of signed longs to write.</param>
        public void WriteS64s(long[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        #endregion


        #region Float16 / float32 / float64

        /// <summary>
        /// Write float16.
        /// </summary>
        public void WriteF16(short value)
        {
            if (!SameEndian)
            {
                value = value.Swap();
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of float16s.
        /// </summary>
        /// <param name="value">The array of float16s to write.</param>
        public void WriteF16s(short[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    value[i] = value[i].Swap();
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        /// <summary>
        /// Write float32
        /// </summary>
        public void WriteF32(float value)
        {
            if (!SameEndian)
            {
                byte[] floatBytes = BitConverter.GetBytes(value);
                Array.Reverse(floatBytes);
                value = BitConverter.ToSingle(floatBytes, 0);
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of float32s.
        /// </summary>
        /// <param name="value">The array of float32s to write.</param>
        public void WriteF32s(float[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    byte[] floatBytes = BitConverter.GetBytes(value[i]);
                    Array.Reverse(floatBytes);
                    value[i] = BitConverter.ToSingle(floatBytes, 0);
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        /// <summary>
        /// Write float64
        /// </summary>
        public void WriteF64(double value)
        {
            if (!SameEndian)
            {
                byte[] doubleBytes = BitConverter.GetBytes(value);
                Array.Reverse(doubleBytes);
                value = BitConverter.ToDouble(doubleBytes, 0);
            }
            Writer.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write array of float64s.
        /// </summary>
        /// <param name="value">The array of float64s to write.</param>
        public void WriteF64s(double[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!SameEndian)
                {
                    byte[] doubleBytes = BitConverter.GetBytes(value[i]);
                    Array.Reverse(doubleBytes);
                    value[i] = BitConverter.ToDouble(doubleBytes, 0);
                }
                Writer.Write(BitConverter.GetBytes(value[i]));
            }
        }

        #endregion


        #region Char

        /// <summary>
        /// Write char.
        /// </summary>
        /// <param name="value">The char to write.</param>
        public void WriteChar(char value)
        {
            Writer.Write(value);
        }

        /// <summary>
        /// Write array of chars.
        /// </summary>
        /// <param name="value">The array of chars to write.</param>
        public void WriteChars(char[] value)
        {
            Writer.Write(Encoding.GetBytes(value));
        }

        #endregion


        #region String

        /// <summary>
        /// Write string.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public void WriteStr(string value)
        {
            byte[] data = Encoding.GetBytes(value);
            Writer.Write(data);
        }

        /// <summary>
        /// Write string of fixed length. (Will be truncated/padded)
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <param name="count">The fixed length to write.</param>
        /// <param name="padChar">The char you wish to pad with.</param>
        public void WriteFixedStr(string value, int count, char padChar = '\0')
        {
            byte[] data = Encoding.GetBytes(value);
            if (data.Length > count)
            {
                Writer.Write(data, 0, count);
            }
            else
            {
                byte[] padding = new byte[count - data.Length];
                Writer.Write(data);
                for(int i = 0; i < count-data.Length; i++)
                {
                    Writer.Write(padChar);
                }
            }
        }

        #endregion


        #region Vectors

        /// <summary>
        /// Write Vector2.
        /// </summary>
        /// <param name="value">The Vector2 to write.</param>
        public void WriteVec2(Vec2 value)
        {
            WriteF32(value.X);
            WriteF32(value.Y);
        }

        /// <summary>
        /// Write array of Vector2.
        /// </summary>
        /// <param name="value">The array of Vector2s to write.</param>
        public void WriteVec3(Vec2[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                WriteF32(value[i].X);
                WriteF32(value[i].Y);
            }
        }

        /// <summary>
        /// Write Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to write.</param>
        public void WriteVec3(Vec3 value)
        {
            WriteF32(value.X);
            WriteF32(value.Y);
            WriteF32(value.Z);
        }

        /// <summary>
        /// Write array of Vector3.
        /// </summary>
        /// <param name="value">The array of Vector3s to write.</param>
        public void WriteVec3(Vec3[] value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                WriteF32(value[i].X);
                WriteF32(value[i].Y);
                WriteF32(value[i].Z);
            }
        }

        /// <summary>
        /// Write Vector4.
        /// </summary>
        /// <param name="value">The Vector4 to write.</param>
        public void WriteVec4(Vec4 value)
        {
            WriteF32(value.X);
            WriteF32(value.Y);
            WriteF32(value.Z);
            WriteF32(value.W);
        }

        /// <summary>
        /// Write array of Vector4.
        /// </summary>
        /// <param name="value">The array of Vector4s to write.</param>
        public void WriteVec4(Vec4[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                WriteF32(value[i].X);
                WriteF32(value[i].Y);
                WriteF32(value[i].Z);
                WriteF32(value[i].W);
            }
        }

        #endregion


        #region Matrices

        /// <summary>
        /// Write Mat4.
        /// </summary>
        /// <param name="value">The Mat4 to write.</param>
        public void WriteMat4(Mat4 value)
        {
            WriteVec4(value.Row1);
            WriteVec4(value.Row2);
            WriteVec4(value.Row3);
            WriteVec4(value.Row4);
        }

        /// <summary>
        /// Write array of Mat4.
        /// </summary>
        /// <param name="value">The array of Mat4s to write.</param>
        public void WriteMat4(Mat4[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                WriteVec4(value[i].Row1);
                WriteVec4(value[i].Row2);
                WriteVec4(value[i].Row3);
                WriteVec4(value[i].Row4);
            }
        }

        #endregion


        #region Colors

        /// <summary>
        /// Write Clr4.
        /// </summary>
        /// <param name="value">The Clr4 to write.</param>
        public void WriteClr4(Clr4 value)
        {
            uint data =
                (uint)(value.R * byte.MaxValue) << 24 |
                (uint)(value.G * byte.MaxValue) << 16 |
                (uint)(value.B * byte.MaxValue) << 8 |
                (uint)(value.A * byte.MaxValue);
            WriteU32(data);
        }

        /// <summary>
        /// Write array of Clr4.
        /// </summary>
        /// <param name="value">The array of Clr4s to write.</param>
        public void WriteClr4s(Clr4[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                uint data =
                (uint)(value[i].R * byte.MaxValue) << 24 |
                (uint)(value[i].G * byte.MaxValue) << 16 |
                (uint)(value[i].B * byte.MaxValue) << 8 |
                (uint)(value[i].A * byte.MaxValue);
                WriteU32(data);
            }
        }

        #endregion


        #region Padding

        /// <summary>
        /// Write padding.
        /// </summary>
        /// <param name="count">Count of bytes to write.</param>
        /// <param name="padChar">The char you wish to pad with.</param>
        public void WritePadding(int count, char padChar = '\0')
        {
            byte[] data = new byte[count];
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)padChar;
            }
            Write(data);
        }

        /// <summary>
        /// Write padding to nearest whole 16.
        /// </summary>
        /// <param name="padChar">The char you wish to pad with.</param>
        public void WritePadding16(char padChar = '\0')
        {
            while (Writer.BaseStream.Position % 16 != 0)
            {
                Write((byte)padChar);
            }
        }


        /// <summary>
        /// Write padding to nearest whole 32.
        /// </summary>
        /// <param name="padChar">The char you wish to pad with.</param>
        public void WritePadding32(char padChar = '\0')
        {
            while (Writer.BaseStream.Position % 32 != 0)
            {
                Write((byte)padChar);
            }
        }

        #endregion


        #region Seek

        /// <summary>
        /// Retrives the writer's current position.
        /// </summary>
        /// <returns>The current position offset in the stream.</returns>
        public long Position()
        {
            return Writer.BaseStream.Position;
        }

        /// <summary>
        /// Saves the writer's current position into the achor offsets list at the
        /// specified index.
        /// </summary>
        public void SaveOffset(int index)
        {
            AnchorOffsets[index] = Writer.BaseStream.Position;
        }

        /// <summary>
        /// Sets the writer's current position to the value stored with the offset stored
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
            Writer.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        /// <summary>
        /// Jumps to a offset relative to the current position in the file.
        /// </summary>
        /// <param name="offset">The offset you wish to seek to - relative to the current position in the file.</param>
        public void Sail(long offset)
        {
            Writer.BaseStream.Seek(offset, SeekOrigin.Current);
        }

        /// <summary>
        /// Jumps to a offset relative to the end of the file.
        /// </summary>
        /// <param name="offset">The offset you wish to seek to - relative to the end of the file.</param>
        public void Back(long offset)
        {
            Writer.BaseStream.Seek(offset, SeekOrigin.End);
        }

        #endregion
    }
}
