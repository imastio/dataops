using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The Multiple Results reader interface
    /// </summary>
    public interface IMultiResult : IDisposable
    {
        /// <summary>
        /// Reads next available result from the reader
        /// </summary>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TResult>> ReadAsync<TResult>();

        /// <summary>
        /// Reads the next result set
        /// </summary>
        /// <typeparam name="TFirst">The first type</typeparam>
        /// <typeparam name="TSecond">The second type</typeparam>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TResult>> ReadAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> map, string splitOn = "id");

        /// <summary>
        /// Reads the first entity from result set or default if nothing found
        /// </summary>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        Task<TResult> ReadFirstOrDefaultAsync<TResult>();
    }
}