// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads and writes <typeparamref name="TValue"/> as a JSON number
/// using its underlying <see cref="decimal"/> representation.
/// </summary>
public class JsonDecimalValueConverter<TValue> : JsonConverter<TValue>
    where TValue : struct, IValue<TValue, decimal>
{
    /// <inheritdoc/>
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TValue)reader.GetDecimal();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteNumberValue((decimal)value);
    }
}
