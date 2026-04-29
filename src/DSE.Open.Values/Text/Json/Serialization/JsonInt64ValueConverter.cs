// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads and writes <typeparamref name="TValue"/> as a JSON number
/// using its underlying <see cref="long"/> representation.
/// </summary>
public class JsonInt64ValueConverter<TValue> : JsonConverter<TValue>
    where TValue : struct, IValue<TValue, long>
{
    /// <inheritdoc/>
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TValue)reader.GetInt64();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue((long)value);
    }
}
