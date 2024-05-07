using QueryFilter.Models;
using System.Linq.Expressions;


namespace QueryFilter.Operators
{
    internal class OperatorEqual : Operator
    {
        internal OperatorEqual(FilterNode filterItem) : base(filterItem, true) {}
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            var expression = Expression.Equal(memberExpression, constantExpression);
            return expression;
        }
    }
}
