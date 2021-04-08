using System;
using System.Collections.Generic;
using System.Data;
using Imast.DataOps.Api;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// The connection supplier registry
    /// </summary>
    public class ConnectionSupplierRegistry : IConnectionRegistry
    {
        /// <summary>
        /// The storage of registered suppliers
        /// </summary>
        protected readonly Dictionary<SqlProvider, Func<IDbConnection>> suppliers;

        /// <summary>
        /// Creates new instance of connection supplier registry
        /// </summary>
        public ConnectionSupplierRegistry()
        {
            this.suppliers = new Dictionary<SqlProvider, Func<IDbConnection>>();
        }

        /// <summary>
        /// Registers the connection supplier for the given provider
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <param name="supplier">The connection supplier</param>
        public void Register(SqlProvider provider, Func<IDbConnection> supplier)
        {
            this.suppliers[provider] = supplier;
        }

        /// <summary>
        /// Gets the supplier for the provider or null
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public Func<IDbConnection> GetOrNull(SqlProvider provider)
        {
            return this.suppliers.TryGetValue(provider, out var supplier) ? supplier : null;
        }
    }
}