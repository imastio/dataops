using System.Data;
using Imast.DataOps.Api;
using Imast.DataOps.Definitions;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// The dapper-based operation executor base
    /// </summary>
    /// <typeparam name="TExecutor">The executor base</typeparam>
    public abstract class DapperOperationExecutorBase<TExecutor> : DapperExecutorBase<TExecutor>
    {
        /// <summary>
        /// The operation to execute
        /// </summary>
        public SqlOperation Operation { get; }

        /// <summary>
        /// Creates new instance of dapper-based operation executor base
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="provider">The provider</param>
        /// <param name="operation">The operation to execute</param>
        protected DapperOperationExecutorBase(IDbConnection connection, SqlProvider provider, SqlOperation operation) :
            base(connection, provider, operation.AutoTransaction, operation.Timeout)
        {
            this.Operation = operation;
        }

        /// <summary>
        /// Gets the effective timeout value
        /// </summary>
        /// <returns></returns>
        protected virtual int? GetEffectiveTimeout()
        {
            // the timeout to use
            return this.Timeout.HasValue ? (int)this.Timeout.Value.TotalMilliseconds : default(int?);
        }

        /// <summary>
        /// Gets the effective source value
        /// </summary>
        /// <returns></returns>
        protected virtual string GetEffectiveSource()
        {
            // the source of query
            return this.Operation.Source?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets the effective command type
        /// </summary>
        /// <returns></returns>
        protected virtual CommandType GetEffectiveCommandType()
        {
            // use type based on value
            return this.Operation.Type == OperationType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
        }
    }
}