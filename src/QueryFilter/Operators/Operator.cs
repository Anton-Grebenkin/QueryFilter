using QueryFilter.Models;
using QueryFilter.Presets;
using QueryFilter.Utils;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryFilter.Operators
{
    internal abstract class Operator
    {
        internal static Operator GetOperator(FilterNode filterItem)
        {
            return filterItem.ExpressionOperator switch
            {
                Models.ExpressionOperatorType.Equal => new OperatorEqual(filterItem),
                Models.ExpressionOperatorType.Contains => new OperatorContains(filterItem),
                Models.ExpressionOperatorType.In => new OperatorIn(filterItem),
                Models.ExpressionOperatorType.GreaterThan => new OperatorGreaterThan(filterItem),
                Models.ExpressionOperatorType.GreaterThanOrEqual => new OperatorGreaterThanOrEqual(filterItem),
                Models.ExpressionOperatorType.IsNull => new OperatorIsNull(filterItem),
                Models.ExpressionOperatorType.LessThan => new OperatorLessThan(filterItem),
                Models.ExpressionOperatorType.LessThanOrEqual => new OperatorLessThanOrEqual(filterItem),
                Models.ExpressionOperatorType.NotEqual => new OperatorNotEqual(filterItem),
                _ => throw new NotImplementedException()
            };
        }

        protected readonly FilterNode _filterItem;
        private readonly bool _withValueHolder = false;
        private protected Operator(FilterNode filterItem)
        {
            _filterItem = filterItem;
        }
        private protected Operator(FilterNode filterItem, bool withValueHolder)
        {
            _filterItem = filterItem;
            _withValueHolder = withValueHolder;
        }

        internal Expression GetExpression(ParameterExpression parameter)
        {
            var member = GetMemberExpression(parameter);

            var constant = GetConstantExpression(member);

            var body = GetExpressionBody(member, constant);

            return body;

        }

        private MemberExpression GetMemberExpression(ParameterExpression parameter)
        {
            var member = Expression.PropertyOrField(parameter, _filterItem.PropertyName);
            return member;
        }

        protected virtual Expression GetConstantExpression(MemberExpression memberExpression)
        {
            var value = ValueCastUtility.CastValueToType(_filterItem.Value, memberExpression.Type);

            if (_withValueHolder)
            {
                var valueHolder = ValueHolderUtility.GetValueHolderWithValue(value);
                return Expression.PropertyOrField(Expression.Constant(valueHolder), nameof(ValueHolder<object>.Value));
            }
            else
            {
                return Expression.Constant(value);
            }
        }

        protected abstract Expression GetExpressionBody(MemberExpression memberExpression, Expression constantExpression);
    }

}
