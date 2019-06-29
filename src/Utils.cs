using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AngelORM
{
    public class Utils
    {
        public Table GetTable<T>()
            where T : class
        {
            Type type = typeof(T);
            
            Table table = new Table();
            table.Name = type.Name;
            table.DisplayName = type.Name;
            table.Columns = GetColumns<T>();

            return table;
        }

        public List<Column> GetColumns<T>()
            where T : class
        {
            List<Column> columns = new List<Column>();

            foreach(PropertyInfo property in typeof(T).GetProperties())
            {
                Column column = new Column();
                column.IsPrimaryKey = Regex.IsMatch(property.Name, "^(I|i)(D|d)$");
                column.IsForeignKey = false;
                column.Name = property.Name;
                column.Alias = property.Name;
                column.DisplayName = property.Name;

                columns.Add(column);
            }

            return columns;
        }
    }
}
