// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public class JsonUInt64ValueConverter<TValue> : JsonConverter<TValue>
    where TValue : struct, IValue<TValue, ulong>
{
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TValue)reader.GetUInt64();
    }

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue((ulong)value);
    }

    public override TValue ReadAsPropertyName(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert is not null);

        var name = reader.GetString();

        if (name is null)
        {
            throw new JsonException("Expected a number as property name.");
        }

        if (!ulong.TryParse(name, out var value))
        {
            throw new JsonException($"Invalid property name '{name}' for type {typeToConvert.FullName}.");
        }

        return (TValue)value;
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        Span<char> buffer = stackalloc char[20];

        var result = ((ulong)value).TryFormat(buffer, out var charsWritten, CultureInfo.InvariantCulture);
        Debug.Assert(result);

        writer.WritePropertyName(buffer[..charsWritten]);
    }
}
