// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class ReadOnlyDataSetJsonConverter : JsonConverter<ReadOnlyDataSet>
{
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(ReadOnlyDataSet));
    }

    public override ReadOnlyDataSet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DataSetJsonConverter.Default.Read(ref reader, typeToConvert, options).AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlyDataSet value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        if (value.Name is not null)
        {
            writer.WriteString(DataSetJsonPropertyNames.Name, value.Name);
        }

        writer.WritePropertyName(DataSetJsonPropertyNames.Frames);

        writer.WriteStartArray();

        foreach (var frame in value)
        {
            ReadOnlyDataFrameJsonConverter.Default.Write(writer, frame, options);
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
