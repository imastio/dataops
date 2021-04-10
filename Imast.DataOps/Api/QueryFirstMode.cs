namespace Imast.DataOps.Api
{
    /// <summary>
    /// The mode of query first operation
    /// </summary>
    public enum QueryFirstMode
    {
        /// <summary>
        /// Query the first object or default if nothing returned
        /// </summary>
        FirstOrDefault,

        /// <summary>
        /// Query the first object and throw if nothing found
        /// </summary>
        First,

        /// <summary>
        /// Query the single object and default otherwise
        /// </summary>
        SingleOrDefault,

        /// <summary>
        /// Query the single object or throw otherwise
        /// </summary>
        Single
    }
}