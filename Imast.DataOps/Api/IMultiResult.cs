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
    }
}