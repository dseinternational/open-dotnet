// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public class JsonDate64NumberConverter : JsonConverter<Date64>
{
    public override Date64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();

        if (value is < Date64.MinMilliseconds or > Date64.MaxMilliseconds)
        {
            throw new JsonException($"The value is not valid for a {nameof(Date64)}.");
        }

        return new Date64(value);
    }

    public override void Write(Utf8JsonWriter writer, Date64 value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue(value.TotalMilliseconds);
    }
}
