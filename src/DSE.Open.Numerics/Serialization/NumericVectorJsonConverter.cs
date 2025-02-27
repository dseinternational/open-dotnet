// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class NumericVectorJsonConverter<T> : JsonConverter<NumericVector<T>>
    where T : struct, INumber<T>
{
    public override NumericVector<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.TryGetTypeInfo(typeof(Memory<T>), out var typeInfo)
            && typeInfo.Converter is JsonConverter<Memory<T>> jsonConverter)
        {
            var data = jsonConverter.Read(ref reader, typeof(Memory<T>), options);
            return new NumericVector<T>(data);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, NumericVector<T> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.TryGetTypeInfo(typeof(Memory<T>), out var typeInfo)
            && typeInfo.Converter is JsonConverter<Memory<T>> jsonConverter)
        {
            jsonConverter.Write(writer, value, options);
        }
        else
        {
            throw new JsonException();
        }
    }
}
