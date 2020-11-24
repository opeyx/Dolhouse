namespace Dolhouse.Type
{

    /// <summary>
    /// Custom Vector2
    /// </summary>
    public class Vec2
    {

        #region Properties

        /// <summary>
        /// The X value of the Vec2.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y value of the Vec2.
        /// </summary>
        public float Y { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty Vec2.
        /// </summary>
        public Vec2()
        {
            X = 0.0f;
            Y = 0.0f;
        }

        /// <summary>
        /// Initialize a new Vec2 by a single value.
        /// </summary>
        /// <param name="value">Value to set X and Y to.</param>
        public Vec2(float value)
        {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Initialize a new Vec2 by two values.
        /// </summary>
        /// <param name="x">Value to set X to.</param>
        /// <param name="y">Value to set Y to.</param>
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Defines a unit-length Vec2 that points towards the X-axis.
        /// </summary>
        public static Vec2 UnitX = new Vec2(1, 0);

        /// <summary>
        /// Defines a unit-length Vec2 that points towards the X-axis.
        /// </summary>
        public static Vec2 UnitY = new Vec2(0, 1);

        /// <summary>
        /// Return the Vec2 as a string.
        /// </summary>
        /// <returns>The Vec2 formatted as a string.</returns>
        public override string ToString()
        {
            return "(" + X.ToString("n6") + ", " + Y.ToString("n6") + ")";
        }
    }
}
