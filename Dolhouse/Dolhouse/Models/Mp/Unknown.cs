using Dolhouse.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Models.Mp
{
    public class Unknown
    {
        public byte[] Unknown1 { get; set; }

        public Unknown(DhBinaryReader br)
        {
            Unknown1 = br.Read(3);
        }
    }
}
