using QueryFilter.Presets;
using Newtonsoft.Json;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using QueryFilter.Extensions;

namespace QueryFilter.Utils
{
    internal static class ValueCastUtility
    {
        internal static readonly Type[] AvailableCastTypes =
        {
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(Guid),
            typeof(double),
            typeof(float),
            typeof(decimal),
            typeof(char),
            typeof(string)
        };
        internal static object CastValueToType(object initialValue, Type targetType)
        {
            if (initialValue == null || (!AvailableCastTypes.Contains(targetType.GetNonNullableType()) && !targetType.GetTypeInfo().IsEnum))
                throw new InvalidCastException($"Cannot convert value to type {targetType.Name}.");

            var valueType = initialValue.GetType();

            if (valueType == targetType)
                return initialValue;

            if (targetType.GetTypeInfo().BaseType == typeof(Enum))
                return Enum.Parse(targetType, Convert.ToString(initialValue));

            var s = Convert.ToString(initialValue);

            if (targetType == StringPresets.StringType)
                return s;

            object res;

            if (targetType.GetTypeInfo().IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = targetType.GenericTypeArguments[0];
                res = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType));
            }
            else
            {
                res = Activator.CreateInstance(targetType);
            }

            var argTypes = new[] { StringPresets.StringType, targetType.MakeByRefType() };
            object[] args = { s, res };
            var tryParse = targetType.GetRuntimeMethod("TryParse", argTypes);

            if (!(bool)(tryParse?.Invoke(null, args) ?? false))
                throw new InvalidCastException($"Cannot convert value to type {targetType.Name}.");

            return args[1];
        } 

        internal static object CastJsonStringToArrayOfType(string jsonString,  Type targetType)
        {
            return JsonConvert.DeserializeObject(jsonString, targetType.MakeArrayType());
        }
    }
}
