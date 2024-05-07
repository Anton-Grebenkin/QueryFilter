using QueryFilter.Models;
using System.Linq.Expressions;


namespace QueryFilter.Operators
{
    internal class OperatorIsNull : Operator
    {
        public OperatorIsNull(FilterNode filterItem) : base(filterItem)
        {
        }

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.Equal(memberExpression, constantExpression);
        }

        protected override Expression GetConstantExpression(MemberExpression memberExpression)
        {
            return Expression.Constant(null);
        }
    }
}
