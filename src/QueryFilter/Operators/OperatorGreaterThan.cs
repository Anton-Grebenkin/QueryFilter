using QueryFilter.Models;
using System.Linq.Expressions;

namespace QueryFilter.Operators
{
    internal class OperatorGreaterThan : Operator
    {
        public OperatorGreaterThan(FilterNode filterItem) : base(filterItem, true){}

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.GreaterThan(memberExpression, constantExpression);
        }
    }
}
