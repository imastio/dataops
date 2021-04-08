using System.Data;
using Imast.DataOps.Api;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// The operation router module
    /// </summary>
    public class OperationRouter : IOperationRouter
    {
        /// <summary>
        /// The operation router
        /// </summary>
        protected readonly IOperationRegistry registry;

        /// <summary>
        /// The actual database connection
        /// </summary>
        protected readonly IDbConnection connection;

        /// <summary>
        /// The target Sql Provider
        /// </summary>
        protected readonly SqlProvider provider;

        /// <summary>
        /// Creates new instance of operation router
        /// </summary>
        /// <param name="provider">The Sql Provider type</param>
        /// <param name="connection">The actual database connection</param>
        /// <param name="registry">The registry of operations</param>
        public OperationRouter(SqlProvider provider, IDbConnection connection, IOperationRegistry registry)
        {
            this.provider = provider;
            this.connection = connection;
            this.registry = registry;
        }

        /// <summary>
        /// Initiate the query operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        public IQueryExecutor Query(OpKey key)
        {
            // get valid operation if exists
            var operation = this.GetValidOrThrow(key, ResultType.Table);

            // build query executor
            return new DapperQueryExecutor(this.connection, this.provider, operation);
        }

        /// <summary>
        /// Initiate the non-query operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        public INonQueryExecutor NonQuery(OpKey key)
        {
            // get valid operation if exists
            var operation = this.GetValidOrThrow(key, ResultType.RowCount);

            // build non-query executor
            return new DapperNonQueryExecutor(this.connection, this.provider, operation);
        }
        
        /// <summary>
        /// Initiate the multi-result query
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        public IMultiQueryExecutor MultiQuery(OpKey key)
        {
            // get valid operation if exists
            var operation = this.GetValidOrThrow(key, ResultType.MultipleTables);

            // build non-query executor
            return new DapperMultiQueryExecutor(this.connection, this.provider, operation);
        }

        /// <summary>
        /// Initiate the scalar operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        public IScalarExecutor Scalar(OpKey key)
        {
            // get valid operation if exists
            var operation = this.GetValidOrThrow(key, ResultType.Scalar);

            // build scalar executor
            return new DapperScalarExecutor(this.connection, this.provider, operation);
        }

        /// <summary>
        /// Try get operation or throw if not found or not supported
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <param name="resultType">The expected result type</param>
        /// <returns></returns>
        protected SqlOperation GetValidOrThrow(OpKey key, ResultType resultType)
        {
            // require operation to be defined
            if (!this.registry.IsDefined(key))
            {
                throw new IncorrectOperationException($"The operation {key.Group}/{key.Name} is not defined in the registry.");
            }

            // try get operation 
            var operation = this.registry.GetOrNull(key, this.provider);

            // check if operation for specific provider does not exist
            if (operation == null)
            {
                throw new IncorrectOperationException($"The operation {key.Group}/{key.Name} is not supported for provider {this.provider}.");
            }

            // require the operation to match expected result type (tolerate unknown result type)
            if (operation.ResultType != resultType || operation.ResultType != ResultType.Unknown)
            {
                throw new IncorrectOperationException($"The operation {key.Group}/{key.Name} is defined to result {operation.ResultType} but result {resultType} is required in this context.");
            }

            return operation;
        }
    }
}