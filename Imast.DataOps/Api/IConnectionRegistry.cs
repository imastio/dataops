using System;
using System.Data;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The connection suppliers registry
    /// </summary>
    public interface IConnectionRegistry
    {
        /// <summary>
        /// Registers the connection supplier for the given provider
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <param name="supplier">The connection supplier</param>
        void Register(SqlProvider provider, Func<IDbConnection> supplier);

        /// <summary>
        /// Registers the connection supplier for the given provider
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <param name="supplier">The connection supplier</param>
        void Register(string dataSource, SqlProvider provider, Func<IDbConnection> supplier);

        /// <summary>
        /// Gets the supplier for the provider or null
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        Func<IDbConnection> GetOrNull(SqlProvider provider);

        /// <summary>
        /// Gets the supplier for the provider or null
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        Func<IDbConnection> GetOrNull(string dataSource, SqlProvider provider);
    }
}