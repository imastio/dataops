using System;
using System.Threading.Tasks;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The multi query executor interface
    /// </summary>
    public interface IMultiQueryExecutor : IOperationExecutor<IMultiQueryExecutor>
    {
        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <param name="resultHandler">The result handler function</param>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(Func<IMultiResult, Task<TResult>> resultHandler, object param = null);
    }
}