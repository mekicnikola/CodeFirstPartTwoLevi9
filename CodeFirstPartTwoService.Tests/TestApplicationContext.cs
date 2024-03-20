using CodeFirstPartTwoApplication.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace CodeFirstPartTwoService.Tests
{
    public class TestApplicationContext : ApplicationContext
    {
        public TestApplicationContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestCodeFirstPartTwoDb;Integrated Security=True")
        {
        }

        public void ResetState()
        {
            Database.Delete();
            Database.Create();
        }
    }

}
