// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class ReadOnlyVectorJsonReader
{
    public static ReadOnlyNumericVector<T> ReadNumericVector<T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : struct, INumber<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyNumericVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
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

                return Vector.CreateNumeric(builder.ToMemory());
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

    public static ReadOnlyCategoricalVector<T> ReadCategoryVector<T>(
        ref Utf8JsonReader reader,
        int length,
        Memory<KeyValuePair<string, T>> categories,
        VectorJsonFormat format = default)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyCategoricalVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
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

                return new ReadOnlyCategoricalVector<T>(builder.ToMemory(), categories);
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

    public static ReadOnlyVector<string> ReadStringVector(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<string>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
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

                return ReadOnlyVector.Create(builder.ToReadOnlyMemory());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                builder.Add(reader.GetString() ?? string.Empty);
            }
        }

        throw new JsonException();
    }

    public static ReadOnlyVector<string?> ReadNullableStringVector(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<string?>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        using var builder = length > -1
            ? new ArrayBuilder<string?>(length, rentFromPool: false)
            : new ArrayBuilder<string?>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (length > -1 && builder.Count != length)
                {
                    throw new JsonException();
                }

                return ReadOnlyVector.Create(builder.ToReadOnlyMemory());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                builder.Add(reader.GetString());
            }
        }

        throw new JsonException();
    }

    public static ReadOnlyVector<T> ReadSpanParseableVector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
        where T : notnull, ISpanParsable<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
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

                return ReadOnlyVector.Create(builder.ToReadOnlyMemory());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();

                if (!T.TryParse(value, CultureInfo.InvariantCulture, out var result))
                {
                    throw new JsonException($"Failed to convert value: \"{value}\" to {typeof(T).Name}");
                }

                builder.Add(result);
            }
        }

        return ReadOnlyVector.Create(builder.ToReadOnlyMemory());
    }

    public static ReadOnlyVector<T?> ReadNullableSpanParseableVector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
        where T : ISpanParsable<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T?>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        using var builder = length > -1
            ? new ArrayBuilder<T?>(length, rentFromPool: false)
            : new ArrayBuilder<T?>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (length > -1 && builder.Count != length)
                {
                    throw new JsonException();
                }

                return ReadOnlyVector.Create(builder.ToReadOnlyMemory());
            }

            if (reader.TokenType == JsonTokenType.Null)
            {
                builder.Add(default);
                continue;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();

                if (value is null)
                {
                    builder.Add(default);
                    continue;
                }

                if (!T.TryParse(value, CultureInfo.InvariantCulture, out var result))
                {
                    throw new JsonException($"Failed to convert value: \"{value}\" to {typeof(T).Name}");
                }

                builder.Add(result);
            }
        }

        return ReadOnlyVector.Create(builder.ToReadOnlyMemory());
    }
}
