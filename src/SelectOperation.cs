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

        public SelectOperation(Engine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));

            _engine = engine;
            _utils = new Utils();

            QueryCreator queryCreator = new QueryCreator();
            _selectQuery = queryCreator.CreateSelectQuery<T>();
        }

        public List<T> ToList()
        {
            DataTable dataTable = _engine.ExecuteDataTable(_selectQuery + _where);

            return _utils.ConvertDataTableToList<T>(dataTable);
        }

        public SelectOperation<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (_where != null) throw new InvalidOperationException("The 'Where' method already used.");

            ExpressionResolver expResolver = new ExpressionResolver();
            
            _where = @"
WHERE " + expResolver.Resolve<T>(predicate);

            return this;
        }

        public SelectOperation<T> OrderBy<TResult>(Expression<Func<T, TResult>> qwe)
        {
            return this;
        }
    }
}
