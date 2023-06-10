// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public class JsonStringDecimalConverter : JsonConverter<decimal>
{
    public static readonly JsonStringDecimalConverter Default = new();

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

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);
        writer.WriteStringValue(value.ToStringInvariant());
    }
}
