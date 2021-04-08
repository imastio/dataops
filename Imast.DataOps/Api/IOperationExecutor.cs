using System;
using System.Data;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The query executor interface
    /// </summary>
    public interface IOperationExecutor<out TExecutor>
    {
        /// <summary>
        /// The query executor connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// The Sql Provider
        /// </summary>
        public SqlProvider Provider { get; }

        /// <summary>
        /// The given transaction to use
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// The option to auto-commit transaction within the operation
        /// </summary>
        public bool AutoCommit { get; }

        /// <summary>
        /// The auto-transaction to use
        /// </summary>
        public AutoTransaction? AutoTransaction { get; }

        /// <summary>
        /// Use the given transaction for the operation
        /// </summary>
        /// <param name="transaction">The transaction to use</param>
        /// <param name="autocommit">Auto-commit the transaction within operation</param>
        /// <returns></returns>
        TExecutor WithTransaction(IDbTransaction transaction, bool autocommit = false);

        /// <summary>
        /// Use the given auto-transaction for the operation
        /// </summary>
        /// <param name="transaction">The transaction to use</param>
        /// <returns></returns>
        TExecutor WithTransaction(AutoTransaction? transaction);

        /// <summary>
        /// Use the given timeout for the command
        /// </summary>
        /// <param name="timeout">The timeout to use</param>
        /// <returns></returns>
        TExecutor WithTimeout(TimeSpan? timeout);
    }
}