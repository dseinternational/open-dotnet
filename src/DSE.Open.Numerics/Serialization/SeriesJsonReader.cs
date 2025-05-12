// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Memory;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public static class SeriesJsonReader
{
    public static Series<T> ReadVector<T>(
        ref Utf8JsonReader reader,
        int length,
        SeriesJsonFormat format = default)
        where T : struct, INumber<T>
    {
        if (length == 0)
        {
            return new Series<T>(default);
        }

        using var builder = length > -1
            ? new ArrayBuilder<T>(length, rentFromPool: false)
            : new ArrayBuilder<T>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (length > -1 && builder.Count != length)
                {
                    throw new JsonException();
                }

                return Series.Create(builder.ToArray());
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetNumber(out T number))
                {
                    builder.Add(number);
                }
            }
        }

        throw new JsonException();
    }

    public static Series<T> ReadCategoryVector<T>(
        ref Utf8JsonReader reader,
        int length,
        Dictionary<string, T> categories,
        SeriesJsonFormat format = default)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        if (length == 0)
        {
            return new Series<T>(default);
        }

        using var builder = length > -1
            ? new ArrayBuilder<T>(length, rentFromPool: false)
            : new ArrayBuilder<T>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (length > -1 && builder.Count != length)
                {
                    throw new JsonException();
                }

                return new Series<T>(builder.ToArray(), categories: categories);
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetNumber(out T number))
                {
                    builder.Add(number);
                }
            }
        }

        throw new JsonException();
    }

    public static Series<string> ReadStringVector(
        ref Utf8JsonReader reader,
        int length,
        SeriesJsonFormat format = default)
    {
        if (length == 0)
        {
            return Series<string>.Empty;
        }

        using var builder = length > -1
            ? new ArrayBuilder<string>(length, rentFromPool: false)
            : new ArrayBuilder<string>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (length > -1 && builder.Count != length)
                {
                    throw new JsonException();
                }

                return Series.Create(builder.ToArray());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                builder.Add(reader.GetString() ?? string.Empty);
            }
        }

        throw new JsonException();
    }
}
