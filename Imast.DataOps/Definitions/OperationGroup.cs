using System;
using System.Collections.Generic;

namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// A definition of Sql Operation
    /// </summary>
    public class OperationGroup : ITransactionOptions, IProviderOptions
    {
        /// <summary>
        /// The name of group
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// The set of operations in the group
        /// </summary>
        public List<SqlOperationDefinition> Operations { get; set; }

        /// <summary>
        /// The transactional mode
        /// </summary>
        public AutoTransactionMode? AutoTransaction { get; set; }

        /// <summary>
        /// The compatibility providers
        /// </summary>
        public CompatibilityProviders? Providers { get; set; }

        /// <summary>
        /// The timeout of operation
        /// </summary>
        public TimeSpan? Timeout { get; set; }
    }
}