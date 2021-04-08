using System;
using System.Data.SqlClient;
using System.Linq;
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
            var northwindConnectionString = @"Server = (local); Initial Catalog = Northwind; Integrated Security = true;";

            // build new data operations
            var dataOps = DataOperationsBuilder.New()
                .WithConnection(SqlProvider.SqlServer, () => new SqlConnection(northwindConnectionString))
                .WithDefaultProvider(SqlProvider.SqlServer)
                .WithSchemaValidation()
                .WithSource("Samples/NorthwindOperations.xml")
                .Build();


            // do operation and get result
            var multiResult = dataOps.Connect().MultiQuery("Products", "GetAll").ExecuteAsync(async reader =>
            {
                var products = await reader.ReadAsync<Product>();
                var cats = await reader.ReadAsync<dynamic>();

                return Tuple.Create(products, cats);
            }).Result;

            Console.WriteLine($"MultiRead Result: Products {multiResult.Item1.Count()}, Categories: {multiResult.Item2.Count()}");

            var writeResult = dataOps.Connect().NonQuery("Other", "WriteLog")
                .ExecuteAsync(new {Message = $"Hello. It's {DateTime.Now}"}).Result;

            Console.WriteLine($"Log Written: {writeResult}");
        }
    }
}
