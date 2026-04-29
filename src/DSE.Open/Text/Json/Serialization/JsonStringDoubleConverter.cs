// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that reads <see cref="double"/> values from JSON strings or numbers
/// and writes them as JSON strings using invariant culture.
/// </summary>
public class JsonStringDoubleConverter : JsonConverter<double>
{
    /// <summary>
    /// The default instance of the converter.
    /// </summary>
    public static readonly JsonStringDoubleConverter Default = new();

    /// <inheritdoc/>
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            if (Utf8Parser.TryParse(span, out double number, out var bytesConsumed) && span.Length == bytesConsumed)
            {
                return number;
            }

            if (double.TryParse(reader.GetString(), out number))
            {
                return number;
            }
        }

        return reader.GetSingle();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToStringInvariant());
    }
}
