using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Geometry;
using Mill5C.Core.Utility;

namespace Mill5C.Core.Interpolators
{
    /// <summary>
    /// Linear-Cubic interpolator.
    /// </summary>
    public class LCInterpolator : IInterpolator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets the minimal distance interpolation step beetween two points.
        /// </summary>
        /// <value>The position eps.</value>
        public float PositionEps { get; set; }

        /// <summary>
        /// Gets or sets the minimal orientation interpolation step beetween two vectors in radians.
        /// </summary>
        /// <value>The orientation eps.</value>
        public float OrientationEps { get; set; }

        private int currentPointIndex;

        private float tPos, dtPos, tOrient, dtOrient;

        private bool posStopCond;

        /// <summary>
        /// Initializes a new instance of the <see cref="LCInterpolator"/> class.
        /// </summary>
        public LCInterpolator()
           : this(1.0f, 0.01f)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LCInterpolator"/> class.
        /// </summary>
        /// <param name="positionEps">The position eps.</param>
        /// <param name="orientationEps">The orientation eps.</param>
        public LCInterpolator(float positionEps, float orientationEps)
        {
            PositionEps = positionEps;
            OrientationEps = orientationEps;
            Reset(true);
        }

        /// <summary>
        /// Clones this instance. Used for creating multiple-task strategies.
        /// </summary>
        /// <returns></returns>
        public override IInterpolator Clone()
        {
            return new LCInterpolator(PositionEps, OrientationEps) { Path = Path };
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public override void Reset()
        {
            Reset(false);
        }

        private void Reset(bool init)
        {
            currentPointIndex = -1;
            tPos = tOrient = float.MaxValue;
            HasMoreSteps = true;

            if (!init)
            {
                Cutter.Position = Path[0].Position;
                Cutter.Orientation = Path[0].Orientation;
            }
        }

        /// <summary>
        /// Makes a step forward and updates cutter's position and orientation.
        /// </summary>
        public override void Step()
        {
            if (!HasMoreSteps)
                throw new InvalidOperationException("interpolator has reached the end of the path, " +
                    "use Reset() to start again");

            tPos += dtPos;
            tOrient += dtOrient;

            if (posStopCond && tPos > 1.0f || !posStopCond && tOrient > 1.0f)
                NextPoint();

            if (HasMoreSteps)
            {
                Cutter.Position = Point3D.Lerp(Path[currentPointIndex].Position,
                    Path[currentPointIndex + 1].Position, tPos);

                Cutter.Orientation = Vector3D.Cerp(Path[currentPointIndex].Orientation,
                    Path[currentPointIndex].Orientation, dtOrient);
            }
            
        }

        private void NextPoint()
        {
            currentPointIndex++;

            if (currentPointIndex + 1 == Path.Count)
            {
                HasMoreSteps = false;
                return;
            }

            tPos = 0;     
            float dst = Point3D.Dist(Path[currentPointIndex].Position, Path[currentPointIndex+1].Position);         
            float numStepsDist = (float)Math.Floor(dst / PositionEps);
            
            dtOrient = 0;
            float angle = (float)Math.Acos(MathHelper.Clamp(Vector3D.Dot(
                Path[currentPointIndex].Orientation, 
                Path[currentPointIndex + 1].Orientation), -1, 1));
            float numStepsAngle = (float)Math.Floor(angle / OrientationEps);

            float numSteps;

            if (numStepsDist > numStepsAngle)
            {
                numSteps = numStepsDist;
                posStopCond = true;
            }
            else
            {
                numSteps = numStepsAngle;
                posStopCond = false;
            }

            dtOrient = 1.0f / numSteps;
            dtPos = 1.0f / numSteps;

            PathPointReachedRaise(currentPointIndex);

         }

    }
}
