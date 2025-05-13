// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Serialization;

public class DataSetJsonConverter : JsonConverter<DataSet>
{
    public static readonly DataSetJsonConverter Default = new();

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(DataSet));
    }

    public override DataSet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? name = null;
        Collection<DataFrame> dataFrames = [];

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }

            var propertyName = reader.GetString();

            if (propertyName == DataSetJsonPropertyNames.Name)
            {
                _ = reader.Read();
                name = reader.GetString();
            }
            else if (propertyName == DataSetJsonPropertyNames.Frames)
            {
                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected start of array");
                }

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }

                    var dataFrame = DataFrameJsonConverter.Default.Read(ref reader, typeof(Vector), options);

                    if (dataFrame is null)
                    {
                        throw new JsonException("Expected data frame");
                    }

                    dataFrames.Add(dataFrame);
                }
            }
        }

        return new DataSet(dataFrames, name);
    }

    public override void Write(Utf8JsonWriter writer, DataSet value, JsonSerializerOptions options)
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
            DataFrameJsonConverter.Default.Write(writer, frame, options);
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
