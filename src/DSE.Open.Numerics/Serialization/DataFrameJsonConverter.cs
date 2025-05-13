// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Serialization;

public class DataFrameJsonConverter : JsonConverter<IDataFrame>
{
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(IDataFrame));
    }

    public override IDataFrame Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? name = null;
        Collection<Vector> columns = [];

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

            if (propertyName == DataFrameJsonPropertyNames.Name)
            {
                _ = reader.Read();
                name = reader.GetString();
            }
            else if (propertyName == DataFrameJsonPropertyNames.Columns)
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

                    var vector = VectorJsonConverter.Default.Read(ref reader, typeof(Vector), options);

                    if (vector is null)
                    {
                        throw new JsonException("Expected vector");
                    }

                    columns.Add(vector);
                }
            }
        }

        return new DataFrame(columns, name, null); // TODO: add column names
    }

    public override void Write(Utf8JsonWriter writer, IDataFrame value, JsonSerializerOptions options)
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
            VectorJsonConverter.Default.Write(writer, column, options);
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
