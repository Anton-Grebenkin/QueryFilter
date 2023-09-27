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

            if (filter.Items != null && filter.Items.Any(i => i != null))
                query = AddFilters(query, filter.Items.Where(i => i != null));

            if (filter.Take.HasValue)
                query = AddPaging(query, filter.Take.Value, filter.Skip);

            if (filter.Sorts != null && filter.Sorts.Any())
                query = AddSort(query, filter.Sorts.ToArray());

            return query;
        }

        private IQueryable<T> AddFilters(IQueryable<T> query, IEnumerable<FilterItem> filterItems)
        {
            Expression? body = null;
            foreach (var item in filterItems)
            {
                var property = _properties.FirstOrDefault(p => p.Name.ToUpper() == item.PropertyName.ToUpper());
                if (property == null)
                    throw new InvalidOperationException($"Property with name {item.PropertyName} doesn't exist");

                var @operator = Operator.GetOperator(item);

                var itemExpression = @operator.GetExpression(_parameter);
                body = body != null ? Expression.AndAlso(body, itemExpression) : itemExpression;
            }

            return query.Where(Expression.Lambda<Func<T, bool>>(body, _parameter));
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
    }
}
