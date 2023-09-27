using Microsoft.Extensions.Caching.Memory;
using QueryFilter.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
