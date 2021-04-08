namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The command type options
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// The command type is unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// The command is defined by full text 
        /// </summary>
        Text,

        /// <summary>
        /// The command is defined by reference to a stored procedure
        /// </summary>
        StoredProcedure,
        
        /// <summary>
        /// The command is defined by command template for bulk insert operation
        /// </summary>
        BulkInsert
    }
}