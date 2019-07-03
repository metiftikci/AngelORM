using AngelORM.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AngelORM.Tests
{
    public class UtilsTests
    {
        private Utils _utils = new Utils();

        [Fact]
        public void GetTableTest()
        {
            Table table = _utils.GetTable<User>();
            
            Assert.Equal("User", table.Name);
            Assert.NotNull(table.Columns);
        }

        [Fact]
        public void GetColumnsTest()
        {
            List<Column> columns = _utils.GetColumns<User>();

            Assert.Equal(8, columns.Count);
            Assert.True(columns[0].IsPrimaryKey);
            Assert.Equal("Name", columns[1].Name);
        }
    }
}
