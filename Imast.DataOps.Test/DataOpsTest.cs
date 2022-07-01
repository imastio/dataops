
using Microsoft.Data.Sqlite;

namespace Imast.DataOps.Test
{
    [TestClass]
    public class DataOpsTest
    {
        private readonly DataOperations dataOps;

        public DataOpsTest()
        {
            this.dataOps = DataOperationsBuilder.New()
                .WithSchemaValidation()
                .WithSource("Operations/MiscOperations.xml")
                .WithDefaultProvider(SqlProvider.SQLite)
                .WithConnection(SqlProvider.SQLite, () => new SqliteConnection("Data Source=Assets/northwind.sqlite3"))
                .WithConnection("chinook", SqlProvider.SQLite, () => new SqliteConnection("Data Source=Assets/chinook.db"))
                .Build();
        }

        [TestMethod]
        public async Task Get_One_Returns_One()
        {
            var one = await this.dataOps.Connect().QueryFirst("Group", "GetOne").ExecuteAsync<int>();

            Assert.AreEqual(1, one);
        }

        [TestMethod]
        public async Task Get_Product_By_Id_1_If_Bound_Returns_Id()
        {
            var result = await this.dataOps.Connect()
                .Query("Products", "GetById1IfBound")
                .WithBinding("ByIdOne", true)
                .WithBinding("ByIdTwo", false)
                .ExecuteAsync<dynamic>();

            Assert.AreEqual(1, result.First().Id);
        }
    }
}