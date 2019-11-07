using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Type
{
    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vec3()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }
        public Vec3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
