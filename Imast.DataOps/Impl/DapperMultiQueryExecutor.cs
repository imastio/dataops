using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Imast.DataOps.Api;
using Imast.DataOps.Definitions;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// The dapper-based multi-query executor
    /// </summary>
    public class DapperMultiQueryExecutor : DapperOperationExecutorBase<IMultiQueryExecutor>, IMultiQueryExecutor
    {
        /// <summary>
        /// Dapper-based multi query executor
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="provider">The operation provider</param>
        /// <param name="operation">The operation to execute</param>
        public DapperMultiQueryExecutor(IDbConnection connection, SqlProvider provider, SqlOperation operation) : base(connection, provider, operation)
        {
        }

        /// <summary>
        /// Gets the reference of current object for chaining
        /// </summary>
        /// <returns></returns>
        protected override IMultiQueryExecutor GetThis()
        {
            return this;
        }

        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <param name="resultHandler">The result handler function</param>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        public async Task<TResult>ExecuteAsync<TResult>(Func<IMultiResult, Task<TResult>> resultHandler, object param = null)
        {
            // the timeout to use
            var timeout = this.Timeout.HasValue ? (int)this.Timeout.Value.TotalMilliseconds : default(int?);

            // the source of query
            var source = this.Operation.Source?.ToString() ?? string.Empty;

            // use type based on value
            var type = this.Operation.Type == OperationType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            return await this.MaybeTransactionalAsync(async transaction => 
            {
                // perform operation and wait the result
                var queryResult = await this.Connection.QueryMultipleAsync(source, param, transaction, timeout, type);

                // wrap result into a multi-result reader
                using var reader = (IMultiResult) new DapperMultiResult(queryResult, this.Buffered);
           
                // map and get final result
                return await resultHandler(reader);
            });
        }
    }
}