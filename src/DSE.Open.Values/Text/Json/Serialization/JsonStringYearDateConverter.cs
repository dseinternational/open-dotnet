// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonStringYearDateConverter : JsonConverter<YearDate>
{
    public static readonly JsonStringYearDateConverter Default = new();

    public override YearDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var y = reader.GetInt32();
            return new(y);
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var d = reader.GetString();
            if (d!.Length == 0)
            {
                return YearDate.Empty;
            }

            if (YearDate.TryParse(d, CultureInfo.InvariantCulture, out var yearDate))
            {
                return yearDate;
            }
        }

        throw new JsonException($"Invalid {nameof(YearDate)} value.");
    }

    public override void Write(Utf8JsonWriter writer, YearDate value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);

        if (value.HasYearOnly)
        {
            writer.WriteNumberValue(value.Year);
        }
        else
        {
            writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
        }
    }
}
