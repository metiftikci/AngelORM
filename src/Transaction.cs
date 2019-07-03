using System;
using System.Data;
using System.Data.SqlClient;

namespace AngelORM
{
    public class Transaction : IDisposable
    {
        public bool IsDisposed { get; private set; }

        private Engine _engine;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        /// <exception cref="ArgumentNullException"></exception>
        public Transaction(Engine engine, string connectionString)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            
            _engine = engine;
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Transaction = _transaction;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteNonQuery();
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Transaction = _transaction;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteScalar();
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Transaction = _transaction;

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

        /// <exception cref="ObjectDisposedException"></exception>
        public void Commit()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);

            _transaction.Commit();
        }

        /// <exception cref="ObjectDisposedException"></exception>
        public void Rollback()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);

            _transaction.Rollback();
        }

        /// <exception cref="ObjectDisposedException"></exception>
        public void Dispose()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().Name);

            _transaction.Dispose();
            _connection.Dispose();
            IsDisposed = true;

            if (_engine.HasTransaction)
            {
                _engine.ClearTransactionValue();
            }
        }
    }
}
