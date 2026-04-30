// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads <see cref="decimal"/> values from JSON strings or numbers
/// and writes them as JSON strings using invariant culture.
/// </summary>
public class JsonStringDecimalConverter : JsonConverter<decimal>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringDecimalConverter Default = new();

    /// <inheritdoc/>
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            if (Utf8Parser.TryParse(span, out decimal number, out var bytesConsumed) && span.Length == bytesConsumed)
            {
                return number;
            }

            if (decimal.TryParse(reader.GetString(), out number))
            {
                return number;
            }
        }

        return reader.GetDecimal();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToStringInvariant());
    }
}
