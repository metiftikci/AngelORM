using System;
using System.Linq;
using System.Linq.Expressions;

namespace AngelORM
{
    public class ExpressionResolver
    {
        public string ResolveWhere<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            ExpressionVisitor<T> visitor = new ExpressionVisitor<T>();
            
            return visitor.Visit(predicate.Body);
        }

        public string ResolveOrderBy<T, TResult>(Expression<Func<T, TResult>> columnSelector)
            where T : class
        {
            if (columnSelector.Body is MemberExpression)
            {
                MemberExpression exp = (MemberExpression)columnSelector.Body;

                if (exp.Expression.NodeType != ExpressionType.Parameter)
                {
                    throw new AngelORMException("Expression should be parameter member access.");
                }
                else
                {
                    Utils utils = new Utils();
                    Table table = utils.GetTable<T>();

                    return "[" + table.Columns.First(x => x.Alias == exp.Member.Name).Name + "]";
                }
            }

            throw new AngelORMException("The 'columnSelector' property is not a MemberExpression.");
        }
    }
}
