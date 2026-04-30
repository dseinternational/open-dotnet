// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads and writes <see cref="DateTime64"/> values
/// as JSON numbers representing the total number of milliseconds.
/// </summary>
public class JsonDateTime64NumberConverter : JsonConverter<DateTime64>
{
    /// <inheritdoc/>
    public override DateTime64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();

        if (value is < DateTime64.MinMilliseconds or > DateTime64.MaxMilliseconds)
        {
            throw new JsonException($"The value is not valid for a {nameof(DateTime64)}.");
        }

        return new DateTime64(value);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime64 value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue(value.TotalMilliseconds);
    }
}
