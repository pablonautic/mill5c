using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Core.Geometry
{
    /// <summary>
    /// Class representing a vector in 3D space
    /// </summary>
    [Serializable]
    public class Vector3D : Tuple3D<Vector3D>
    {

        /// <summary>
        /// Initializes a new (0,0,0) instance of the <see cref="Vector3D"/> class.
        /// </summary>
        public Vector3D()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Vector3D(float x, float y, float z)
            : base(x,y,z)
        {
        }

        /// <summary>
        /// Normalizes this instance.
        /// </summary>
        public void Normalize()
        {
            float len = Length;
            X /= len;
            Z /= len;
            Y /= len;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            get { return (float)Math.Sqrt(LengthSqr); }
        }

        /// <summary>
        /// Gets the square of the length.
        /// </summary>
        /// <value></value>
        public float LengthSqr
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        /// <summary>
        /// Return the dot product of two vectors.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float Dot(Vector3D v1, Vector3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Return the cross product of two vectors.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Vector3D Cross(Vector3D a, Vector3D b)
        {
            var result = new Vector3D();
            result[0] = a[1] * b[2] - a[2] * b[1];
            result[1] = a[2] * b[0] - a[0] * b[2];
            result[2] = a[0] * b[1] - a[1] * b[0];
            return result;
        }

        #region non-static proxy methods

        /// <summary>
        /// Return the cross product of this vector and one other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public Vector3D Cross(Vector3D other)
        {
            return Vector3D.Cross(this, other);
        }

        #endregion

        #region factory methods

        /// <summary>
        /// Gets the Up (0,0,1) vector.
        /// </summary>
        /// <value>Up.</value>
        public static Vector3D Up
        {
            get { return new Vector3D { Z = 1 }; }
        }

        #endregion

    }
}
