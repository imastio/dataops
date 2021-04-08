using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The query execution interface
    /// </summary>
    public interface IQueryExecutor : IOperationExecutor<IQueryExecutor>
    {
        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> ExecuteAsync<TResult>(object param = null);
    }
}