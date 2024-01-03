using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAiQuickstart.BusinessDomain.Domain;

namespace OpenAiQuickstart.BusinessApi;

public class MoneyJsonConverter : JsonConverter<Money>
{
    public override Money Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => new Money { Cents = reader.GetInt64() };

    public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.Cents);
}