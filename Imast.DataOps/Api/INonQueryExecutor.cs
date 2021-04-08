using System.Threading.Tasks;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The non-query executor interface
    /// </summary>
    public interface INonQueryExecutor : IOperationExecutor<INonQueryExecutor>
    {
        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(object param = null);
    }
}