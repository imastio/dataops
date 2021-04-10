namespace Imast.DataOps.Api
{
    /// <summary>
    /// The interface to route to operations
    /// </summary>
    public interface IOperationRouter
    {
        /// <summary>
        /// Initiate the query operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        IQueryExecutor Query(OpKey key);

        /// <summary>
        /// Initiate the query operation
        /// </summary>
        /// <param name="group">The operation group name</param>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        IQueryExecutor Query(string group, string operation)
        {
            return this.Query(OpKey.Of(group, operation));
        }

        /// <summary>
        /// Initiate the query operation
        /// </summary>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        IQueryExecutor Query(string operation)
        {
            return this.Query(OpKey.Of(string.Empty, operation));
        }

        /// <summary>
        /// Initiate the query first operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        IQueryFirstExecutor QueryFirst(OpKey key);

        /// <summary>
        /// Initiate the query first operation
        /// </summary>
        /// <param name="group">The operation group name</param>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        IQueryFirstExecutor QueryFirst(string group, string operation)
        {
            return this.QueryFirst(OpKey.Of(group, operation));
        }

        /// <summary>
        /// Initiate the query first operation
        /// </summary>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        IQueryFirstExecutor QueryFirst(string operation)
        {
            return this.QueryFirst(OpKey.Of(string.Empty, operation));
        }

        /// <summary>
        /// Initiate the non-query operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        INonQueryExecutor NonQuery(OpKey key);

        /// <summary>
        /// Initiate the non-query operation
        /// </summary>
        /// <param name="group">The group of operation</param>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        INonQueryExecutor NonQuery(string group, string operation)
        {
            return this.NonQuery(OpKey.Of(group, operation));
        }

        /// <summary>
        /// Initiate the non-query operation
        /// </summary>
        /// <param name="operation">The name of operation</param>
        /// <returns></returns>
        INonQueryExecutor NonQuery(string operation)
        {
            return this.NonQuery(OpKey.Of(string.Empty, operation));
        }

        /// <summary>
        /// Initiate the multi-result query
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        IMultiQueryExecutor MultiQuery(OpKey key);

        /// <summary>
        /// Initiate the multi-result query
        /// </summary>
        /// <param name="group">The group of operation</param>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        IMultiQueryExecutor MultiQuery(string group, string operation)
        {
            return this.MultiQuery(OpKey.Of(group, operation));
        }

        /// <summary>
        /// Initiate the multi-result query
        /// </summary>
        /// <param name="operation">The name of operation</param>
        /// <returns></returns>
        IMultiQueryExecutor MultiQuery(string operation)
        {
            return this.MultiQuery(OpKey.Of(string.Empty, operation));
        }
        
        /// <summary>
        /// Initiate the scalar operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        IScalarExecutor Scalar(OpKey key);

        /// <summary>
        /// Initiate the scalar operation
        /// </summary>
        /// <param name="group">The group of operation</param>
        /// <param name="operation">The operation name</param>
        /// <returns></returns>
        IScalarExecutor Scalar(string group, string operation)
        {
            return this.Scalar(OpKey.Of(group, operation));
        }

        /// <summary>
        /// Initiate the scalar operation
        /// </summary>
        /// <param name="operation">The name of operation</param>
        /// <returns></returns>
        IScalarExecutor Scalar(string operation)
        {
            return this.Scalar(OpKey.Of(string.Empty, operation));
        }
    }
}