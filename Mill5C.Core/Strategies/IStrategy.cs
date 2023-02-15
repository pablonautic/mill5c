using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Cutters;
using Mill5C.Core.Materials;

namespace Mill5C.Core.Strategies
{
    /// <summary>
    /// Interface for creating simulation strategies.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Returns the instance of a currently used cutter.
        /// </summary>
        /// <value>The reference cutter.</value>
        ICutter ReferenceCutter { get; }

        /// <summary>
        /// This event should be risen when a particular path is completed. 
        /// It is used internally be the Engine and should not be handled.
        /// For external event handling use Engine PathCompleted event.
        /// </summary>
        event EventHandler PathCompleted;

        /// <summary>
        /// Init this strategy using the given material. Called by the engine.
        /// </summary>
        /// <param name="material">The material.</param>
        void Init(IMaterial material);

        /// <summary>
        /// Use this method to log detailed parameters of the strategy. Called by the engine.
        /// </summary>
        void LogDetails();

        /// <summary>
        /// This method is called by the Engine prior to processing a path. 
        /// After this call ReferenceCutter property should return the referenceCutter parameter.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="referenceCutter">The reference cutter.</param>
        void PreparePath(Path.Path path, ICutter referenceCutter);

        /// <summary>
        /// Begins the path processing.
        /// </summary>
        void BeginProcessingPath();

        /// <summary>
        /// Gets a value indicating whether cancelation is pending.
        /// </summary>
        /// <value><c>true</c> if [cancel pending]; otherwise, <c>false</c>.</value>
        bool CancelPending { get; }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Resets this instance to the initial state.
        /// </summary>
        void Reset();
    }

}
