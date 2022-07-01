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
        /// The default data source name
        /// </summary>
        protected const string DEFAULT_DATA_SOURCE = "";

        /// <summary>
        /// The storage of registered suppliers
        /// </summary>
        protected readonly Dictionary<Tuple<string, SqlProvider>, Func<IDbConnection>> suppliers;

        /// <summary>
        /// Creates new instance of connection supplier registry
        /// </summary>
        public ConnectionSupplierRegistry()
        {
            this.suppliers = new Dictionary<Tuple<string, SqlProvider>, Func<IDbConnection>>();
        }

        /// <summary>
        /// Registers the connection supplier for the given provider
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <param name="supplier">The connection supplier</param>
        public void Register(SqlProvider provider, Func<IDbConnection> supplier)
        {
            this.suppliers[Tuple.Create(DEFAULT_DATA_SOURCE, provider)] = supplier;
        }

        /// <summary>
        /// Registers the connection supplier for the given provider
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <param name="supplier">The connection supplier</param>
        public void Register(string dataSource, SqlProvider provider, Func<IDbConnection> supplier)
        {
            this.suppliers[Tuple.Create(dataSource ?? DEFAULT_DATA_SOURCE, provider)] = supplier;
        }

        /// <summary>
        /// Gets the supplier for the provider or null
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public Func<IDbConnection> GetOrNull(SqlProvider provider)
        {
            return this.suppliers.TryGetValue(Tuple.Create(DEFAULT_DATA_SOURCE, provider), out var supplier) ? supplier : null;
        }

        /// <summary>
        /// Gets the supplier for the provider or null
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public Func<IDbConnection> GetOrNull(string dataSource, SqlProvider provider)
        {
            return this.suppliers.TryGetValue(Tuple.Create(dataSource ?? DEFAULT_DATA_SOURCE, provider), out var supplier) ? supplier : null;

        }
    }
}