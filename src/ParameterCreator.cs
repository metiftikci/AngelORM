using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AngelORM
{
    public class ParameterCreator
    {
        /// <exception cref="ArgumentNullException"></exception>
        public List<SqlParameter> CreateAllParameters<T>(T model)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Utils utils = new Utils();

            Table table = utils.GetTable<T>();

            return CreateParameters(model, table.Columns);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public SqlParameter CreatePrimaryKeyParameter<T>(T model)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Utils utils = new Utils();

            Table table = utils.GetTable<T>();

            if (!table.HasPrimaryKeyColumn) throw new PrimaryKeyNotFoundException();

            return CreateParameters(model, table.PrimaryKeyColumns).First();
        }

        /// <exception cref="ArgumentNullException"></exception>
        public List<SqlParameter> CreateAllParametersWithoutPrimaryKeyColumns<T>(T model)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Utils utils = new Utils();

            Table table = utils.GetTable<T>();

            return CreateParameters(model, table.ColumnsWithoutPrimaryKeyColumns);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public List<SqlParameter> CreateParameters<T>(T model, List<Column> columns)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            List<SqlParameter> parameters = new List<SqlParameter>();

            Type type = typeof(T);

            foreach (Column column in columns)
            {
                object value = type.GetProperty(column.Alias).GetValue(model, null);

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = $"@{column.Alias}";
                parameter.Value = value == null ? DBNull.Value : value;

                parameters.Add(parameter);
            }

            return parameters;
        }
    }
}
