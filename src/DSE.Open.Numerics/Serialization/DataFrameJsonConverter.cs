// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Serialization;

public class DataFrameJsonConverter : JsonConverter<DataFrame>
{
    public static readonly DataFrameJsonConverter Default = new();

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(DataFrame));
    }

    public override DataFrame Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? name = null;
        Collection<Series> columns = [];

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

            if (propertyName == NumericsPropertyNames.Name)
            {
                _ = reader.Read();
                name = reader.GetString();
            }
            else if (propertyName == NumericsPropertyNames.Columns)
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

                    var vector = SeriesJsonConverter.Default.Read(ref reader, typeof(Series), options);

                    if (vector is null)
                    {
                        throw new JsonException("Expected vector");
                    }

                    columns.Add(vector);
                }
            }
        }

        return new DataFrame(columns, name);
    }

    public override void Write(Utf8JsonWriter writer, DataFrame value, JsonSerializerOptions options)
    {
        DataFrameJsonWriter.Write(writer, value, options);
    }
}
