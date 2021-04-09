using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Imast.DataOps.Api;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// A dapper-based multi-result adapter
    /// </summary>
    public class DapperMultiResult : IMultiResult
    {
        /// <summary>
        /// The grid reader instance
        /// </summary>
        protected readonly SqlMapper.GridReader gridReader;

        /// <summary>
        /// Indicates if buffering is needed
        /// </summary>
        protected readonly bool buffered;

        /// <summary>
        /// Creates a multi result reader
        /// </summary>
        /// <param name="gridReader">The grid reader instance</param>
        /// <param name="buffered">The buffering indicator</param>
        public DapperMultiResult(SqlMapper.GridReader gridReader, bool buffered)
        {
            this.gridReader = gridReader;
            this.buffered = buffered;
        }

        /// <summary>
        /// Reads the next result set
        /// </summary>
        /// <typeparam name="TFirst">The first type</typeparam>
        /// <typeparam name="TSecond">The second type</typeparam>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        public Task<IEnumerable<TResult>> ReadAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> map, string splitOn = "id")
        {
            return Task.Run(() => this.gridReader.Read(map, splitOn, this.buffered));
        }

        /// <summary>
        /// Reads the next result set
        /// </summary>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        public Task<IEnumerable<TResult>> ReadAsync<TResult>()
        {
            return this.gridReader.ReadAsync<TResult>(this.buffered);
        }

        /// <summary>
        /// Reads the first entity from result set or default if nothing found
        /// </summary>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        public Task<TResult> ReadFirstOrDefaultAsync<TResult>()
        {
            return this.gridReader.ReadFirstOrDefaultAsync<TResult>();
        }

        /// <summary>
        /// Dispose the reader
        /// </summary>
        public void Dispose()
        {
            this.gridReader.Dispose();
        }
    }
}