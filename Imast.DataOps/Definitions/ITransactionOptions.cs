namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The transaction options definition
    /// </summary>
    public interface ITransactionOptions
    {
        /// <summary>
        /// The mode of transactional execution
        /// </summary>
        AutoTransactionMode? AutoTransaction { get; set; }
    }
}