using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Utility;

namespace Mill5C.Core.Cutters
{
    /// <summary>
    /// Ball cutter implementation.
    /// </summary>
    public class BallCutter : ICutter
    {
        /// <summary>
        /// Intersects the specified sphere center.
        /// </summary>
        /// <param name="sphereCenter">The sphere center.</param>
        /// <param name="sphereR">The sphere R.</param>
        /// <returns></returns>
        public override CollisionType Intersect(Mill5C.Core.Geometry.Point3D sphereCenter, float sphereR)
        {
            CollisionType ss = CollisionHelper.SphereSphere(Position, R, sphereCenter, sphereR);
            if (ss == CollisionType.Total)
                return CollisionType.Total;

            CollisionType cs = CollisionHelper.CylinderSphere(sphereCenter, sphereR, Position, Orientation, R, H);
            if (cs == CollisionType.Total)
                return CollisionType.Total;

            if (ss == CollisionType.Partial || cs == CollisionType.Partial)
                return CollisionType.Partial;

            return CollisionType.None;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override ICutter Clone()
        {
            return CloneHelper<BallCutter>();
        }
    }
}
