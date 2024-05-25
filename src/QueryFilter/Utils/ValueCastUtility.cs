using QueryFilter.Presets;
using System.Reflection;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace QueryFilter.Utils
{
    internal static class ValueCastUtility
    {
        internal static object CastValueToType(object initialValue, Type targetType)
        {
            if (initialValue == null)
                throw new ArgumentNullException(nameof(initialValue));

            if (targetType == StringPresets.StringType)
                return initialValue.ToString();

            if (targetType.GetTypeInfo().IsEnum)
                return Enum.Parse(targetType, initialValue.ToString());

            if (typeof(IConvertible).IsAssignableFrom(targetType))
                return Convert.ChangeType(initialValue.ToString(), targetType);

            throw new InvalidCastException($"Cannot convert value to type {targetType.Name}.");
        } 

        internal static object CastJsonStringToArrayOfType(string jsonString,  Type targetType)
        {
            var jsonElement = JsonDocument.Parse(jsonString).RootElement;
            var arrayType = targetType.MakeArrayType();
            return JsonSerializer.Deserialize(jsonElement.GetRawText(), arrayType);
        }
    }
}
