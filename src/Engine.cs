using System;
using System.Data;
using System.Data.SqlClient;

namespace AngelORM
{
    public class Engine
    {
        public bool HasTransaction => _transaction != null;
        public int CommandTimeout { get; set; } = 30;

        private string _connectionString;
        private Transaction _transaction;
        private QueryCreator _queryCreator;
        private ParameterCreator _parameterCreator;
        private Utils _utils;

        public Engine(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(connectionString);

            _connectionString = connectionString;
            _queryCreator = new QueryCreator();
            _parameterCreator = new ParameterCreator();
            _utils = new Utils();
        }

        public SelectOperation<T> Select<T>()
            where T : class, new()
        {
            return new SelectOperation<T>(this);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public void Insert<T>(T model)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Table table = _utils.GetTable<T>();

            string query = _queryCreator.CreateInsertQuery<T>();
            SqlParameter[] parameters = _parameterCreator.CreateAllParametersWithoutPrimaryKeyColumns<T>(model).ToArray();

            if (table.HasPrimaryKeyColumn)
            {
                object key = ExecuteScalar(query, parameters);

                typeof(T).GetProperty(table.PrimaryKeyColumns[0].Alias).SetValue(model, key, null);
            }
            else
            {
                ExecuteNonQuery(query, parameters);
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        public int Update<T>(T model)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            string query = _queryCreator.CreateUpdateQuery<T>();
            SqlParameter[] parameters = _parameterCreator.CreateAllParameters<T>(model).ToArray();

            return ExecuteNonQuery(query, parameters);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public int Delete<T>(T model)
            where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            string query = _queryCreator.CreateDeleteQuery<T>();
            SqlParameter parameter = _parameterCreator.CreatePrimaryKeyParameter<T>(model);

            return ExecuteNonQuery(query, parameter);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            if (_transaction != null) return _transaction.ExecuteNonQuery(query, parameters);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;
                    command.CommandTimeout = CommandTimeout;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            if (_transaction != null) return _transaction.ExecuteScalar(query, parameters);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;
                    command.CommandTimeout = CommandTimeout;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteScalar();
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        public DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            if (_transaction != null) return _transaction.ExecuteDataTable(query, parameters);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;
                    command.CommandTimeout = CommandTimeout;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
        }

        /// <exception cref="InvalidOperationException">If transaction already created.</exception>
        public Transaction BeginTransaction()
        {
            if (HasTransaction) throw new InvalidOperationException("This engine has already a transaction.");

            _transaction = new Transaction(this, _connectionString);

            return _transaction;
        }

        /// <exception cref="AngelORMException">If transaction not created.</exception>
        public void EndTransaction()
        {
            if (!HasTransaction) throw new AngelORMException("Transaction not found.");

            if (!_transaction.IsDisposed)
            {
                _transaction.Dispose();
            }
            else
            {
                _transaction = null;
            }
        }

        internal void ClearTransactionValue()
        {
            _transaction = null;
        }
    }
}
