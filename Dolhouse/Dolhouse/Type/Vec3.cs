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

        /// <summary>
        /// Gets the length (magnitude) of the Vec3.
        /// </summary>
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

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

        /// <summary>
        /// Return the Vec3 as a string.
        /// </summary>
        /// <returns>The Vec3 formatted as a string.</returns>
        public override string ToString()
        {
            return "(" + X.ToString("n6") + ", " + Y.ToString("n6") + ", " + Z.ToString("n6") + ")";
        }


        #region Static Methods

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized vector.</returns>
        public static Vec3 Normalize(Vec3 vec)
        {
            vec.X *= (1.0f / vec.Length);
            vec.Y *= (1.0f / vec.Length);
            vec.Z *= (1.0f / vec.Length);
            return vec;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <returns>Result of the addition.</returns>
        public static Vec3 Add(Vec3 left, Vec3 right)
        {
            return new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <returns>Result of the subtraction.</returns>
        public static Vec3 Subtract(Vec3 left, Vec3 right)
        {
            return new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Multiplies a vector by the components a vector (scale).
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <param>Result of the multiplication.</param>
        public static Vec3 Multiply(Vec3 left, Vec3 right)
        {
            return new Vec3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        /// <summary>
        /// Divide a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <param>Result of the division.</param>
        public static Vec3 Divide(Vec3 left, Vec3 right)
        {
            return new Vec3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        /// <summary>
        /// Calculate the dot (scalar) product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <param>The dot product of the two inputs</param>
        public static float Dot(Vec3 left, Vec3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        /// <summary>
        /// Caclulate the cross (vector) product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">First operand.</param>
        /// <returns>The cross product of the two inputs</returns>
        public static Vec3 Cross(Vec3 left, Vec3 right)
        {
            return new Vec3(
                (left.Y * right.Z - left.Z * right.Y),
                (left.Z * right.X - left.X * right.Z),
                (left.X * right.Y - left.Y * right.X)
            );
        }

        #endregion
    }
}
