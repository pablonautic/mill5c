using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Geometry;
using Mill5C.Core.Materials;
using Mill5C.Core.Cutters;

namespace Mill5C.Core.Strategies
{
    /// <summary>
    /// Represents a base class for strategies where the cutter is driven by the user.
    /// </summary>
    public class BaseManualStrategy : IStrategy
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
        /// Returns the instance of a currently used cutter.
        /// </summary>
        /// <value>The reference cutter.</value>
        public ICutter ReferenceCutter { get; set; }

        private IMaterial material;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseManualStrategy"/> class.
        /// </summary>
        public BaseManualStrategy()
        {
        }

        /// <summary>
        /// Init this strategy using the given material. Called by the engine.
        /// </summary>
        /// <param name="material">The material.</param>
        public void Init(IMaterial material)
        {
            this.material = material;
        }


        /// <summary>
        /// This method is called by the Engine prior to processing a path.
        /// After this call ReferenceCutter property should return the referenceCutter parameter.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="referenceCutter">The reference cutter.</param>
        public void PreparePath(Mill5C.Core.Path.Path path, ICutter referenceCutter)
        {
            ReferenceCutter = referenceCutter;
            ReferenceCutter.Position = path[0].Position;
            ReferenceCutter.Orientation = path[0].Orientation;
        }

        /// <summary>
        /// Begins the path processing.
        /// </summary>
        public void BeginProcessingPath()
        {
            
        }

        /// <summary>
        /// Translates the cutter by the given vector.
        /// </summary>
        /// <param name="translation">The translation.</param>
        public void TranslateCutter(Vector3D translation)
        {
            ReferenceCutter.Position += translation;
            Update();
        }

        /// <summary>
        /// Use this method to log detailed parameters of the strategy. Called by the engine.
        /// </summary>
        public void LogDetails()
        {
            
        }

        /// <summary>
        /// Set a new orientation for the cutter.
        /// </summary>
        /// <param name="newOrientation">The new orientation.</param>
        public void RotateCutter(Vector3D newOrientation)
        {
            ReferenceCutter.Orientation = newOrientation;
        }

        private void Update()
        {
            material.Intersect(ReferenceCutter);
            if (log.IsDebugEnabled)
                log.Debug("Cutter reached point " + ReferenceCutter.Position.ToString());
        }

        /// <summary>
        /// Raised the PathCompleted event.
        /// </summary>
        public void EndFile()
        {
            if (PathCompleted != null)
                PathCompleted(this, EventArgs.Empty);
        }

        /// <summary>
        /// Resets this instance to the initial state.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Gets a value indicating whether cancelation is pending.
        /// </summary>
        /// <value><c>true</c> if [cancel pending]; otherwise, <c>false</c>.</value>
        public bool CancelPending { get; private set; }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
        }

    }
}
