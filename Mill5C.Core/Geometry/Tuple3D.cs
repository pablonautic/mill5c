using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Mill5C.Core.Geometry
{
    /// <summary>
    /// Base class defining a 3-tuple of float coordinates.
    /// </summary>
    /// <typeparam name="T">the target type inheriting from this class</typeparam>
    [Serializable]
    public class Tuple3D<T> where T : Tuple3D<T>, new() 
    {
        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        public float X;

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public float Y;

        /// <summary>
        /// Gets or sets the Z.
        /// </summary>
        /// <value>The Z.</value>
        public float Z;

        /// <summary>
        /// Creates a new empty (X=Y=Z=0) tuple.
        /// </summary>
        public Tuple3D()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tuple3D&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Tuple3D(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public T Clone()
        {
            return new T { X = X, Y = Y, Z = Z };
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> representation formatted: (X,Y,Z).
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current tuple.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }

        /// <summary>
        /// Gets or sets the 0-based <see cref="System.Single"/> with the specified idx.
        /// </summary>
        /// <value>{0,1,2}</value>
        public float this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new ArgumentException("index must be between 0 and 2");
                }
            }
            set
            {
                switch (idx)
                {
                    case 0:
                        X = value;
                        return;
                    case 1:
                        Y = value;
                        return;
                    case 2:
                        Z = value;
                        return;
                    default:
                        throw new ArgumentException("index must be between 0 and 2");
                }
            }
        }

        /// <summary>
        /// A simple sum of two tuples.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>The result of the operator.</returns>
        public static T operator +(Tuple3D<T> t1, Tuple3D<T> t2)
        {
            return new T { X = t1.X + t2.X, Y = t1.Y + t2.Y, Z = t1.Z + t2.Z };
        }

        /// <summary>
        /// Multiplies all coordinates by the given scalar.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The result of the operator.</returns>
        public static T operator *(Tuple3D<T> tuple, float scalar)
        {
            return new T { X = tuple.X * scalar, Y = tuple.Y * scalar, Z = tuple.Z * scalar };
        }

        /// <summary>
        /// Multiplies all coordinates by the given scalar.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="tuple">The tuple.</param>
        /// <returns>The result of the operator.</returns>
        public static T operator *(float scalar, Tuple3D<T> tuple)
        {
            return tuple * scalar;
        }

        /// <summary>
        /// Unary "-" operator.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns>new negated tuple</returns>
        public static T operator -(Tuple3D<T> tuple)
        {
            return new T { X = -tuple.X, Y = -tuple.Y, Z = -tuple.Z };
        }

        /// <summary>
        /// Linear interpolation between two tuples.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="t">amount</param>
        /// <returns></returns>
        public static T Lerp(Tuple3D<T> p1, Tuple3D<T> p2, float t)
        {
            return (1 - t) * p1 + t * p2;
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static T Cerp(T value1, T value2, float amount)
        {
            T vector = new T();
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            vector.Z = value1.Z + ((value2.Z - value1.Z) * amount);
            return vector;
        }

        /// <summary>
        /// Squared euclid distance between two tuples, calculates faster than full distance (no Math.Sqrt),
        /// used primarly for distance comparisons.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns></returns>
        public static float DistSqr(Tuple3D<T> p1, float x, float y, float z)
        {
            return (p1.X - x).Square() + (p1.Y - y).Square() + (p1.Z - z).Square();
        }

        /// <summary>
        /// Squared euclid distance between two tuples, calculates faster than full distance (no Math.Sqrt),
        /// used primarly for distance comparisons.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        public static float DistSqr(Tuple3D<T> p1, Tuple3D<T> p2)
        {
            return DistSqr(p1, p2.X, p2.Y, p2.Z);
        }

        /// <summary>
        /// Euclid distance between two tuples.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <returns></returns>
        public static float Dist(Tuple3D<T> p1, float x, float y, float z)
        {
            return (float)Math.Sqrt(DistSqr(p1, x, y, z));
        }

        /// <summary>
        /// Euclid distance between two tuples.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        public static float Dist(Tuple3D<T> p1, Tuple3D<T> p2)
        {
            return Dist(p1, p2.X, p2.Y, p2.Z);
        }

        /// <summary>
        /// Factory method for creating empty (zero) instances.
        /// </summary>
        /// <value>The zero.</value>
        public static T Zero
        {
            get { return new T(); }
        }

        /// <summary>
        /// Tries to parse the tuple using the given separator.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="result">The result.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static bool TryParse(string text, out T result, string separator)
        {
            result = null;
            string[] vals = text.Split(new string[] { separator }, StringSplitOptions.None);
            if (vals.Length != 3)
                return false;

            float x,y,z;
            if (!SingleExtension.TryParse(vals[0], out x))
                return false;
            if (!SingleExtension.TryParse(vals[1], out y))
                return false;
            if (!SingleExtension.TryParse(vals[2], out z))
                return false;

            result = new T { X = x, Y = y, Z = z };
            return true;
        }

        /// <summary>
        /// Tries to parse semicolon-separated values of a tuple.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParse(string text, out T result)
        {
            return TryParse(text, out result, ";");
        }

    }
}
