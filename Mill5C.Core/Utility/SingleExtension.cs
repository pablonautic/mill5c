using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace System
{
    /// <summary>
    /// Helper class for manipulating float numbers.
    /// </summary>
    public static class SingleExtension
    {
        /// <summary>
        /// The value considered to be the minimal difference between two values.
        /// </summary>
        public const float Eps = 0.000001f;

        #region extension methods

        /// <summary>
        /// Determines whether the specified f is near the other one.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        /// 	<c>true</c> if the specified f is near; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNear (this Single f, Single other)
        {
            return Math.Abs(f - other) < Eps;
        }

        /// <summary>
        /// Squares the specified f.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static float Square(this Single f)
        {
            return f * f;
        }

        /// <summary>
        /// Return a short string representation of the number [0.000].
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static string ToShortString(this Single f)
        {
            return SingleExtension.ToString(f);
        }

        #endregion

        #region static methods

        /// <summary>
        /// Tries to parse a dot-separated number.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParse(string s, out float result)
        {
            return float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out result);
        }

        /// <summary>
        /// Return a short string representation of the number [0.000].
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns></returns>
        public static string ToString(float num)
        {
            return num.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        #endregion
    }
}
