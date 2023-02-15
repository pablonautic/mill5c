using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Utility;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.Cutters
{
    /// <summary>
    /// Flat cutter implementation.
    /// </summary>
    public class FlatCutter : ICutter
    {
        /// <summary>
        /// Intersects the specified sphere center.
        /// </summary>
        /// <param name="sphereCenter">The sphere center.</param>
        /// <param name="sphereR">The sphere R.</param>
        /// <returns></returns>
        public override CollisionType Intersect(Point3D sphereCenter, float sphereR)
        {
            return CollisionHelper.CylinderSphere(sphereCenter, sphereR, Position, Orientation, R, H);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override ICutter Clone()
        {
            return CloneHelper<FlatCutter>();
        }
    }
}
