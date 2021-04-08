using System;

namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The set of compatibility providers
    /// </summary>
    [Flags]
    public enum CompatibilityProviders : long
    {
        /// <summary>
        /// The operation is not compatible with none of providers
        /// </summary>
        None = 0,
        
        /// <summary>
        /// The operation is applicable only for SqlServer provider
        /// </summary>
        SqlServer = 1,

        /// <summary>
        /// The operation is applicable only for MySQL provider
        /// </summary>
        MySQL = 2,

        /// <summary>
        /// The operation is applicable only for PostgreSQL provider
        /// </summary>
        PostgreSQL = 4,

        /// <summary>
        /// The operation is applicable only for SQLite provider
        /// </summary>
        SQLite = 8,

        /// <summary>
        /// The operations is applicable to be executed any available provider
        /// </summary>
        Any = SqlServer | MySQL | PostgreSQL | SQLite
    }
}