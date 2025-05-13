// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class ReadOnlyDataFrameJsonConverter : JsonConverter<ReadOnlyDataFrame>
{
    public static readonly ReadOnlyDataFrameJsonConverter Default = new();

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(ReadOnlyDataFrame));
    }

    public override ReadOnlyDataFrame Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DataFrameJsonConverter.Default.Read(ref reader, typeToConvert, options).AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlyDataFrame value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        if (value.Name is not null)
        {
            writer.WriteString(DataFrameJsonPropertyNames.Name, value.Name);
        }

        writer.WritePropertyName(DataFrameJsonPropertyNames.Columns);

        writer.WriteStartArray();

        foreach (var column in value)
        {
            ReadOnlyVectorJsonConverter.Default.Write(writer, column, options);
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
