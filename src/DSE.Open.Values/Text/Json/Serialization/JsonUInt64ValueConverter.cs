// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads and writes <typeparamref name="TValue"/> as a JSON number
/// using its underlying <see cref="ulong"/> representation, and supports use as a JSON property name.
/// </summary>
public class JsonUInt64ValueConverter<TValue> : JsonConverter<TValue>
    where TValue : struct, IValue<TValue, ulong>
{
    /// <inheritdoc/>
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TValue)reader.GetUInt64();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue((ulong)value);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        Span<char> buffer = stackalloc char[20];

        var result = ((ulong)value).TryFormat(buffer, out var charsWritten, CultureInfo.InvariantCulture);
        Debug.Assert(result);

        writer.WritePropertyName(buffer[..charsWritten]);
    }
}
