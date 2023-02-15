using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Core.Geometry
{
    /// <summary>
    /// Class representing a point in 3D space
    /// </summary>
    [Serializable]
    public class Point3D : Tuple3D<Point3D>
    {

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Point3D"/> class.
        /// </summary>
        public Point3D()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3D"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Point3D(float x, float y, float z) 
            : base(x,y,z)
        {           
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Point3D operator +(Point3D point, Vector3D vector)
        {
            return new Point3D { X = point.X + vector.X, Y = point.Y + vector.Y, Z = point.Z + vector.Z };
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="end">The end.</param>
        /// <param name="begin">The begin.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3D operator -(Point3D end, Point3D begin)
        {
            return new Vector3D { X = end.X - begin.X, Y = end.Y - begin.Y, Z = end.Z - begin.Z };
        }

        /// <summary>
        /// Determines whether the specified p1 is near p2 in Manhattan distance.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>
        /// 	<c>true</c> if the specified p1 is near; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNear(Point3D p1, Point3D p2)
        {
            return IsNear(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z);
        }

        /// <summary>
        /// Determines whether the specified p1 is near p2 in Manhattan distance.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="z1">The z1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="z2">The z2.</param>
        /// <returns>
        /// 	<c>true</c> if the specified x1 is near; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNear(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            return x1.IsNear(x2) && y1.IsNear(y2) && z1.IsNear(z2);
        }


        #region non-static proxy methods

        /// <summary>
        /// Determines whether the specified x is near.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns>
        /// 	<c>true</c> if the specified x is near; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNear(float x, float y, float z)
        {
            return IsNear(x, y, z, X, Y, Z);
        }

        /// <summary>
        /// Euclid distance between this and other point.
        /// </summary>
        /// <param name="other">the other.</param>
        /// <returns></returns>
        public float Dist(Point3D other)
        {
            return Point3D.Dist(this, other);
        }

        /// <summary>
        /// Squared euclid distance between this and other point, calculates faster than full distance (no Math.Sqrt),
        /// used primarly for distance comparisons.
        /// </summary>
        /// <param name="other">The p1.</param>
        /// <returns></returns>
        public float DistSqr(Point3D other)
        {
            return Point3D.DistSqr(this, other);
        }

        #endregion
    }
}
