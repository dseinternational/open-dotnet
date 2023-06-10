// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public sealed class JsonStringBinaryValueBase64Converter : JsonConverter<BinaryValue>
{
    public static readonly JsonStringBinaryValueBase64Converter Default = new();

    public override BinaryValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            if (value != null)
            {
                return value.Length == 0 ? BinaryValue.Empty : BinaryValue.FromBase64EncodedString(value);
            }
        }

        throw new JsonException("Expected BinaryValue encoded as Base64 string.");
    }

    public override void Write(Utf8JsonWriter writer, BinaryValue value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);

        if (value == BinaryValue.Empty)
        {
            writer.WriteStringValue(string.Empty);
        }
        else
        {
            writer.WriteStringValue(value.ToBase64EncodedString());
        }
    }
}
