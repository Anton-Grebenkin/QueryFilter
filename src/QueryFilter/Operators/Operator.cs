using QueryFilter.Models;
using QueryFilter.Presets;
using QueryFilter.Utils;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryFilter.Operators
{
    internal abstract class Operator
    {
        internal static Operator GetOperator(FilterItem filterItem)
        {
            return filterItem.Operator switch
            {
                OperatorType.Equal => new OperatorEqual(filterItem),
                OperatorType.Contains => new OperatorContains(filterItem),
                OperatorType.In => new OperatorIn(filterItem),
                OperatorType.GreaterThan => new OperatorGreaterThan(filterItem),
                OperatorType.GreaterThanOrEqual => new OperatorGreaterThanOrEqual(filterItem),
                OperatorType.IsNull => new OperatorIsNull(filterItem),
                OperatorType.LessThan => new OperatorLessThan(filterItem),
                OperatorType.LessThanOrEqual => new OperatorLessThanOrEqual(filterItem),
                OperatorType.NotEqual => new OperatorNotEqual(filterItem),
                _ => throw new NotImplementedException()
            };
        }

        protected readonly FilterItem _filterItem;
        private readonly bool _withValueHolder = false;
        private protected Operator(FilterItem filterItem)
        {
            _filterItem = filterItem;
        }
        private protected Operator(FilterItem filterItem, bool withValueHolder)
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
