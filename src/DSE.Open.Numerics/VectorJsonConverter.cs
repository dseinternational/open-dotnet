// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public class VectorJsonConverter<T> : JsonConverter<Vector<T>>
    where T : struct, INumber<T>
{
    public override Vector<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected the start of an array.");
        }

        var list = new List<T>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var value))
                {
                    list.Add(T.CreateChecked(value));
                }
                else
                {
                    list.Add(T.CreateChecked(reader.GetDouble()));
                }
            }
            else
            {
                throw new JsonException("Expected number.");
            }
        }

        return new Vector<T>([.. list]);
    }

    public override void Write(Utf8JsonWriter writer, Vector<T> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartArray();

        foreach (var e in value)
        {
            if (T.IsInteger(e))
            {
                writer.WriteNumberValue(long.CreateChecked(e));
            }
            else
            {
                writer.WriteNumberValue(double.CreateChecked(e));
            }
        }

        writer.WriteEndArray();
    }
}
