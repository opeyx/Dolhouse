﻿using System;
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
        /// Init Binary Writer
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
            if (Endian == DhEndian.Big) { Array.Reverse(value); } // TODO: Fix this.
            Writer.Write(value);
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
        /// Write signed byte.
        /// </summary>
        public void WriteS8(sbyte value)
        {
            Writer.Write(value);
        }

        #endregion


        #region Ushort / short

        /// <summary>
        /// Write unsigned short.
        /// </summary>
        public void WriteU16(ushort value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write signed short.
        /// </summary>
        public void WriteS16(short value)
        {
            Write(BitConverter.GetBytes(value));
        }

        #endregion


        #region Uint / int

        /// <summary>
        /// Write unsigned integer.
        /// </summary>
        public void WriteU32(uint value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write signed integer.
        /// </summary>
        public void WriteS32(int value)
        {
            Write(BitConverter.GetBytes(value));
        }

        #endregion


        #region Ulong / long

        /// <summary>
        /// Write unsigned long.
        /// </summary>
        public void WriteU64(ulong value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write signed long.
        /// </summary>
        public void WriteS64(long value)
        {
            Write(BitConverter.GetBytes(value));
        }

        #endregion


        #region Float16 / float32 / float64

        /// <summary>
        /// Write float16.
        /// </summary>
        public void WriteF16(short value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write float32
        /// </summary>
        public void WriteF32(float value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write float64
        /// </summary>
        public void WriteF64(double value)
        {
            Write(BitConverter.GetBytes(value));
        }

        #endregion


        #region String (WORK IN PROGRESS)

        // TODO - Add string functionality.

        #endregion
        

        #region Padding

        /// <summary>
        /// Write padding.
        /// </summary>
        /// <param name="count">Count of bytes to write.</param>
        public void WritePadding(int count)
        {
            Write(new byte[count]);
        }

        /// <summary>
        /// Write padding to nearest whole 16.
        /// </summary>
        public void WritePadding16()
        {
            while (Writer.BaseStream.Position % 16 != 0)
            {
                Write(0);
            }
        }

        /// <summary>
        /// Write padding to nearest whole 32.
        /// </summary>
        public void WritePadding32()
        {
            while (Writer.BaseStream.Position % 32 != 0)
            {
                Write(0);
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