using Dolhouse.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Models.Mp
{
    public class TriangleGroup
    {
        public int[] Indices { get; set; }

        public TriangleGroup(DhBinaryReader br)
        {
            List<int> indices = new List<int>();

            while(br.ReadU16() != 0xFFFF)
            {
                br.Sail(-2);

                indices.Add(br.ReadS16());
            }

            Indices = indices.ToArray();
        }
    }
}
