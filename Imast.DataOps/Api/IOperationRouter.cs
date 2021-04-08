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
        /// Initiate the non-query operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        INonQueryExecutor NonQuery(OpKey key);

        /// <summary>
        /// Initiate the multi-result query
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        IMultiQueryExecutor MultiQuery(OpKey key);

        /// <summary>
        /// Initiate the scalar operation
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <returns></returns>
        IScalarExecutor Scalar(OpKey key);
    }
}