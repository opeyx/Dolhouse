using Dolhouse.Binary;
using OpenTK;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Engine
{

    public class TXP 
    {
        #region Properties
        
        /// <summary>
        /// List of TXPEntries
        /// <summary>
        List<TXPEntry> TXPEntries { get; set; }

        /// <summary>
        /// Frame count used for every entry
        /// <summary>
        UInt16 FrameCount { get; set; }
        
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
            uint entryCount = br.ReadU32();

            //Read the number of frames in each entry
            FrameCount = br.ReadU32();

            //Read the offset to the frame data, though its unused
            uint frameDataOffset = br.ReadU32();

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
            bw.WriteU16(TXPEntries.Count);

            // Write Frame Count
            bw.WriteU16(FrameCount);

            //Calculate and write frame data offset
            bw.WriteU32(0x08 * TXPEntries.Count);

            bw.SaveOffset(0);
            //Write dummy frame data offsets
            for(int i = 0; i < TXPEntries.Count; i++){
                bw.WriteU64(0);
            }

            //Make a list to store the offsets for each entry's frame data
            List<uint> frameDataOffsets = new List<uint>();

            //Write the entries
            for(int entry = 0; entry < TXPEntries.Count; entry++)
            {
                entry.Write(bw, frameDataOffsets);
            }

            bw.LoadOffset(0);
            //Write frame data offsets
            for(int i = 0; i < TXPEntries.Count; i++){
                bw.WriteU16(1);
                bw.WriteU16(0);
                bw.WriteU32(frameDataOffsets[i]);
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
        short Unknown1 { get; set; }

        /// <summary>
        /// Material Index
        /// </summary>
        short MaterialIndex { get; set; }

        /// <summary>
        /// MDL TexObj indicies
        /// </summary>
        short[] TexObjIndicies { get; set; }

        #endregion

        public TXPEntry(DhBinaryReader br, uint frameCount)
        {

            //Ensure that Uknown 1 and 2 are 1 and 0 respectivley
            if(br.ReadU16() != 1)
            {
                throw new Exception("Unknown1 wasn't 1!");
            }
            
            if(br.ReadU16() != 0)
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
                TexObjIndicies[frame] = br.ReadU16();
            }

            br.LoadOffset(0);

        }

        public Stream Write(DhBinaryWriter bw, List<uint> frameDataOffsets)
        {
            frameDataOffsets.Add(bw.GetStream().Position);
            for(int i = 0; i < TexObjIndicies.Length; i++)
            {
                bw.WriteU16(TexObjIndicies[i]);
            }
        }
    }
}