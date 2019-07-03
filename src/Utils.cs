using System;
using System.Collections.Generic;
using System.Data;
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

                columns.Add(column);
            }

            return columns;
        }

        public List<T> ConvertDataTableToList<T>(DataTable dataTable)
            where T : class, new()
        {
            Table table = GetTable<T>();

            Type type = typeof(T);

            List<T> list = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T model = new T();

                foreach (Column column in table.Columns)
                {
                    object value = row[column.Alias];

                    if (value is DBNull) continue;

                    type.GetProperty(column.Alias).SetValue(model, value, null);
                }

                list.Add(model);
            }

            return list;
        }
    }
}
