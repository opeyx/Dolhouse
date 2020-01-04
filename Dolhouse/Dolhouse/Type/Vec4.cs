namespace Dolhouse.Type
{

    /// <summary>
    /// Custom Vector4
    /// </summary>
    public class Vec4
    {

        #region Properties

        /// <summary>
        /// The X value of the Vec4.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y value of the Vec4.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The Z value of the Vec4.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// The W value of the Vec4.
        /// </summary>
        public float W { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty Vec4.
        /// </summary>
        public Vec4()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
            W = 0.0f;
        }

        /// <summary>
        /// Initialize a new Vec4 by a single value.
        /// </summary>
        /// <param name="value">Value to set X, Y, Z and W to.</param>
        public Vec4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Initialize a new Vec4 by a single vector and float w.
        /// </summary>
        /// <param name="vector">The vector used for X, Y and Z.</param>
        /// <param name="vector">The float used for W.</param>
        public Vec4(Vec3 vector, float w)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            W = w;
        }

        /// <summary>
        /// Initialize a new Vec4 by three values.
        /// </summary>
        /// <param name="x">Value to set X to.</param>
        /// <param name="y">Value to set Y to.</param>
        /// <param name="z">Value to set Z to.</param>
        /// <param name="w">Value to set W to.</param>
        public Vec4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Return the Vec4 as a string.
        /// </summary>
        /// <returns>The Vec4 formatted as a string.</returns>
        public override string ToString()
        {
            return "(" + X.ToString("n6") + ", " + Y.ToString("n6") + ", " + Z.ToString("n6") + ", " + W.ToString("n6") + ")";
        }
    }
}
