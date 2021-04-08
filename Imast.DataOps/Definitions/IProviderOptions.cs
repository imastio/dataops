using System;

namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The provider options definition
    /// </summary>
    public interface IProviderOptions
    {
        /// <summary>
        /// The compatibility providers
        /// </summary>
        CompatibilityProviders? Providers { get; set; }

        /// <summary>
        /// The timeout of operation
        /// </summary>
        TimeSpan? Timeout { get; set; }
    }
}