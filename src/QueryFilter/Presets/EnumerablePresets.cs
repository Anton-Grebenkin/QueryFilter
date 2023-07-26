
using System.Reflection;

namespace QueryFilter.Presets
{
    internal static class EnumerablePresets
    {
        internal static readonly Type GenericIEnumerableType = typeof(IEnumerable<>);

        internal static readonly Type StaticEnumerableType = typeof(Enumerable);

        internal static readonly MethodInfo ContainsMethod = StaticEnumerableType
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Single(x => x.Name == "Contains" && x.GetParameters().Length == 2);
    }
}
