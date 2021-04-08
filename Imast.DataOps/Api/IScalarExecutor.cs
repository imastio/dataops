using System.Threading.Tasks;

namespace Imast.DataOps.Api
{
    /// <summary>
    /// The scalar query executor
    /// </summary>
    public interface IScalarExecutor : IOperationExecutor<IScalarExecutor>
    {
        /// <summary>
        /// Execute the current operation
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="param">The parameter if given</param>
        /// <returns></returns>
        Task<TResult> ExecuteAsync<TResult>(object param = null);
    }
}