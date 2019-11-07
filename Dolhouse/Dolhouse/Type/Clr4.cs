using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolhouse.Type
{
    public class Clr4
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public Clr4()
        {
            R = 0.0f;
            G = 0.0f;
            B = 0.0f;
            A = 1.0f;
        }

        public Clr4(byte value)
        {
            R = (float)value / 255;
            G = (float)value / 255;
            B = (float)value / 255;
            A = 255;
        }

        public Clr4(byte r, byte g, byte b)
        {
            R = (float)r / 255;
            G = (float)g / 255;
            B = (float)b / 255;
            A = 255;
        }

        public Clr4(byte r, byte g, byte b, byte a)
        {
            R = (float)r / 255;
            G = (float)g / 255;
            B = (float)b / 255;
            A = (float)a / 255;
        }
    }
}
