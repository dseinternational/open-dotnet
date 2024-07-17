// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringBinaryValueBase64Converter : JsonConverter<BinaryValue>
{
    public static readonly JsonStringBinaryValueBase64Converter Default = new();

    public override BinaryValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected BinaryValue encoded as Base64 string.");
        }

        var isEmpty = reader.HasValueSequence
            ? reader.ValueSequence.IsEmpty
            : reader.ValueSpan.IsEmpty;

        if (isEmpty)
        {
            return BinaryValue.Empty;
        }

        var bytes = reader.GetBytesFromBase64();
        return BinaryValue.CreateUnsafe(bytes);
    }

    public override void Write(Utf8JsonWriter writer, BinaryValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value == BinaryValue.Empty)
        {
            writer.WriteStringValue(string.Empty);
        }
        else
        {
            writer.WriteBase64StringValue(value.AsSpan());
        }
    }
}
