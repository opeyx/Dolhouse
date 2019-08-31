using Dolhouse.Binary;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dolhouse.Models.Bin
{

    /// <summary>
    /// (Bin)ary Model
    /// </summary>
    public class BIN
    {

        #region Properties

        /// <summary>
        /// Bin version. Should read either 0x01 or 0x02.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Bin model name.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Bin offsets. There's always 21 offsets!
        /// </summary>
        public List<uint> Offsets { get; set; }

        /// <summary>
        /// Bin textures.
        /// </summary>
        public List<BinTexture> Textures { get; set; }

        /// <summary>
        /// Bin materials.
        /// </summary>
        public List<BinMaterial> Materials { get; set; }

        /// <summary>
        /// Bin positions.
        /// </summary>
        public List<BinPosition> Positions { get; set; }

        /// <summary>
        /// Bin normals.
        /// </summary>
        public List<BinNormal> Normals { get; set; }

        /// <summary>
        /// Bin texture coordinates.
        /// </summary>
        public List<BinTextureCoordinate> TextureCoordinates { get; set; }

        /// <summary>
        /// Bin normals.
        /// </summary>
        public List<BinShader> Shaders { get; set; }

        #endregion

        /// <summary>
        /// Reads BIN from a stream.
        /// </summary>
        /// <param name="stream">The stream containing the BIN data.</param>
        public BIN(Stream stream)
        {

            // Define a binary reader to read with.
            DhBinaryReader br = new DhBinaryReader(stream, DhEndian.Big);

            // Read bin version.
            Version = br.Read();

            // Make sure the bin version is either 0x01 or 0x02.
            if (Version == 0x00 || Version > 0x02 )
            { throw new Exception(string.Format("{0} is not a valid bin version!", Version.ToString())); }

            // Read bin model name.
            ModelName = br.ReadStr(11).Trim('\0');

            // Define a new list to hold the bin's offsets.
            Offsets = new List<uint>();

            // Loop through the bin's offsets.
            for (int i = 0; i < 21; i++)
            {
                // Read offset and add it to the offsets list.
                Offsets.Add(br.ReadU32());
            }


            // Go to the bin textures offset.
            br.Goto(Offsets[0]);

            // Define a new list to hold the bin's textures.
            Textures = new List<BinTexture>();

            // Loop through bin's textures. TODO: This is static for now, add automatic texture count.
            for (int i = 0; i < 3; i++)
            {
                // Read texture and add it to the textures list.
                Textures.Add(new BinTexture(br, Offsets[0]));
            }


            // Go to the bin materials offset.
            br.Goto(Offsets[1]);

            // Define a new list to hold the bin's materials.
            Materials = new List<BinMaterial>();

            // Loop through bin's materials. TODO: This is static for now, add automatic material count.
            for (int i = 0; i < 3; i++)
            {
                // Read texture and add it to the materials list.
                Materials.Add(new BinMaterial(br));
            }


            // Go to the bin positions offset.
            br.Goto(Offsets[2]);

            // Define a new list to hold the bin's positions.
            Positions = new List<BinPosition>();

            // Loop through bin's positions. TODO: Fix this; This is a pretty shitty way to calculate amount of bin positions ...
            for (int i = 0; i < ((Math.Floor((decimal)(Offsets[3] - Offsets[2])) / 6) - 1); i++)
            {
                // Skip 6 bytes to "simulate" reading a bin position.
                br.Skip(6);

                // Make sure the currenet position has not passed the normals offset.
                if(!(br.Position() > Offsets[3]))
                {
                    // Go back 6 bytes as we just "simulated" to read a bin position.
                    br.Goto(br.Position() - 6);

                    // Read a bin position and add it to the positions list.
                    Positions.Add(new BinPosition(br));
                }
            }


            // Go to the bin normals offset.
            br.Goto(Offsets[3]);

            // Define a new list to hold the bin's normals.
            Normals = new List<BinNormal>();

            // Loop through bin's normals. TODO: This is static for now, add automatic normal count.
            for (int i = 0; i < 69; i++)
            {

                // Read a bin normal and add it to the normals list.
                Normals.Add(new BinNormal(br));
            }


            // Go to the bin texture coordinates offset.
            br.Goto(Offsets[6]);

            // Define a new list to hold the bin's texture coordinates.
            TextureCoordinates = new List<BinTextureCoordinate>();

            // Loop through bin's texture coordinates. TODO: This is static for now, add automatic texture coordinates count.
            for (int i = 0; i < 72; i++)
            {

                // Read a bin texture coordinates and add it to the texture coordinates list.
                TextureCoordinates.Add(new BinTextureCoordinate(br));
            }


            // Go to the bin shaders offset.
            br.Goto(Offsets[10]);

            // Define a new list to hold the bin's shaders.
            Shaders = new List<BinShader>();

            // Loop through bin's shaders. TODO: This is static for now, add automatic shader count.
            for (int i = 0; i < 3; i++)
            {

                // Read a bin texture coordinates and add it to the shaders list.
                Shaders.Add(new BinShader(br));
            }


            // Go to the bin batches offset.
            br.Goto(Offsets[11]);
        }

        /// <summary>
        /// Creates a stream from this BIN.
        /// </summary>
        /// <returns>The BIN as a stream.</returns>
        public Stream Write()
        {

            // Define a stream to hold our BIN data.
            Stream stream = new MemoryStream();

            // Define a binary writer to write with.
            DhBinaryWriter bw = new DhBinaryWriter(stream, DhEndian.Big);

            // Write BIN's Version.
            bw.Write(Version);

            // Write BIN's Model Name.
            bw.WriteStr(ModelName);

            // Loop through BIN's Offsets.
            for (int i = 0; i < Offsets.Count; i++)
            {
                // Write BIN Offset.
                bw.WriteU32(Offsets[i]);
            }

            // TODO: WRITE THE REST OF BIN.

            return stream;
        }

        /// <summary>
        /// Creates a Wavefront Obj from this BIN.
        /// </summary>
        /// <returns>The BIN as a Wavefront Obj.</returns>
        public string ToObj()
        {
            string objString = "# This file was generated by Dolhouse.\r\n\r\n";

            objString += "# Vertices\r\n\r\n";
            for (int i = 0; i <Positions.Count; i++)
            {

                objString += "v " + Positions[i].X.ToString() + " " + Positions[i].Y.ToString() + " " + Positions[i].Z.ToString() + "\r\n";
            }

            objString += "# Texture Coordinates\r\n\r\n";

            for (int i = 0; i < TextureCoordinates.Count; i++)
            {

                objString += "vt " + TextureCoordinates[i].X.ToString() + " " + TextureCoordinates[i].Y.ToString() + "\r\n";
            }


            objString += "# Normals\r\n\r\n";

            for (int i = 0; i < Normals.Count; i++)
            {

                objString += "vn " + Normals[i].X.ToString() + " " + Normals[i].Y.ToString() + "\r\n";
            }

            objString += "# Faces\r\n\r\n";

            return objString;
        }
    }
}
