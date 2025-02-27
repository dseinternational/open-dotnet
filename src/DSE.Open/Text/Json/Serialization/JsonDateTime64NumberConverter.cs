// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public class JsonDateTime64NumberConverter : JsonConverter<DateTime64>
{
    public override DateTime64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();

        if (value is < DateTime64.MinMilliseconds or > DateTime64.MaxMilliseconds)
        {
            throw new JsonException($"The value is not valid for a {nameof(DateTime64)}.");
        }

        return new DateTime64(value);
    }

    public override void Write(Utf8JsonWriter writer, DateTime64 value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue(value.TotalMilliseconds);
    }
}
