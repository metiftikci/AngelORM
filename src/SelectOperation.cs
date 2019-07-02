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
            DataTable dataTable = _engine.ExecuteDataTable(_selectQuery);

            return _utils.ConvertDataTableToList<T>(dataTable);
        }

        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            ExpressionResolver expResolver = new ExpressionResolver();
            
            string query = _selectQuery + " " + expResolver.Resolve<T>(predicate);

            DataTable dataTable = _engine.ExecuteDataTable(query);

            return _utils.ConvertDataTableToList<T>(dataTable);
        }
    }
}
