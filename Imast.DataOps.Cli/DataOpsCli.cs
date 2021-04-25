using System.Data.SqlClient;
using Imast.DataOps.Api;
using Imast.DataOps.Init;

namespace Imast.DataOps.Cli
{
    /// <summary>
    /// The Cli interface for DataOps Tests
    /// </summary>
    public class DataOpsCli
    {
        /// <summary>
        /// The entry point for CLI application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // the connection string to Northwind database
            var northwindConnectionString = @"Server = (local); Initial Catalog = Northwind; Integrated Security = True";

            // build new data operations
            var dataOps = DataOperationsBuilder.New()
                .WithConnection(SqlProvider.SqlServer, () => new SqlConnection(northwindConnectionString))
                .WithDefaultProvider(SqlProvider.SqlServer)
                .WithSchemaValidation()
                .WithSource("Samples/NorthwindOperations.xml")
                .Build();

            var options = new
            {
                SubjectId = "",
                SessionId = default(string),
                ClientId = "Client",
                Type = "something"
            };

            // static source generation
            var filtered = dataOps.Connect().Query("Identity", "GetGrantsBy")
                .WithBinding("useSubject", options.SubjectId != null)
                .WithBinding("useSession", options.SessionId != null)
                .WithBinding("useClient", options.ClientId != null)
                .WithBinding("useType", options.Type != null)
                .ExecuteAsync<dynamic>().Result;

        }
    }
}
