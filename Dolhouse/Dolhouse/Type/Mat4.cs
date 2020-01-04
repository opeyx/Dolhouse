namespace Dolhouse.Type
{

    /// <summary>
    /// Custom Matrix4
    /// TODO: Proper formatting for ToString().
    /// </summary>
    public class Mat4
    {

        #region Properties

        /// <summary>
        /// The first Vec4 of the Mat4.
        /// </summary>
        public Vec4 Row1 { get; set; }

        /// <summary>
        /// The second Vec4 of the Mat4.
        /// </summary>
        public Vec4 Row2 { get; set; }

        /// <summary>
        /// The third Vec4 of the Mat4.
        /// </summary>
        public Vec4 Row3 { get; set; }

        /// <summary>
        /// The fourth Vec4 of the Mat4.
        /// </summary>
        public Vec4 Row4 { get; set; }

        #endregion


        /// <summary>
        /// Initialize a new empty Mat4.
        /// </summary>
        public Mat4()
        {
            Row1 = new Vec4();
            Row2 = new Vec4();
            Row3 = new Vec4();
            Row4 = new Vec4();
        }

        /// <summary>
        /// Initialize a new Mat4 by four Vec4's.
        /// </summary>
        /// <param name="row1">Value to set row1 to.</param>
        /// <param name="row2">Value to set row2 to.</param>
        /// <param name="row3">Value to set row3 to.</param>
        /// <param name="row4">Value to set row4 to.</param>
        public Mat4(Vec4 row1, Vec4 row2, Vec4 row3, Vec4 row4)
        {
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
            Row4 = row4;
        }

        /// <summary>
        /// Return the Mat4 as a string.
        /// </summary>
        /// <returns>The Mat4 formatted as a string.</returns>
        public override string ToString()
        {
            return "(" + Row1.ToString() + ", " + Row2.ToString() + ", " + Row3.ToString() + ", " + Row4.ToString() + ")";
        }
    }
}
