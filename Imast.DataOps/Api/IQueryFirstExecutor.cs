using System.Threading.Tasks;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The query first object execution interface
    /// </summary>
    public interface IQueryFirstExecutor : IOperationExecutor<IQueryFirstExecutor>
    {
        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="param">The parameter if given</param>
        /// <param name="mode">The mode of query</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(object param = null, QueryFirstMode mode = QueryFirstMode.FirstOrDefault);
    }
}