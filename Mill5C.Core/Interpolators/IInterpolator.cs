using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Cutters;

namespace Mill5C.Core.Interpolators
{
    /// <summary>
    /// Base class for creating interpolators.
    /// </summary>
    public abstract class IInterpolator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Occurs when the interpolator reaches a point on the path.
        /// </summary>
        public event InterpolatorEventHandler PathPointReached;

        /// <summary>
        /// Gets the path. Set internally by the engine.
        /// </summary>
        /// <value>The path.</value>
        public Path.Path Path { get; internal set; }

        /// <summary>
        /// Gets the cutter managed by the interpolator. Set internally by the engine.
        /// </summary>
        /// <value>The cutter.</value>
        public ICutter Cutter { get; internal set; }

        /// <summary>
        /// Makes a step forward and updates cutter's position and orientation.
        /// </summary>
        public abstract void Step();

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Clones this instance. Used for creating multiple-task strategies.
        /// </summary>
        /// <returns></returns>
        public abstract IInterpolator Clone();

        /// <summary>
        /// Gets value indicating whether this instance has more steps.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has more steps; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasMoreSteps { get; protected set; }

        /// <summary>
        /// Raises the PathPointReached event.
        /// </summary>
        /// <param name="pointIndex">Index of the point.</param>
        protected void PathPointReachedRaise(int pointIndex)
        {
            if (PathPointReached != null)
                PathPointReached(this, new InterpolatorEventArgs { Path = this.Path, PointIndex = pointIndex });

            if (log.IsDebugEnabled)
                log.Debug("Cutter reached point " + pointIndex.ToString() 
                    + "/" + Path.Count + "; " + Path[pointIndex].Position.ToString());
        }

    }

    /// <summary>
    /// Delegate declaration for interpolator-related events
    /// </summary>
    public delegate void InterpolatorEventHandler(object sender, InterpolatorEventArgs args);

    /// <summary>
    /// EventArgs declaration for interpolator-related events
    /// </summary>
    public class InterpolatorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the path on which the event occured.
        /// </summary>
        /// <value>The path.</value>
        public Path.Path Path { get; set; }

        /// <summary>
        /// Gets or sets the index of the point.
        /// </summary>
        /// <value>The index of the point.</value>
        public int PointIndex { get; set; }
    }
}
