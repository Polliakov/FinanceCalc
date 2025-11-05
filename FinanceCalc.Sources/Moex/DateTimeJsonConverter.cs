using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinanceCalc.Sources.Moex
{
    public class DateTimeJsonConverter : JsonConverter<DateTime?>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return Nullable.GetUnderlyingType(typeToConvert) == typeof(DateTime);
        }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String &&
                reader.GetString() is { } date)
            {
                if (date == "0000-00-00")
                    return null;
                return DateTime.Parse(date, CultureInfo.InvariantCulture);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value?.ToString("o", CultureInfo.InvariantCulture));
            else
                writer.WriteNullValue();
        }
    }
}