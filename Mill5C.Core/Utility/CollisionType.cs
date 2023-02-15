using System;

namespace Mill5C.Core.Utility
{
    /// <summary>
    /// Defines possible types of collision beetween objects.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// No collision.
        /// </summary>
        None,

        /// <summary>
        /// Objects intersect.
        /// </summary>
        Partial,

        /// <summary>
        /// One object inside the other
        /// </summary>
        Total
    }
}