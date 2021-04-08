namespace Imast.DataOps.Api
{
    /// <summary>
    /// The transactional behavior
    /// </summary>
    public enum AutoTransaction
    {
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