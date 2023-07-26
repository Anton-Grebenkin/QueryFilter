using QueryFilter.Models;
using System.Linq.Expressions;

namespace QueryFilter.Operators
{
    internal class OperatorLessThanOrEqual : Operator
    {
        public OperatorLessThanOrEqual(FilterItem filterItem) : base(filterItem, true){}

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.LessThanOrEqual(memberExpression, constantExpression);
        }
    }
}
