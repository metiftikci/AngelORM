using System;
using System.Linq;
using System.Linq.Expressions;

namespace AngelORM
{
    public class ExpressionVisitor<T>
        where T : class
    {
        private Table _table;

        public ExpressionVisitor()
        {
            Utils utils = new Utils();

            _table = utils.GetTable<T>();
        }

        public string Visit(Expression expression)
        {
            if (expression is ConstantExpression)
            {
                return VisitConstant((ConstantExpression)expression);
            }
            else if (expression is BinaryExpression)
            {
                return VisitBinary((BinaryExpression)expression);
            }
            else if (expression is MemberExpression)
            {
                return VisitMember((MemberExpression)expression);
            }
            else
            {
                throw new NotImplementedException($"Unsupported expression type: {expression.GetType().Name}");
            }
        }

        public string VisitBinary(BinaryExpression expression)
        {
            string op = string.Empty;

            switch (expression.NodeType)
            {
                case ExpressionType.And: op = "&"; break;
                case ExpressionType.AndAlso: op = "AND"; break;
                case ExpressionType.Or: op = "|"; break;
                case ExpressionType.OrElse: op = "OR"; break;
                case ExpressionType.Equal: op = "="; break;
                case ExpressionType.NotEqual: op = "<>"; break;
                case ExpressionType.LessThan: op = "<"; break;
                case ExpressionType.LessThanOrEqual: op = "<="; break;
                case ExpressionType.GreaterThan: op = ">"; break;
                case ExpressionType.GreaterThanOrEqual: op = ">="; break;
                case ExpressionType.Add: op = "+"; break;
                case ExpressionType.Subtract: op = "-"; break;
                case ExpressionType.Multiply: op = "*"; break;
                case ExpressionType.Divide: op = "/"; break;
                case ExpressionType.Modulo: op = "%"; break;
                default: throw new NotImplementedException($"Unsupported node type: {expression.NodeType}");
            }

            return $"({Visit(expression.Left)} {op} {(Visit(expression.Right))})";
        }

        public string VisitConstant(ConstantExpression expression)
        {
            return ConvertObjectToString(expression.Value);
        }

        public string VisitMember(MemberExpression expression)
        {
            if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Parameter)
            {
                string columnName = _table.Columns.First(x => x.Alias == expression.Member.Name).Name;

                return $"[{columnName}]";
            }
            else
            {
                return GetValue(expression);
            }
        }

        private string GetValue(MemberExpression expression)
        {
            var objectMember = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();

            return ConvertObjectToString(getter());
        }

        public string ConvertObjectToString(object obj)
        {
            if (obj is string)
            {
                return $"'{((string)obj).Replace("'", "''")}'";
            }
            else if (obj is int)
            {
                return ((int)obj).ToString();
            }
            else if (obj is DateTime)
            {
                DateTime dateTime = (DateTime)obj;

                return $"'{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            else if (obj is bool)
            {
                return (bool)obj ? "1" : "0";
            }
            else
            {
                throw new NotImplementedException($"Unsupported object type: {obj.GetType().Name}");
            }
        }
    }
}
