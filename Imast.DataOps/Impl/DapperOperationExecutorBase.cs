using System.Data;
using Imast.DataOps.Api;

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
    }
}