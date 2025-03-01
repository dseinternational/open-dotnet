// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Data;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class SeriesJsonConverter : JsonConverter<Series>
{
    public SeriesJsonConverter() : this(VectorJsonFormat.Default)
    {
    }

    public SeriesJsonConverter(VectorJsonFormat vectorFormat)
    {
        VectorFormat = vectorFormat;
    }

    public VectorJsonFormat VectorFormat { get; }

    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        return typeToConvert.IsAssignableTo(typeof(Series));
    }

    public override Series? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? name = null;
        Dictionary<string, Variant>? annotations = null;
        Vector? data = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();

                if (propertyName == SeriesJsonPropertyNames.Name)
                {
                    _ = reader.Read();
                    name = reader.GetString();
                }
                else if (propertyName == SeriesJsonPropertyNames.Annotations)
                {
                    annotations = [];

                    if (reader.Read() && reader.TokenType == JsonTokenType.StartObject)
                    {
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndObject)
                            {
                                break;
                            }

                            if (reader.TokenType == JsonTokenType.PropertyName)
                            {
                                var key = reader.GetString();

                                if (key is null)
                                {
                                    throw new JsonException("Annotation key must be specified");
                                }

                                var value = JsonVariantConverter.Default.Read(ref reader, typeof(Variant), options);

                                annotations.Add(key, value);
                            }
                        }
                    }
                }
                else if (propertyName == SeriesJsonPropertyNames.Data)
                {
                    _ = reader.Read();
                    data = VectorJsonConverter.Default.Read(ref reader, typeof(Vector), options);
                }
            }
        }

        if (data is null)
        {
            throw new JsonException("Data must be specified");
        }

        return Series.Create(name, data, annotations);
    }

    public override void Write(Utf8JsonWriter writer, Series value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        if (value.Name is not null)
        {
            writer.WriteString(SeriesJsonPropertyNames.Name, value.Name);
        }

        if (value.Annotations is not null && value.Annotations.Count > 0)
        {
            writer.WriteStartObject(SeriesJsonPropertyNames.Annotations);

            foreach (var (key, annotation) in value.Annotations)
            {
                writer.WritePropertyName(key);
                JsonVariantConverter.Default.Write(writer, annotation, options);
            }

            writer.WriteEndObject();
        }

        writer.WritePropertyName(SeriesJsonPropertyNames.Data);

        VectorJsonConverter.Default.Write(writer, value.GetData(), options);

        writer.WriteEndObject();
    }
}
