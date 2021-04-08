using System;
using System.Data;
using System.Threading.Tasks;
using Imast.DataOps.Api;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// Dapper-based executor base shell
    /// </summary>
    /// <typeparam name="TExecutor">The executor</typeparam>
    public abstract class DapperExecutorBase<TExecutor> : IOperationExecutor<TExecutor>
    {
        /// <summary>
        /// The connection to use
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// The Sql Provider
        /// </summary>
        public SqlProvider Provider { get; }

        /// <summary>
        /// The transaction to use
        /// </summary>
        public IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// The option to auto-commit transaction within the operation
        /// </summary>
        public bool AutoCommit { get; private set; }

        /// <summary>
        /// The auto-transaction to use
        /// </summary>
        public AutoTransaction? AutoTransaction { get; private set; }

        /// <summary>
        /// The timeout to use
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Dapper-based query executor
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="provider">The operation provider</param>
        /// <param name="autoTransaction">The auto-transaction setup reference</param>
        /// <param name="timeout">The desired timeout</param>
        protected DapperExecutorBase(IDbConnection connection, SqlProvider provider, AutoTransaction? autoTransaction, TimeSpan? timeout)
        {
            this.Connection = connection;
            this.Provider = provider;
            this.AutoTransaction = autoTransaction;
            this.Timeout = timeout;
            this.AutoCommit = autoTransaction.HasValue;
        }

        /// <summary>
        /// Gets this for chaining
        /// </summary>
        /// <returns></returns>
        protected abstract TExecutor GetThis();

        /// <summary>
        /// Wrap async operation into a transaction if required
        /// </summary>
        /// <typeparam name="TResult">The result type of entity</typeparam>
        /// <param name="function">The function to execute</param>
        /// <returns></returns>
        protected async Task<TResult> MaybeTransactionalAsync<TResult>(Func<IDbTransaction, Task<TResult>> function)
        {
            // connection was initially closed
            var wasClosed = this.Connection.State == ConnectionState.Closed;

            // use given transaction
            var transaction = this.Transaction;

            // if no transaction given but auto-transaction is setup
            if (transaction == null && this.AutoTransaction.HasValue)
            {
                // open connection to build a transaction
                if (wasClosed)
                {
                    this.Connection.Open();
                }

                transaction = this.Connection.BeginTransaction(MapIsolation(this.AutoTransaction.Value));
            }

            try
            {
                // execute given function within context
                var result = await function(transaction);

                // should this be committed 
                if (this.AutoCommit)
                {
                    // commit transaction if any
                    transaction?.Commit();
                }

                return result;
            }
            catch (Exception)
            {
                // if committing is enabled then rollback should be there also
                if (this.AutoCommit)
                {
                    // rollback transaction if any
                    transaction?.Rollback();
                }

                // rethrow
                throw;
            }
            finally
            {
                // if connection was closed then close it back
                if (wasClosed)
                {
                    this.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Use the given transaction for the operation
        /// </summary>
        /// <param name="transaction">The transaction to use</param>
        /// <param name="autocommit">Auto-commit the transaction within operation</param>
        /// <returns></returns>
        public TExecutor WithTransaction(IDbTransaction transaction, bool autocommit = false)
        {
            this.Transaction = transaction;
            this.AutoCommit = autocommit;
            this.AutoTransaction = null;
            return this.GetThis() ;
        }


        /// <summary>
        /// Use the given auto-transaction for the operation
        /// </summary>
        /// <param name="transaction">The transaction to use</param>
        /// <returns></returns>
        public TExecutor WithTransaction(AutoTransaction? transaction)
        {
            this.Transaction = null;
            this.AutoCommit = true;
            this.AutoTransaction = transaction;
            return this.GetThis();
        }

        /// <summary>
        /// Use the given timeout for the command
        /// </summary>
        /// <param name="timeout">The timeout to use</param>
        /// <returns></returns>
        public TExecutor WithTimeout(TimeSpan? timeout)
        {
            this.Timeout = timeout;
            return this.GetThis();
        }

        /// <summary>
        /// Maps isolation level to concrete one
        /// </summary>
        /// <param name="transaction">The isolation type</param>
        /// <returns></returns>
        protected static IsolationLevel MapIsolation(AutoTransaction transaction)
        {
            return transaction switch
            {
                Api.AutoTransaction.RepeatableRead => IsolationLevel.RepeatableRead,
                Api.AutoTransaction.ReadUncommitted => IsolationLevel.ReadUncommitted,
                Api.AutoTransaction.ReadCommitted => IsolationLevel.ReadCommitted,
                Api.AutoTransaction.Serializable => IsolationLevel.Serializable,
                _ => throw new InvalidOperationException($"Isolation level {transaction} is not supported")
            };
        }
    }
}