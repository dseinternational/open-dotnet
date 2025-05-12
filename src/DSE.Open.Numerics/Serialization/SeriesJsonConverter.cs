// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Data;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class SeriesJsonConverter : JsonConverter<Vector>
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
        return typeToConvert.IsAssignableTo(typeof(Vector));
    }

    public override Vector? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            var propertyName = reader.GetString();

            if (propertyName == VectorJsonPropertyNames.Name)
            {
                _ = reader.Read();
                name = reader.GetString();
            }
            else if (propertyName == VectorJsonPropertyNames.Annotations)
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

                            var value = ReadValue(ref reader);

                            annotations.Add(key, value);
                        }
                    }
                }
            }
            else if (propertyName == VectorJsonPropertyNames.Data)
            {
                _ = reader.Read();
                data = VectorJsonConverter.Default.Read(ref reader, typeof(Vector), options);
            }
        }

        if (data is null)
        {
            throw new JsonException("Data must be specified");
        }

        return data;
    }

    private static Variant ReadValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => Variant.Null,
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt64(out var integer) => integer,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String => (Variant)(reader.GetString() ?? string.Empty),
            _ => throw new JsonException("Unable to read Variant value.")
        };
    }

    public override void Write(Utf8JsonWriter writer, Vector value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        if (value.Name is not null)
        {
            writer.WriteString(VectorJsonPropertyNames.Name, value.Name);
        }

        if (value.Annotations is not null && value.Annotations.Count > 0)
        {
            writer.WriteStartObject(VectorJsonPropertyNames.Annotations);

            foreach (var (key, annotation) in value.Annotations)
            {
                writer.WritePropertyName(key);
                JsonVariantConverter.Default.Write(writer, annotation, options);
            }

            writer.WriteEndObject();
        }

        writer.WritePropertyName(VectorJsonPropertyNames.Data);

        VectorJsonConverter.Default.Write(writer, value, options);

        writer.WriteEndObject();
    }
}
