using System;
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
        Task<IEnumerable<TResult>> ExecuteAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> map, string splitOn = "id", object param = null);
    }
}