using QueryFilter.Presets;
using System.Reflection;

namespace QueryFilter.Utils
{
    internal static class ValueHolderUtility
    {
        internal static object GetValueHolderWithValue(object value)
        {
            var valueHolderType = ValueHolderPresets.ValueHolderType;

            Type[] typeArgs = { value.GetType() };

            var genericType = valueHolderType.MakeGenericType(typeArgs);

            object instance = Activator.CreateInstance(genericType);

            var valueProperty = instance.GetType().GetProperty(nameof(ValueHolder<object>.Value), 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            valueProperty.SetValue(instance, value, null);

            return instance;
        }
    }
}
