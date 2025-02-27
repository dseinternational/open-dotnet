// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class VectorCharJsonConverter : JsonConverter<Vector<char>>
{
    public override Vector<char> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected the start of an array.");
        }

        List<char> result = [];

        while (!reader.Read())
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected string token.");
            }

            var value = reader.GetString();

            if (value is null)
            {
                throw new JsonException("Expected non-null string.");
            }

            if (value.Length != 1)
            {
                throw new JsonException("Expected single character string.");
            }

            result.Add(value[0]);
        }

        return new Vector<char>([.. result]);
    }

    public override void Write(Utf8JsonWriter writer, Vector<char> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartArray();

        foreach (var e in value)
        {
            writer.WriteStringValue(e.ToString());
        }

        writer.WriteEndArray();
    }
}
