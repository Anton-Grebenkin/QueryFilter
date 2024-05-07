using QueryFilter.Models;
using QueryFilter.Presets;
using QueryFilter.Utils;
using System.Linq.Expressions;

namespace QueryFilter.Operators
{
    internal class OperatorIn : Operator
    {
        internal OperatorIn(FilterNode item) : base(item) { }
        protected override Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression)
        {
            return Expression.Call(EnumerablePresets.ContainsMethod.MakeGenericMethod(memberExpression.Type), constantExpression, memberExpression);
        }

        protected override Expression GetConstantExpression(MemberExpression memberExpression)
        {
            var enumerable = _filterItem.Value.GetType().GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == EnumerablePresets.GenericIEnumerableType)
                .FirstOrDefault();

            if (enumerable != null && enumerable.GenericTypeArguments.First() == memberExpression.Type)
            {
                return Expression.Constant(_filterItem.Value);
            }
                
            var value = ValueCastUtility.CastJsonStringToArrayOfType(_filterItem.Value.ToString(), memberExpression.Type);

            return Expression.Constant(value);

        }
    }
}
