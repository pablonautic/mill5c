using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Core.Utility
{
    /// <summary>
    /// Framework exception class.
    /// </summary>
    public class Mill5CException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mill5CException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Mill5CException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mill5CException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public Mill5CException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
