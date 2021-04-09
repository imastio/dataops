using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Imast.DataOps.Api;
using Imast.DataOps.Definitions;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// A dapper-based implementation of query executor
    /// </summary>
    public class DapperQueryExecutor : DapperOperationExecutorBase<IQueryExecutor>, IQueryExecutor
    {
        /// <summary>
        /// Dapper-based query executor
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="provider">The operation provider</param>
        /// <param name="operation">The operation to execute</param>
        public DapperQueryExecutor(IDbConnection connection, SqlProvider provider, SqlOperation operation) : base(connection, provider, operation)
        {
        }

        /// <summary>
        /// Gets the reference of current object for chaining
        /// </summary>
        /// <returns></returns>
        protected override IQueryExecutor GetThis()
        {
            return this;
        }

        /// <summary>
        /// Gets the effective timeout value
        /// </summary>
        /// <returns></returns>
        protected int? GetEffectiveTimeout()
        {
            // the timeout to use
            return this.Timeout.HasValue ? (int)this.Timeout.Value.TotalMilliseconds : default(int?);
        }

        /// <summary>
        /// Gets the effective source value
        /// </summary>
        /// <returns></returns>
        protected string GetEffectiveSource()
        {
            // the source of query
            return this.Operation.Source?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective command type
        /// </summary>
        /// <returns></returns>
        protected CommandType GetEffectiveCommandType()
        {
            // use type based on value
            return this.Operation.Type == OperationType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
        }

        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        public Task<IEnumerable<TResult>> ExecuteAsync<TResult>(object param = null)
        {
            return this.MaybeTransactionalAsync(transaction => this.Connection.QueryAsync<TResult>(
                this.GetEffectiveSource(), 
                param, 
                transaction,
                this.GetEffectiveTimeout(),
                this.GetEffectiveCommandType()));
        }

        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TFirst">The first type</typeparam>
        /// <typeparam name="TSecond">The second type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="map">The mapping function for given set</param>
        /// <param name="splitOn">The set should split on the given value</param>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        public Task<IEnumerable<TResult>> ExecuteAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> map, string splitOn = "id", object param = null)
        {
            return this.MaybeTransactionalAsync(transaction => 
                this.Connection.QueryAsync(
                    this.GetEffectiveSource(),
                    map,
                    param,
                    transaction,
                    this.Buffered,
                    splitOn,
                    this.GetEffectiveTimeout(),
                    this.GetEffectiveCommandType())
                );
        }

        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        public async Task<TResult> ExecuteFirstOrDefaultAsync<TResult>(object param = null)
        {
            return await this.MaybeTransactionalAsync(transaction => this.Connection.QueryFirstOrDefaultAsync<TResult>(
                this.GetEffectiveSource(),
                param,
                transaction,
                this.GetEffectiveTimeout(),
                this.GetEffectiveCommandType()));
        }
    }
}