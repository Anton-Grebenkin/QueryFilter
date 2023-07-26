using QueryFilter.Models;
using System.Linq.Expressions;

namespace QueryFilter.Operators
{
    internal class OperatorGreaterThanOrEqual : Operator
    {
        public OperatorGreaterThanOrEqual(FilterItem filterItem) : base(filterItem, true){}

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
        }
    }
}
