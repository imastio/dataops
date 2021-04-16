using System.Data;
using Imast.DataOps.Impl;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The Data Operations access point
    /// </summary>
    public class DataOperations
    {
        /// <summary>
        /// The operation registry
        /// </summary>
        protected readonly IOperationRegistry operations;

        /// <summary>
        /// The connection suppliers registry
        /// </summary>
        protected readonly IConnectionRegistry connections;

        /// <summary>
        /// The target provider if specified
        /// </summary>
        protected readonly SqlProvider? defaultProvider;

        /// <summary>
        /// Creates new instance of Data Operations Entry Point
        /// </summary>
        /// <param name="operations">The registry of operations</param>
        /// <param name="connections">The registry of connection suppliers</param>
        /// <param name="defaultProvider">The specific provider to use by default</param>
        public DataOperations(IOperationRegistry operations, IConnectionRegistry connections = null, SqlProvider? defaultProvider = null)
        {
            this.operations = operations;
            this.connections = connections;
            this.defaultProvider = defaultProvider;
        }

        /// <summary>
        /// Initialize globally when appropriate
        /// </summary>
        static DataOperations()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// Connect to a certain provider with given connection
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <param name="connection">The actual connection instance</param>
        /// <returns></returns>
        public IOperationRouter Connect(SqlProvider provider, IDbConnection connection)
        {
            return new OperationRouter(provider, connection, this.operations);
        }

        /// <summary>
        /// Connect to a certain provider with given connection
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public IOperationRouter Connect(SqlProvider provider)
        {
            // try get supplier for provider connection
            var supplier = this.connections?.GetOrNull(provider);

            // check if supplier is there
            if (supplier == null)
            {
                throw new IncorrectOperationException($"The connection supplier for {provider} is requested but missing.");
            }

            return this.Connect(provider, supplier.Invoke());
        }

        /// <summary>
        /// Connect to a default provider by supplier registry
        /// </summary>
        /// <returns></returns>
        public IOperationRouter Connect()
        {
            // check if default provider is set
            if (this.defaultProvider == null)
            {
                throw new IncorrectOperationException("The default provider is not set.");
            }

            return this.Connect(this.defaultProvider.Value);
        }
    }
}