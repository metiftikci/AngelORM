using System;
using System.Linq.Expressions;

namespace AngelORM
{
    public class ExpressionResolver
    {
        public string Resolve<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            ExpressionVisitor<T> visitor = new ExpressionVisitor<T>();
            
            return visitor.Visit(predicate.Body);
        }
    }
}
