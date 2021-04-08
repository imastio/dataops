namespace Imast.DataOps.Api
{
    /// <summary>
    /// The options of expected result
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// The expected result is unknown
        /// </summary>
        Unknown, 

        /// <summary>
        /// The operation is expected to return a table
        /// </summary>
        Table,

        /// <summary>
        /// The operation is expected to return multiple tables
        /// </summary>
        MultipleTables,

        /// <summary>
        /// The operation is expected to return a scalar value
        /// </summary>
        Scalar,

        /// <summary>
        /// The operation is expected to return number of affected rows
        /// </summary>
        RowCount
    }
}