using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Core.Materials
{
    /// <summary>
    /// Interface for defining materials.
    /// </summary>
    public interface IMaterial
    {
        /// <summary>
        /// Intersects this instance with a specified cutter instance.
        /// </summary>
        /// <param name="cutter">The cutter.</param>
        void Intersect(Cutters.ICutter cutter);

        /// <summary>
        /// Use this method to log detailed parameters of the material. Called by the engine.
        /// </summary>
        void LogDetails();

        /// <summary>
        /// Use this method to log summary information. Called by the engine.
        /// </summary>
        void LogSummary();

        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();

        /// <summary>
        /// Writes this instance to the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void Save(string filename);

        /// <summary>
        /// Writes this instance to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        void Save(System.IO.Stream stream);

        /// <summary>
        /// Loads data from a file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void Load(string filename);

        /// <summary>
        /// Loads data from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        void Load(System.IO.Stream stream);
    }
}
