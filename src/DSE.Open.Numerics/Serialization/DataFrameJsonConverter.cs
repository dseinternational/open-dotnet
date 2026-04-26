// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON converter for <see cref="DataFrame"/>. Reads frames as
/// <c>{ "name": ..., "columns": [ Series, Series, ... ] }</c> using
/// <see cref="SeriesJsonConverter"/> for each column; writes via
/// <see cref="DataFrameJsonWriter"/>.
/// </summary>
public class DataFrameJsonConverter : JsonConverter<DataFrame>
{
    /// <summary>The default instance used by the source-generated JSON pipeline.</summary>
    public static readonly DataFrameJsonConverter Default = new();

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(DataFrame));
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DataFrame value, JsonSerializerOptions options)
    {
        DataFrameJsonWriter.Write(writer, value, options);
    }
}
