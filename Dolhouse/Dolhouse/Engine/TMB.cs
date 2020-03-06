using Dolhouse.Binary;
using Dolhouse.Type;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolhouse.Engine
{

    /// <summary>
    /// (T)i(M)ing (B)ank
    /// </summary>
    public class TMB
    {

        #region Properties

        /// <summary>
        /// Amount of sequences within this bank.
        /// </summary>
        public ushort SequenceCount;

        /// <summary>
        /// Total duration of this timing.
        /// </summary>
        public ushort Duration;

        /// <summary>
        /// Offset to the sequence data.
        /// </summary>
        public uint SequenceDataOffset;

        /// <summary>
        /// List of sequences within this bank.
        /// </summary>
        public List<TMBSequence> Sequences;

        #endregion


        /// <summary>
        /// Reads TMB from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the TMB data.</param>
        public TMB(byte[] data)
        {
            // Define a new binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(data, DhEndian.Big);

            // Read sequence count.
            SequenceCount = br.ReadU16();

            // Read timing duration.
            Duration = br.ReadU16();

            // Read sequence data offset.
            SequenceDataOffset = br.ReadU32();

            // Define a new list to hold our sequences.
            Sequences = new List<TMBSequence>();

            // Goto sequence data offset.
            br.Goto(SequenceDataOffset);

            // Loop through sequences within this bank.
            for (int i = 0; i < SequenceCount; i++)
            {

                // Read a sequence and add it to the list of sequences.
                Sequences.Add(new TMBSequence(br));
            }

            // Loop through sequences within this bank.
            for (int i = 0; i < SequenceCount; i++)
            {

                // Go to this sequence's keyframe data.
                br.Goto(8 + ((Sequences[i].StartIndex * 4)));

                // Loop through keyframe's within this sequence.
                for (int y = 0; y < Sequences[i].KeyFrameCount; y++)
                {

                    // Read this keyframe and add it to the sequence's list of the keyframes.
                    Sequences[i].KeyFrames.Add(new TIMKeyFrame(br, Sequences[i].KeyFrameSize));
                }
            }
        }

        /// <summary>
        /// Creates a byte array from this TMB.
        /// </summary>
        /// <returns>The TMB as a byte array.</returns>
        public byte[] Write()
        {

            // Define a stream to hold our TMB data.
            MemoryStream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write the amount of sequences.
            bw.WriteU16((ushort)Sequences.Count);

            // Find the largest duration value in each keyframe in each sequence.
            float longestDuration = Sequences.Max(s => s.KeyFrames.Max(k => k.Time));

            // Write the largest duration. (This is the duration of the timing)
            bw.WriteU16((ushort)longestDuration);

            // Write placeholder for sequence data offset.
            bw.WriteU32(0);

            // Loop through sequences within this bank.
            foreach (TMBSequence sequence in Sequences)
            {

                // Loop through keyframe's within this sequence.
                foreach (TIMKeyFrame keyframe in sequence.KeyFrames)
                {

                    // Write this keyframe.
                    keyframe.Write(bw);
                }
            }

            // Save the sequence data offset.
            uint sequenceDataOffset = (uint)bw.Position();
            // Goto sequence data offset value.
            bw.Goto(4);
            // Write sequence data offset.
            bw.WriteU32(sequenceDataOffset);
            // Go back to end of stream.
            bw.Back(0);

            // Loop through sequences within this bank.
            foreach (TMBSequence sequence in Sequences)
            {

                // Write this sequence.
                sequence.Write(bw);
            }

            // Pad to nearest whole 32.
            bw.WritePadding32();

            // Return the TMB as byte array.
            return stream.ToArray();
        }
    }

    /// <summary>
    /// (T)i(M)ing (B)ank (Sequence)
    /// </summary>
    public class TMBSequence
    {

        #region Properties

        /// <summary>
        /// Sequence Name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Keyframe count.
        /// </summary>
        public uint KeyFrameCount;

        /// <summary>
        /// Float data start index.
        /// </summary>
        public ushort StartIndex;

        /// <summary>
        /// The amount of floats in each Keyframe.
        /// </summary>
        public ushort KeyFrameSize;

        /// <summary>
        /// List of keyframes within this sequence.
        /// </summary>
        public List<TIMKeyFrame> KeyFrames;

        #endregion


        /// <summary>
        /// Read a single sequence from TMB.
        /// </summary>
        /// <param name="br">The binary reader to read with.</param>
        public TMBSequence(DhBinaryReader br)
        {

            // Read name.
            Name = br.ReadFixedStr(28);

            // Read keyframe count.
            KeyFrameCount = br.ReadU32();

            // Read start index.
            StartIndex = br.ReadU16();

            // Read keyframe size.
            KeyFrameSize = br.ReadU16();

            // Define a new list to hold our keyframes.
            KeyFrames = new List<TIMKeyFrame>();
        }

        public void Write(DhBinaryWriter bw)
        {
            // Write name.
            bw.WriteFixedStr(Name, 28);

            // Write keyframe count.
            bw.WriteU32(KeyFrameCount);

            // Write start index.
            bw.WriteU16(StartIndex);

            // Write keyframe size.
            bw.WriteU16(KeyFrameSize);
        }
    }

    /// <summary>
    /// (T)i(M)ing (B)ank (Key) (Frame)
    /// </summary>
    public class TIMKeyFrame
    {

        #region Properties

        // The time at this keyframe is played.
        public float Time;

        // List of float values for this keyframe.
        public List<float> Data;

        #endregion


        /// <summary>
        /// Read a single keyframe from TMB.
        /// </summary>
        /// <param name="br">The binary reader to read with.</param>
        /// <param name="dataCount">The amount of floats in this keyframe.</param>
        public TIMKeyFrame(DhBinaryReader br, ushort dataCount)
        {

            // Read time.
            Time = br.ReadF32();

            // Read keyframe data.
            Data = br.ReadF32s(dataCount - 1).ToList();
        }

        /// <summary>
        /// Read a single keyframe.
        /// </summary>
        /// <param name="bw">The binary writer to write with.</param>
        public void Write(DhBinaryWriter bw)
        {

            // Write time.
            bw.WriteF32(Time);

            // Write keyframe data.
            bw.WriteF32s(Data.ToArray());
        }
    }
}
