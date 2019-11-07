using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Type
{
    public class Vec2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vec2()
        {
            X = 0.0f;
            Y = 0.0f;
        }

        public Vec2(float value)
        {
            X = value;
            Y = value;
        }

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
