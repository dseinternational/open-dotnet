// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public class DataPointArrayJsonConverter<TX, TY> : JsonConverter<DataPoint<TX, TY>>
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    public override DataPoint<TX, TY> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected the start of an array.");
        }

        _ = reader.Read();

        if (!reader.TryGetNumber<TX>(out var x))
        {
            throw new JsonException("Failed to read x value.");
        }

        _ = reader.Read();

        if (!reader.TryGetNumber<TY>(out var y))
        {
            throw new JsonException("Failed to read y value.");
        }

        return new DataPoint<TX, TY>(x, y);
    }

    public override void Write(Utf8JsonWriter writer, DataPoint<TX, TY> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartArray();
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteEndArray();
    }
}
