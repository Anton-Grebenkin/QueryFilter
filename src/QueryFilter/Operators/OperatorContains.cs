using QueryFilter.Models;
using QueryFilter.Presets;
using System.Linq.Expressions;


namespace QueryFilter.Operators
{
    internal class OperatorContains : Operator
    {
        internal OperatorContains(FilterNode filterItem) : base(filterItem) { }
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            if (memberExpression.Type != StringPresets.StringType)
                throw new InvalidCastException("Type of property must be string");

            var expression = Expression.Call(memberExpression, StringPresets.ContainsMethod, constantExpression);

            return expression;
        }
    }
}
