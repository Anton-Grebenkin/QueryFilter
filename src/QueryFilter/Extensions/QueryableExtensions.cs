using QueryFilter.Models;

namespace QueryFilter.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Filter filter) where T : class
        {
            var queryBuilder = new QueryBuilder<T>();
            return queryBuilder.BuildQuery(query, filter);
        }

        public static QueryResult<T> GetQueryResult<T>(this IQueryable<T> query, Filter filter) where T : class
        {
            var queryBuilder = new QueryBuilder<T>();

            var items = queryBuilder
                .BuildQuery(query, filter)
                .ToList();

            var totalCount = query.Count();

            return new QueryResult<T>(items, totalCount);
        }
    }
}
