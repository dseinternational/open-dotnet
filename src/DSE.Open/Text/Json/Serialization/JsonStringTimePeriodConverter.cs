// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Time;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads and writes <see cref="TimePeriod"/> values as JSON strings.
/// </summary>
public class JsonStringTimePeriodConverter : JsonConverter<TimePeriod>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringTimePeriodConverter Default = new();

    /// <inheritdoc/>
    public override TimePeriod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            return TimePeriod.TryParse(value, out var timePeriod) ? timePeriod : throw new JsonException("Invalid TimePeriod value: " + value);
        }

        throw new JsonException("Invalid TimePeriod value.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TimePeriod value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToString());
    }
}
