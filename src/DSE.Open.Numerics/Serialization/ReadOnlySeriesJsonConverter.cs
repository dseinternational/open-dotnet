// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public sealed class ReadOnlySeriesJsonConverter : JsonConverter<ReadOnlySeries>
{
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(ReadOnlySeries));
    }

    public override ReadOnlySeries? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return SeriesJsonReader.Read(ref reader, options)?.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlySeries value, JsonSerializerOptions options)
    {
        Debug.Assert(value is not null);
        SeriesJsonWriter.Write(writer, value, options);
    }
}
