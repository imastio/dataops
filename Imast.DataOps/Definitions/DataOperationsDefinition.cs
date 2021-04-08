using System.Collections.Generic;

namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The definition of data operations
    /// </summary>
    public class DataOperationsDefinition
    {
        /// <summary>
        /// The scoped configuration of operations
        /// </summary>
        public DataConfiguration Configuration { get; set; }

        /// <summary>
        /// The operation groups
        /// </summary>
        public List<OperationGroup> Groups { get; set; }
    }
}