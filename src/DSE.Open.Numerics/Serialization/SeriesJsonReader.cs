// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class SeriesJsonReader
{
    public static Series? Read(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? name = null;
        ICategorySet? categories = null;
        Vector? values = null;

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

            if (propertyName == NumericsPropertyNames.Name)
            {
                _ = reader.Read();
                name = reader.GetString();
            }
            else if (propertyName == NumericsPropertyNames.Categories)
            {
                _ = reader.Read();
                categories = CategorySetJsonReader.Read(ref reader);
            }
            else if (propertyName == NumericsPropertyNames.Vector)
            {
                _ = reader.Read();
                values = VectorJsonReader.ReadVector(ref reader, VectorJsonFormat.Default);
            }
            else
            {
                throw new JsonException($"Unexpected property name: {propertyName}");
            }
        }

        if (values is null)
        {
            throw new JsonException("Cannot read series without values");
        }

        if (categories is not null)
        {
           // todo  return CategoricalSeries.CreateUntyped(values, name, null, categories, null);
        }

        return Series.CreateUntyped(name, values, null);
    }
}
