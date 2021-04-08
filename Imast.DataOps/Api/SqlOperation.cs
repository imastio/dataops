using System;
using Imast.DataOps.Definitions;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The operation definition to execute
    /// </summary>
    public class SqlOperation
    {
        /// <summary>
        /// The name of operation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The group of operation
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The type of operation
        /// </summary>
        public OperationType Type { get; set; }

        /// <summary>
        /// The source of command 
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// The type of result operation expects
        /// </summary>
        public ResultType ResultType { get; set; }

        /// <summary>
        /// The transactional behavior of operation
        /// </summary>
        public AutoTransaction? AutoTransaction { get; set; }

        /// <summary>
        /// The timeout of operation
        /// </summary>
        public TimeSpan? Timeout { get; set; }
    }
}