namespace Imast.DataOps.Api
{
    /// <summary>
    /// The operation registry interface
    /// </summary>
    public interface IOperationRegistry
    {
        /// <summary>
        /// Registers the operation with key and provider
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <param name="provider">The provider of operation</param>
        /// <param name="operation">The operation itself</param>
        void Register(OpKey key, SqlProvider provider, SqlOperation operation);

        /// <summary>
        /// Checks if operation is defined for at least one provider
        /// </summary>
        /// <param name="key">The operation key</param>
        /// <returns></returns>
        bool IsDefined(OpKey key);

        /// <summary>
        /// Try get operation for given key and provider and null otherwise
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        SqlOperation GetOrNull(OpKey key, SqlProvider provider);
    }
}