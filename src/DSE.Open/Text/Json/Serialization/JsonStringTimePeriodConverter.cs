// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Time;

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringTimePeriodConverter : JsonConverter<TimePeriod>
{
    public static readonly JsonStringTimePeriodConverter Default = new();

    public override TimePeriod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            return TimePeriod.TryParse(value, out var timePeriod) ? timePeriod : throw new JsonException("Invalid TimePeriod value: " + value);
        }

        throw new JsonException("Invalid TimePeriod value.");
    }

    public override void Write(Utf8JsonWriter writer, TimePeriod value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);
        writer.WriteStringValue(value.ToString());
    }
}
