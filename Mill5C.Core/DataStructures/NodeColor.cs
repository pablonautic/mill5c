using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Core.DataStructures
{
    /// <summary>
    /// Represents a color of an octree node
    /// </summary>
    public enum NodeColor
    {

        /// <summary>
        /// This node is entirely cut.
        /// </summary>
        White,

        /// <summary>
        /// This node is untouched.
        /// </summary>
        Black,

        /// <summary>
        /// This node is partially cut.
        /// </summary>
        Gray
    }
}
