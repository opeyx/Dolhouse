using Dolhouse.Binary;
using System.Collections.Generic;
using System.IO;
using System;

namespace Dolhouse.Engine
{

    public class TXP 
    {

        #region Properties
        
        /// <summary>
        /// List of entries.
        /// <summary>
        public List<TXPEntry> TXPEntries { get; set; }

        /// <summary>
        /// Frame count used for every entry
        /// <summary>
        public ushort FrameCount { get; set; }
        
        #endregion

        public TXP(Stream stream)
        {

            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            //Ensure that Uknown 1 and 2 are 1 and 0 respectivley
            if(br.ReadU16() != 1)
            {
                throw new Exception("Unknown1 wasn't 1!");
            }
            
            if(br.ReadU16() != 0)
            {
                throw new Exception("Unknown2 wasn't 0!");
            }
        
            //Read the number of entries in the txp file
            ushort entryCount = br.ReadU16();

            //Read the number of frames in each entry
            FrameCount = br.ReadU16();

            //Skip unused frame offset
            br.ReadU32();

            TXPEntries = new List<TXPEntry>();

            for(int entry = 0; entry < entryCount; entry++)
            {
                TXPEntries.Add(new TXPEntry(br, FrameCount));
            }

        }

        public Stream Write()
        {
            // Buffer for new TXP File
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write the fixed Unknown values
            bw.WriteU16(1);
            bw.WriteU16(0);

            // Write Entry Count
            bw.WriteU16((ushort)TXPEntries.Count);

            // Write Frame Count
            bw.WriteU16(FrameCount);

            //Calculate and write frame data offset
            bw.WriteU32((uint)(0xC * TXPEntries.Count));

            bw.SaveOffset(0);

            //Write dummy entry headers
            for(int i = 0; i < TXPEntries.Count; i++){
                bw.WriteU64(0);
                bw.WriteU32(0);
            }

            //Make a list to store the offsets for each entry's frame data
            List<long> frameDataOffsets = new List<long>();

            //Write the entries
            for(int entry = 0; entry < TXPEntries.Count; entry++)
            {
                TXPEntries[entry].Write(bw, frameDataOffsets);
            }

            bw.LoadOffset(0);
            //Write frame data offsets
            for(int i = 0; i < TXPEntries.Count; i++){
                bw.WriteU16(1);
                bw.WriteS16(TXPEntries[i].MaterialIndex);
                bw.WriteU32(0);
                bw.WriteU32((uint)frameDataOffsets[i]);
            }

            // Returns the TXP as a stream
            return stream;
        }


    }    

    /// <summary>
    /// Entry in TXP File
    /// </summary>
    public class TXPEntry
    {

        #region Properties

        /// <summary>
        /// Unknown Short
        /// </summary>
        public short Unknown1 { get; set; }

        /// <summary>
        /// Material Index
        /// </summary>
        public short MaterialIndex { get; set; }

        /// <summary>
        /// MDL TexObj indicies
        /// </summary>
        public short[] TexObjIndicies { get; set; }

        #endregion

        public TXPEntry(DhBinaryReader br, ushort frameCount)
        {

            //Ensure that Uknown 1 is 1
            if(br.ReadU16() != 1)
            {
                throw new Exception("Unknown1 wasn't 1!");
            }
            
            //Read material index
            MaterialIndex = br.ReadS16();

            if(br.ReadU32() != 0)
            {
                throw new Exception("Unknown2 wasn't 0!");
            }

            //Read the frame data offset for this 
            uint frameDataOffset = br.ReadU32();

            TexObjIndicies = new short[frameCount];

            //Save reader's current position and seek to the frame data
            br.SaveOffset(0);
            br.Goto(frameDataOffset);

            //Fill TexObjIndicies for each frame
            for(int frame = 0; frame < frameCount; frame++) 
            {
                TexObjIndicies[frame] = br.ReadS16();
            }

            br.LoadOffset(0);

        }

        public void Write(DhBinaryWriter bw, List<long> frameDataOffsets)
        {
            frameDataOffsets.Add(bw.GetStream().Position);
            for(int i = 0; i < TexObjIndicies.Length; i++)
            {
                bw.WriteS16(TexObjIndicies[i]);
            }
        }
    }
}