namespace Imast.DataOps.Definitions
{
    /// <summary>
    /// The definition of command
    /// </summary>
    public class CommandDefinition
    {
        /// <summary>
        /// The command source (text or stored procedure)
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The command type
        /// </summary>
        public CommandTypeOption Type { get; set; }

        /// <summary>
        /// The result expected from the operation
        /// </summary>
        public ExpectedResult ExpectedResult { get; set; }

        /// <summary>
        /// Indicates if command is declared as non-supported
        /// </summary>
        public bool NotSupported { get; set; }
    }
}