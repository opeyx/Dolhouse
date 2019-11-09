namespace Dolhouse.Type
{

    /// <summary>
    /// Custom Color4
    /// </summary>
    public class Clr4
    {

        #region Properties

        /// <summary>
        /// The Red value of the Color4.
        /// </summary>
        public float R { get; set; }

        /// <summary>
        /// The Green value of the Color4.
        /// </summary>
        public float G { get; set; }

        /// <summary>
        /// The Blue value of the Color4.
        /// </summary>
        public float B { get; set; }

        /// <summary>
        /// The Alpha value of the Color4.
        /// </summary>
        public float A { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty Clr4.
        /// </summary>
        public Clr4()
        {
            R = 0.0f;
            G = 0.0f;
            B = 0.0f;
            A = 1.0f;
        }

        /// <summary>
        /// Initialize a new Color4 by a single value. (Alpha = 255)
        /// </summary>
        /// <param name="value">Value to set R, G, B to.</param>
        public Clr4(byte value)
        {
            R = (float)value / 255;
            G = (float)value / 255;
            B = (float)value / 255;
            A = 255;
        }

        /// <summary>
        /// Initialize a new Color4 by three values. (Alpha = 255)
        /// </summary>
        /// <param name="r">Value to set R to.</param>
        /// <param name="g">Value to set G to.</param>
        /// <param name="b">Value to set B to.</param>
        public Clr4(byte r, byte g, byte b)
        {
            R = (float)r / 255;
            G = (float)g / 255;
            B = (float)b / 255;
            A = 255;
        }

        /// <summary>
        /// Initialize a new Color4 by four values.
        /// </summary>
        /// <param name="r">Value to set R to.</param>
        /// <param name="g">Value to set G to.</param>
        /// <param name="b">Value to set B to.</param>
        /// <param name="a">Value to set A to.</param>
        public Clr4(byte r, byte g, byte b, byte a)
        {
            R = (float)r / 255;
            G = (float)g / 255;
            B = (float)b / 255;
            A = (float)a / 255;
        }
    }
}
