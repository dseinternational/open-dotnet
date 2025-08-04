// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringBinaryValueHexConverter : JsonConverter<BinaryValue>
{
    public static readonly JsonStringBinaryValueHexConverter Default = new();

    public override BinaryValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected BinaryValue encoded as Hex string.");
        }

        var isEmpty = reader.HasValueSequence
            ? reader.ValueSequence.IsEmpty
            : reader.ValueSpan.IsEmpty;

        if (isEmpty)
        {
            return BinaryValue.Empty;
        }

        var bytes = Convert.FromHexString(reader.GetString()!);
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
            writer.WriteStringValue(Convert.ToHexStringLower(value.AsSpan()));
        }
    }
}
