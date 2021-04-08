using System;

namespace Imast.DataOps.Init
{
    /// <summary>
    /// The operation parsing exception
    /// </summary>
    public class OperationParsingException : Exception
    {
        /// <summary>
        /// Creates new instance of parsing exception
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="inner">The inner exception</param>
        public OperationParsingException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}