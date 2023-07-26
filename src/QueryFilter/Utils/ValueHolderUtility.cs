using QueryFilter.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            var valueProperty = instance.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(p => p.Name == nameof(ValueHolder<object>.Value));

            valueProperty.SetValue(instance, value, null);

            return instance;
        }
    }
}
