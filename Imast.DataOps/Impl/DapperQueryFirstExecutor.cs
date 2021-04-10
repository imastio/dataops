using System.Data;
using System.Threading.Tasks;
using Dapper;
using Imast.DataOps.Api;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// A dapper-based implementation of query first executor
    /// </summary>
    public class DapperQueryFirstExecutor : DapperOperationExecutorBase<IQueryFirstExecutor>, IQueryFirstExecutor
    {
        /// <summary>
        /// Dapper query first executor delegate
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="cnn">The connection</param>
        /// <param name="sql">The sql statement to execute</param>
        /// <param name="param">The parameter value</param>
        /// <param name="transaction">The transaction instance </param>
        /// <param name="commandTimeout">The effective timeout</param>
        /// <param name="commandType">The command type</param>
        /// <returns></returns>
        protected delegate Task<TResult> QueryFirstExecutor<TResult>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Dapper-based query executor
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="provider">The operation provider</param>
        /// <param name="operation">The operation to execute</param>
        public DapperQueryFirstExecutor(IDbConnection connection, SqlProvider provider, SqlOperation operation) : base(connection, provider, operation)
        {
        }

        /// <summary>
        /// Gets the reference of current object for chaining
        /// </summary>
        /// <returns></returns>
        protected override IQueryFirstExecutor GetThis()
        {
            return this;
        }
        
        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="param">The parameter if given</param>
        /// <param name="mode">The mode of query</param>
        /// <returns></returns>
        public Task<TResult> ExecuteAsync<TResult>(object param = null, QueryFirstMode mode = QueryFirstMode.FirstOrDefault)
        {
            // use the executor based on mode
            QueryFirstExecutor<TResult> executor = mode switch
            {
                QueryFirstMode.First => SqlMapper.QueryFirstAsync<TResult>,
                QueryFirstMode.Single => SqlMapper.QuerySingleAsync<TResult>,
                QueryFirstMode.SingleOrDefault => SqlMapper.QuerySingleOrDefaultAsync<TResult>,
                QueryFirstMode.FirstOrDefault => SqlMapper.QueryFirstOrDefaultAsync<TResult>,
                _ => SqlMapper.QueryFirstOrDefaultAsync<TResult>
            };

            return this.MaybeTransactionalAsync(transaction => executor(
                this.Connection,
                this.GetEffectiveSource(),
                param,
                transaction,
                this.GetEffectiveTimeout(),
                this.GetEffectiveCommandType()));
        }

    }
}