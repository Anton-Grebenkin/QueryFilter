using QueryFilter.Models;
using System.Linq.Expressions;

namespace QueryFilter.Operators
{
    internal class OperatorLessThan : Operator
    {
        public OperatorLessThan(FilterItem filterItem) : base(filterItem, true){}

        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.LessThan(memberExpression, constantExpression);
        }
    }
}
