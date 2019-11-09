namespace Dolhouse.Type
{

    /// <summary>
    /// Custom Vector3
    /// </summary>
    public class Vec3
    {

        #region Properties

        /// <summary>
        /// The X value of the Vec3.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y value of the Vec3.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The Z value of the Vec3.
        /// </summary>
        public float Z { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty Vec3.
        /// </summary>
        public Vec3()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        /// <summary>
        /// Initialize a new Vec3 by a single value.
        /// </summary>
        /// <param name="value">Value to set X, Y and Z to.</param>
        public Vec3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Initialize a new Vec3 by three values.
        /// </summary>
        /// <param name="x">Value to set X to.</param>
        /// <param name="y">Value to set Y to.</param>
        /// <param name="z">Value to set Z to.</param>
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
