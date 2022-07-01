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
        /// The target data source if specified
        /// </summary>
        protected readonly string dataSource;

        /// <summary>
        /// The target provider if specified
        /// </summary>
        protected readonly SqlProvider provider;

        /// <summary>
        /// Creates new instance of Data Operations Entry Point
        /// </summary>
        /// <param name="operations">The registry of operations</param>
        /// <param name="connections">The registry of connection suppliers</param>
        /// <param name="dataSource">The data source by default</param>
        /// <param name="provider">The specific provider to use by default</param>
        public DataOperations(IOperationRegistry operations, IConnectionRegistry connections, string dataSource, SqlProvider provider)
        {
            this.operations = operations;
            this.connections = connections;
            this.dataSource = dataSource;
            this.provider = provider; 
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
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <param name="connection">The actual connection instance</param>
        /// <returns></returns>
        public IOperationRouter Connect(string dataSource, SqlProvider provider, IDbConnection connection)
        {
            return new OperationRouter(dataSource, provider, connection, this.operations);
        }

        /// <summary>
        /// Connect to a certain provider with given connection
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <param name="connection">The actual connection instance</param>
        /// <returns></returns>
        public IOperationRouter Connect(SqlProvider provider, IDbConnection connection)
        {
            return this.Connect(this.dataSource, provider, connection);
        }

        /// <summary>
        /// Connect to a certain provider with given connection
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public IOperationRouter Connect(string dataSource, SqlProvider provider)
        {
            // try get supplier for provider connection
            var supplier = this.connections?.GetOrNull(dataSource, provider);

            // check if supplier is there
            if (supplier == null)
            {
                throw new IncorrectOperationException($"The connection supplier for data source {dataSource} compatable with provider {provider} is requested but missing.");
            }

            return this.Connect(dataSource, provider, supplier.Invoke());
        }

        /// <summary>
        /// Connect to a certain provider with given connection
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public IOperationRouter Connect(SqlProvider provider)
        {
            return this.Connect(this.dataSource, provider);
        }

        /// <summary>
        /// Connect to a default provider by supplier registry
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <returns></returns>
        public IOperationRouter Connect(string dataSource)
        {
            return this.Connect(dataSource, this.provider);
        }

        /// <summary>
        /// Connect to a default provider by supplier registry
        /// </summary>
        /// <returns></returns>
        public IOperationRouter Connect()
        {
            return this.Connect(this.dataSource, this.provider);
        }

        /// <summary>
        /// Creates a new instance of Data Operations adapted for the data source
        /// </summary>
        /// <param name="dataSource">The data source</param>
        /// <returns></returns>
        public DataOperations WithDataSource(string dataSource)
        {
            return this.With(dataSource, this.provider);
        }

        /// <summary>
        /// Creates a new instance of Data Operations adapted for the provider
        /// </summary>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public DataOperations WithProvider(SqlProvider provider)
        {
            return this.With(this.dataSource, provider);
        }

        /// <summary>
        /// Creates a new instance of Data Operations adapted for the data source and provider
        /// </summary>
        /// <param name="dataSource">The data source</param>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public DataOperations With(string dataSource, SqlProvider provider)
        {
            return new DataOperations(this.operations, this.connections, dataSource, provider);
        }
    }
}