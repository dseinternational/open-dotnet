// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class DataPoint3DArrayJsonConverter<T> : JsonConverter<DataPoint3D<T>>
    where T : struct, INumber<T>
{
    public override DataPoint3D<T> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected the start of an array.");
        }

        _ = reader.Read();

        if (!reader.TryGetNumber<T>(out var x))
        {
            throw new JsonException("Failed to read x value.");
        }

        _ = reader.Read();

        if (!reader.TryGetNumber<T>(out var y))
        {
            throw new JsonException("Failed to read y value.");
        }

        _ = reader.Read();

        if (!reader.TryGetNumber<T>(out var z))
        {
            throw new JsonException("Failed to read z value.");
        }

        _ = reader.Read();

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Expected the end of an array.");
        }

        return new DataPoint3D<T>(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, DataPoint3D<T> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartArray();
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteNumberValue(value.Z);
        writer.WriteEndArray();
    }
}
