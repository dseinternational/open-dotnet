// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Memory;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public class SeriesJsonConverter : JsonConverter<Series>
{
    public static SeriesJsonConverter Default { get; } = new();

    public SeriesJsonConverter() : this(default)
    {
    }

    public SeriesJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    public VectorJsonFormat Format { get; }

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(Series));
    }

    public override Series? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        Series? series = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }
        }

        // SDebug.Assert(vector is not null);

        return series;
    }

    public override void Write(Utf8JsonWriter writer, Series value, JsonSerializerOptions options)
    {
        SeriesJsonWriter.Write(writer, value, options);
    }
}

internal static class SeriesJsonWriter
{
    public static void Write(Utf8JsonWriter writer, IReadOnlySeries series, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(series);

        writer.WriteStartObject();

        if (series.Name is not null)
        {
            writer.WriteString(SeriesJsonPropertyNames.Name, series.Name);
        }

        writer.WritePropertyName(SeriesJsonPropertyNames.Data);

        VectorJsonWriter.Write(writer, series.Data, options);

        writer.WriteEndObject();
    }
}
