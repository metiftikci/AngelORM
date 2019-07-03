using System.Collections.Generic;
using System.Linq;

namespace AngelORM
{
    public class QueryCreator
    {
        private Utils _utils = new Utils();

        public string CreateSelectQuery<T>()
            where T : class
        {
            Table table = _utils.GetTable<T>();

            string tableName = $"[{table.Name}]";

            List<string> columnNames = table.Columns.Select(x => $"[{x.Name}] AS [{x.Alias}]").ToList();
            string columns = string.Join("\r\n      ,", columnNames);

            string query = $@"SELECT {columns}
FROM {tableName}";

            return query;
        }

        public string CreateInsertQuery<T>()
            where T : class
        {
            Table table = _utils.GetTable<T>();
            Column primaryKeyColumn = table.Columns.FirstOrDefault(x => x.IsPrimaryKey);

            List<string> columnNames = table.ColumnsWithoutPrimaryKeyColumns.Select(x => $"[{x.Name}]").ToList();
            List<string> parameterNames = table.ColumnsWithoutPrimaryKeyColumns.Select(x => $"@{x.Alias}").ToList();

            string columns = string.Join("\r\n   ,", columnNames);
            string output = string.Empty;
            string parameters = string.Join("\r\n   ,", parameterNames);

            if (primaryKeyColumn != null)
            {
                output = $"\r\nOUTPUT inserted.[{primaryKeyColumn.Name}]";
            }

            string query = $@"INSERT INTO [{table.Name}] (
    {columns}
){output}
VALUES
(
    {parameters}
)";

            return query;
        }

        public string CreateUpdateQuery<T>()
            where T : class
        {
            Table table = _utils.GetTable<T>();

            if (!table.HasPrimaryKeyColumn) throw new PrimaryKeyNotFoundException();

            Column primaryKeyColumn = table.PrimaryKeyColumns.First();

            List<string> assignmentList = table.ColumnsWithoutPrimaryKeyColumns.Select(x => $"[{x.Name}] = @{x.Alias}").ToList();

            string tableName = $"[{table.Name}]";
            string assignments = string.Join("\r\n   ,", assignmentList);
            string primaryKeyColumnName = $"[{primaryKeyColumn.Name}]";
            string primaryKeyColumnParameter = $"@{primaryKeyColumn.Alias}";

            string query = $@"UPDATE {tableName}
SET {assignments}
WHERE {primaryKeyColumnName} = {primaryKeyColumnParameter}";

            return query;
        }

        public string CreateDeleteQuery<T>()
            where T : class
        {
            Table table = _utils.GetTable<T>();

            if (!table.HasPrimaryKeyColumn) throw new PrimaryKeyNotFoundException();

            Column primaryKeyColumn = table.PrimaryKeyColumns.First();

            string tableName = $"[{table.Name}]";
            string primaryKeyColumnName = $"[{primaryKeyColumn.Name}]";
            string primaryKeyColumnParameter = $"@{primaryKeyColumn.Alias}";

            string query = $"DELETE FROM {tableName} WHERE {primaryKeyColumnName} = {primaryKeyColumnParameter}";

            return query;
        }
    }
}
