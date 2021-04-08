using System;

namespace Imast.DataOps.Definitions
{
    public class DataConfiguration : ITransactionOptions, IProviderOptions
    {
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