
namespace QueryFilter.Presets
{
    internal static class QueryablePresets
    {
        internal static readonly Type QueryableType = typeof(Queryable);
        internal static string OrderByDescendingMethodName = nameof(Queryable.OrderByDescending);
        internal static string OrderByMethodName = nameof(Queryable.OrderBy);
        internal static string ThenByMethodName = nameof(Queryable.ThenBy);
        internal static string ThenByDescendingMeyhodName = nameof(Queryable.ThenByDescending);
    }
}
