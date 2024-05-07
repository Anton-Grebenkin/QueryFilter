using QueryFilter.Models;
using QueryFilter.Operators;
using QueryFilter.Presets;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryFilter
{
    public interface IQueryBuilder<T> where T : class
    {
        IQueryable<T> BuildQuery(IQueryable<T> query, Filter filter);
    }

    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private readonly Type _type;
        private readonly ParameterExpression _parameter;
        private readonly IEnumerable<PropertyInfo> _properties;

        public QueryBuilder()
        {
            _type = typeof(T);
            _properties = _type.GetProperties();
            _parameter = Expression.Parameter(_type, "x");
        }

        public IQueryable<T> BuildQuery(IQueryable<T> query, Filter filter)
        {
            if (filter == null)
                return query;

            if (filter.MainNode?.FilterNodes != null && 
                filter.MainNode.FilterNodes.Any(i => i != null) && 
                filter.MainNode.LogicalOperator != null)
                query = AddFilters(query, filter.MainNode.FilterNodes.Where(i => i != null), filter.MainNode.LogicalOperator.Value);

            if (filter.Take.HasValue)
                query = AddPaging(query, filter.Take.Value, filter.Skip);

            if (filter.Sorts != null && filter.Sorts.Any())
                query = AddSort(query, filter.Sorts.ToArray());

            return query;
        }

        private IQueryable<T> AddFilters(IQueryable<T> query, IEnumerable<FilterNode> filterItems, LogicalOperatorType logicalOperator)
        {
            return query.Where(Expression.Lambda<Func<T, bool>>(GetFiltersExpressionBody(filterItems, logicalOperator), _parameter));
        }

        private Expression GetFiltersExpressionBody(IEnumerable<FilterNode> filterItems, LogicalOperatorType logicalOperator)
        {
            Expression? body = null;
            Expression? itemExpression = null;
            foreach (var item in filterItems)
            {
                if (item.FilterNodes != null && item.FilterNodes.Any() && item.LogicalOperator.HasValue)
                {
                    itemExpression = GetFiltersExpressionBody(item.FilterNodes, item.LogicalOperator.Value);
                }
                else
                {
                    var property = _properties.FirstOrDefault(p => string.Equals(p.Name, item.PropertyName, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                        throw new InvalidOperationException($"Property with name {item.PropertyName} doesn't exist");

                    var @operator = Operator.GetOperator(item);

                    itemExpression = @operator.GetExpression(_parameter);
                }
                body = body != null ? GetExpressionByLogicalOperatorType(logicalOperator, body, itemExpression) : itemExpression;
            }

            return body;
        }

        private IQueryable<T> AddPaging(IQueryable<T> query, int take, int? skip)
        {
            if (!skip.HasValue)
                skip = 0;

            return query
                .Skip(skip.Value)
                .Take(take);
        }

        private IQueryable<T> AddSort(IQueryable<T> query, PropertySort[] sorts)
        {
            var queryExpr = query.Expression;

            for (var i = 0; i < sorts.Length; i++)
            {
                var sort = sorts[i];

                var command = string.Empty;
                if (sort.Desc)
                {
                    if (i == 0)
                        command = QueryablePresets.OrderByDescendingMethodName;
                    else
                        command = QueryablePresets.ThenByDescendingMeyhodName;
                }
                else
                {
                    if (i == 0)
                        command = QueryablePresets.OrderByMethodName;
                    else
                        command = QueryablePresets.ThenByMethodName;
                }

                var property = _properties.FirstOrDefault(p => p.Name.ToUpper() == sorts[i].PropertyName.ToUpper());
                if (property == null)
                    throw new InvalidOperationException($"Property with name {sort.PropertyName} doesn't exist");

                var propertyAccess = Expression.MakeMemberAccess(_parameter, property);

                var orderByExpression = Expression.Lambda(propertyAccess, _parameter);

                queryExpr = Expression.Call(
                    type: QueryablePresets.QueryableType,
                    methodName: command,
                    typeArguments: new Type[] { _type, property.PropertyType },
                    queryExpr,
                    Expression.Quote(orderByExpression));

                query = query.Provider.CreateQuery<T>(queryExpr);
            }

            return query;
        }

        private BinaryExpression GetExpressionByLogicalOperatorType(LogicalOperatorType logicalOperatorType, Expression left, Expression right) =>
            logicalOperatorType switch
            {
                LogicalOperatorType.And => Expression.AndAlso(left, right),
                LogicalOperatorType.Or => Expression.OrElse(left, right),
                _ => throw new NotImplementedException()
            };
    }
}
