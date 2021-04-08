using System;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The invalid operation exception
    /// </summary>
    public class IncorrectOperationException : Exception
    {
        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The cause exception</param>
        public IncorrectOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="message">The message</param>
        public IncorrectOperationException(string message) : base(message, null)
        {
        }
    }
}