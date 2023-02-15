using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Interpolators;
using Mill5C.Core.Materials;
using Mill5C.Core.Utility;

namespace Mill5C.Core.Strategies
{
    /// <summary>
    /// A single-threaded material-independent strategy implementation. 
    /// </summary>
    public class BasicSingleThreadedStrategy : IStrategy
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This event should be risen when a particular path is completed.
        /// It is used internally be the Engine and should not be handled.
        /// For external event handling use Engine PathCompleted event.
        /// </summary>
        public event EventHandler PathCompleted;

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>The material.</value>
        public IMaterial Material { get; private set; }

        /// <summary>
        /// Gets or sets the interpolator.
        /// </summary>
        /// <value>The interpolator.</value>
        public IInterpolator Interpolator { get; private set; }

        /// <summary>
        /// Gets a value indicating whether cancelation is pending.
        /// </summary>
        /// <value><c>true</c> if [cancel pending]; otherwise, <c>false</c>.</value>
        public bool CancelPending { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSingleThreadedStrategy"/> class.
        /// </summary>
        /// <param name="interpolator">The interpolator.</param>
        public BasicSingleThreadedStrategy(IInterpolator interpolator)
        {
            Interpolator = interpolator;
        }

        /// <summary>
        /// Init this strategy using the given material. Called by the engine.
        /// </summary>
        /// <param name="material">The material.</param>
        public virtual void Init(Mill5C.Core.Materials.IMaterial material)
        {
            Material = material;
        }

        /// <summary>
        /// Prepares the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="cutter">The cutter.</param>
        public virtual void PreparePath(Mill5C.Core.Path.Path path, Cutters.ICutter cutter)
        {    
            Interpolator.Cutter = ReferenceCutter = cutter;
            Interpolator.Path = path;
            Interpolator.Reset();
        }

        /// <summary>
        /// Use this method to log detailed parameters of the strategy. Called by the engine.
        /// </summary>
        public virtual void LogDetails()
        {
            if (log.IsInfoEnabled)
                log.Info("Using interpolator of type: " + Interpolator.GetType().Name);
        }

        /// <summary>
        /// Begins the path processing.
        /// </summary>
        public void BeginProcessingPath()
        {
            new Action(Run).BeginInvoke(null, null);
            //Run();
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        protected void Run()
        {
            while (Interpolator.HasMoreSteps)
            {
                if (CancelPending)
                {
                    CancelPending = false;
                    return;
                }

                Interpolator.Step();
                Material.Intersect(ReferenceCutter);
            }

            if (PathCompleted != null)
                PathCompleted(this, EventArgs.Empty);
        }

        /// <summary>
        /// Returns the instance of a currently used cutter.
        /// </summary>
        /// <value>The reference cutter.</value>
        public Mill5C.Core.Cutters.ICutter ReferenceCutter
        {
            get;
            set;
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            CancelPending = true;
        }

        /// <summary>
        /// Resets this instance to the initial state.
        /// </summary>
        public void Reset()
        {
            Interpolator.Reset();
        }

    }
}
