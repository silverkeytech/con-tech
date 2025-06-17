using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ConTech.Core;

public static class JsonHelpers
{
    public static string ModifyJsonWithHash<T>(T obj)
    {
        var node = JsonSerializer.SerializeToNode(obj);
        if (node == null) throw new ArgumentException("Invalid object for serialization.");

        ModifyIdFields(node);

        return JsonSerializer.Serialize(node);
    }

    private static void ModifyIdFields(JsonNode? node)
    {
        if (node is JsonObject jsonObject)
        {
            foreach (var property in jsonObject.ToList())
            {
                var propertyName = property.Key;
                var propertyValue = property.Value;

                if (propertyName.Contains("id", StringComparison.InvariantCultureIgnoreCase) &&
                    !propertyName.Contains("governorate", StringComparison.InvariantCultureIgnoreCase) &&
                    !propertyName.Contains("type", StringComparison.InvariantCultureIgnoreCase) &&
                    propertyValue is JsonValue value && value.TryGetValue<int>(out var intValue) && intValue != 0)
                {
                    jsonObject[propertyName] = Hash.EncodeInt(intValue);
                }

                else if (!propertyName.Contains("geographic", StringComparison.InvariantCultureIgnoreCase))
                {
                    ModifyIdFields(propertyValue);
                }
            }
        }
        else if (node is JsonArray jsonArray)
        {
            foreach (var item in jsonArray)
            {
                ModifyIdFields(item);
            }
        }
    }

    public static T RevertJsonWithDecodedHash<T>(JsonNode json)
    {
        RevertHashedIdFields(json);
        return json.Deserialize<T>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new NumericConverter<T>() }
        })!;
    }

    private static void RevertHashedIdFields(JsonNode? node)
    {
        if (node is JsonObject jsonObject)
        {
            foreach (var property in jsonObject.ToList())
            {
                var propertyName = property.Key;
                var propertyValue = property.Value;

                if (propertyName.Contains("id", StringComparison.InvariantCultureIgnoreCase) &&
                    !propertyName.Contains("governorate", StringComparison.InvariantCultureIgnoreCase) &&
                    !propertyName.Contains("type", StringComparison.InvariantCultureIgnoreCase) &&
                    !propertyName.Contains("roleid", StringComparison.InvariantCultureIgnoreCase) &&
                    propertyValue is JsonValue value && value.TryGetValue<string>(out var hashValue))
                {
                    try
                    {
                        var decodedValue = Hash.DecodeToLong(hashValue);
                        jsonObject[propertyName] = decodedValue;
                    }
                    catch
                    {
                        jsonObject[propertyName] = propertyValue;
                    }
                }
                else if (!propertyName.Contains("geographic", StringComparison.InvariantCultureIgnoreCase))
                {
                    RevertHashedIdFields(propertyValue);
                }
            }
        }
        else if (node is JsonArray jsonArray)
        {
            foreach (var item in jsonArray)
            {
                RevertHashedIdFields(item);
            }
        }
    }
}

public class NumericConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (typeof(T) == typeof(short) && reader.TryGetInt16(out var shortValue))
                return (T)(object)shortValue;

            if (typeof(T) == typeof(int) && reader.TryGetInt32(out var intValue))
                return (T)(object)intValue;

            if (typeof(T) == typeof(long) && reader.TryGetInt64(out var longValue))
                return (T)(object)longValue;

            if (typeof(T) == typeof(float) && reader.TryGetSingle(out var floatValue))
                return (T)(object)floatValue;

            if (typeof(T) == typeof(double) && reader.TryGetDouble(out var doubleValue))
                return (T)(object)doubleValue;

            if (typeof(T) == typeof(decimal) && reader.TryGetDecimal(out var decimalValue))
                return (T)(object)decimalValue;
        }

        throw new JsonException($"Cannot convert {reader.TokenType} to {typeof(T)}.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(Convert.ToDouble(value));
    }
}