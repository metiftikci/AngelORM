using System.Collections.Generic;
using System.Linq;

namespace AngelORM
{
    public class Table
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<Column> Columns { get; set; }

        public List<Column> ColumnsWithoutPrimaryKeyColumns => Columns.Where(x => !x.IsPrimaryKey).ToList();
        public List<Column> PrimaryKeyColumns => Columns.Where(x => x.IsPrimaryKey).ToList();
        public bool HasPrimaryKeyColumn => PrimaryKeyColumns.Count > 0;
    }
}
