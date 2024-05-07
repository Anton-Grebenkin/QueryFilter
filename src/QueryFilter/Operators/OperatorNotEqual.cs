using QueryFilter.Models;
using System.Linq.Expressions;

namespace QueryFilter.Operators
{
    internal class OperatorNotEqual : Operator
    {
        internal OperatorNotEqual(FilterNode filterItem) : base(filterItem, true){}
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.NotEqual(memberExpression, constantExpression);
        }
    }
}
