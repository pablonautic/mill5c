using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Core.Utility
{
    /// <summary>
    /// Helper class defining math helper methods and constants.
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// The value of PI.
        /// </summary>
        public const float PI = 3.1415926535897932384626433832795f;

        /// <summary>
        /// The value of 2 * PI.
        /// </summary>
        public const float TwoPI = 2 * PI;

        /// <summary>
        /// The value of PI / 2.
        /// </summary>
        public const float PIOver2 = PI / 2;

        /// <summary>
        /// Square root of 2.
        /// </summary>
        public const float Sqrt2 = 1.4142135623730950488016887242097f;

        /// <summary>
        /// Square root of 3.
        /// </summary>
        public const float Sqrt3 = 1.7320508075688772935274463415059f;

        /// <summary>
        /// Changes radians to degrees.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        public static float ToDegrees(float radians)
        {
            return radians / TwoPI * 360f;
        }

        /// <summary>
        /// Changes degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns></returns>
        public static float ToRadians(float degrees)
        {
            return degrees / 360f * TwoPI;
        }

        /// <summary>
        /// Clamps the specified number.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static float Clamp(float num, float min, float max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }
    }
}
