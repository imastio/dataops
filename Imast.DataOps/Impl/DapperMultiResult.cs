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
        private readonly SqlMapper.GridReader gridReader;
        
        /// <summary>
        /// Creates a multi result reader
        /// </summary>
        /// <param name="gridReader">The grid reader instance</param>
        public DapperMultiResult(SqlMapper.GridReader gridReader)
        {
            this.gridReader = gridReader;
        }
        
        /// <summary>
        /// Reads the next result set
        /// </summary>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <returns></returns>
        public Task<IEnumerable<TResult>> ReadAsync<TResult>()
        {
            return this.gridReader.ReadAsync<TResult>();
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