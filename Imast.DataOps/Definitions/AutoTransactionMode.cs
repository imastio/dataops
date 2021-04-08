namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The possible modes for transactional execution
    /// </summary>
    public enum AutoTransactionMode
    {
        /// <summary>
        /// Indicates that no transaction should be created for the operation
        /// </summary>
        No,

        /// <summary>
        /// The transaction with repeatable read
        /// </summary>
        RepeatableRead,

        /// <summary>
        /// The transaction with read uncommitted mode 
        /// </summary>
        ReadUncommitted,

        /// <summary>
        /// The transaction with read committed mode
        /// </summary>
        ReadCommitted,

        /// <summary>
        /// The transaction with serializable option
        /// </summary>
        Serializable
    }
}