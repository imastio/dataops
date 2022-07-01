
using System.Data.SqlClient;

namespace Imast.DataOps.Test
{
    [TestClass]
    public class DataOperationsBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Create_Builder_Without_Default_Provider()
        {
            var ops = DataOperationsBuilder.New()
                .WithConnection(SqlProvider.SqlServer, () => new SqlConnection("some connection string"))
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Create_Builder_With_Invalid_Schema()
        {
            var ops = DataOperationsBuilder.New()
                .WithConnection(SqlProvider.SqlServer, () => new SqlConnection("some connection string"))
                .WithSource("Operations/InvalidOperations.xml")
                .WithDefaultProvider(SqlProvider.SqlServer)
                .WithSchemaValidation()
                .Build();
        }
    }
}