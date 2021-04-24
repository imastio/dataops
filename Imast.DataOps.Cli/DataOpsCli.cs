using System;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using Imast.DataOps.Api;
using Imast.DataOps.Cli.Model;
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
            var northwindConnectionString = @"Server = (local); Initial Catalog = Northwind; user=root;password=root";

            // build new data operations
            var dataOps = DataOperationsBuilder.New()
                .WithConnection(SqlProvider.SqlServer, () => new SqlConnection(northwindConnectionString))
                .WithDefaultProvider(SqlProvider.SqlServer)
                .WithSchemaValidation()
                .WithSource("Samples/NorthwindOperations.xml")
                .Build();


            var filtered = dataOps.Connect().Query("Identity", "FilterGrants")
                .WithBinding("useSubject", true)
                .ExecuteAsync<dynamic>().Result;

            Console.WriteLine($"Log Written: {0}");
        }
    }
}
