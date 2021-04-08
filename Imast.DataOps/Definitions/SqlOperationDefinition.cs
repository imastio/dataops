using System;

namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// A definition of Sql Operation
    /// </summary>
    public class SqlOperationDefinition : ITransactionOptions, IProviderOptions
    {
        /// <summary>
        /// The name of command
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The definition of command
        /// </summary>
        public CommandDefinition Command { get; set; }

        /// <summary>
        /// The group of current operation
        /// </summary>
        public OperationGroup Group { get; set; }

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