using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace AngelORM
{
    public class SelectOperation<T>
        where T : class, new()
    {
        private Engine _engine;
        private Utils _utils;
        private string _selectQuery;

        private string _where;
        private string _orderBy;

        public SelectOperation(Engine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));

            _engine = engine;
            _utils = new Utils();

            QueryCreator queryCreator = new QueryCreator();
            _selectQuery = queryCreator.CreateSelectQuery<T>();
        }

        public string ToSQL()
        {
            string query = _selectQuery;

            if (_where != null) query += _where;
            if (_orderBy != null) query += _orderBy;

            return query;
        }

        public List<T> ToList()
        {
            string query = ToSQL();

            DataTable dataTable = _engine.ExecuteDataTable(query);

            return _utils.ConvertDataTableToList<T>(dataTable);
        }

        public SelectOperation<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (_where != null) throw new InvalidOperationException("The 'Where' method already used.");

            ExpressionResolver expressionResolver = new ExpressionResolver();

            _where = @"
WHERE " + expressionResolver.ResolveWhere(predicate);

            return this;
        }

        public SelectOperation<T> OrderBy<TResult>(Expression<Func<T, TResult>> columnSelector)
        {
            ExpressionResolver expressionResolver = new ExpressionResolver();
            string columnName = expressionResolver.ResolveOrderBy<T, TResult>(columnSelector);

            if (_orderBy == null)
            {
                _orderBy = @"
ORDER BY " + columnName;
            }
            else
            {
                _orderBy += ", " + columnName;
            }

            return this;
        }

        public SelectOperation<T> OrderByDescending<TResult>(Expression<Func<T, TResult>> columnSelector)
        {
            OrderBy(columnSelector);

            _orderBy += " DESC";
            
            return this;
        }
    }
}
