using AngelORM.Expressions;
using System;
using System.Linq.Expressions;

namespace AngelORM
{
    public class ExpressionResolver
    {
        public string Resolve<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            Visitor<T> visitor = new Visitor<T>();
            
            return visitor.Visit(predicate.Body);
        }
    }
}
