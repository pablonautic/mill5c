using System;
namespace Mill5C.Core.Cutters
{
    /// <summary>
    /// Interfaces for factories of cutter
    /// </summary>
    public interface ICutterFactory
    {
        /// <summary>
        /// Creates a cutter instance based on the name of the path file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        ICutter CreateCutter(string filename);
    }
}
