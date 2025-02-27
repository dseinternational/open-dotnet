// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class VectorJsonConverter<T> : JsonConverter<Vector<T>>
    where T : notnull
{
    public override Vector<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (typeof(T) == typeof(int) && options.TryGetTypeInfo(typeof(int[]), out var intArrayTypeInfo)
            && intArrayTypeInfo.Converter is JsonConverter<int[]> jsonConverter)
        {
            var data = jsonConverter.Read(ref reader, typeToConvert, options);
            return new Vector<T>(data ?? []);
        }

        if (options.TryGetTypeInfo(typeof(T[]), out var typeInfo)
            && typeInfo.Converter is JsonConverter<T[]> jsonConverter)
        {
            var data = jsonConverter.Read(ref reader, typeToConvert, options);
            return new Vector<T>(data ?? []);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Vector<T> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
public class VectorStringJsonConverter : JsonConverter<Vector<string>>
{
    public override Vector<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected the start of an array.");
        }

        List<string> result = [];

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

            result.Add(value);
        }

        return new Vector<string>([.. result]);
    }

    public override void Write(Utf8JsonWriter writer, Vector<string> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartArray();

        foreach (var e in value)
        {
            writer.WriteStringValue(e);
        }

        writer.WriteEndArray();
    }
}
