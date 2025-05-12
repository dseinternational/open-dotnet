// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;
using DSE.Open.Memory;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public static class VectorJsonReader
{
    public static Vector<T> ReadVector<T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : struct, INumber<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<T>.Empty;
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

    public static Vector<string> ReadStringVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<string>.Empty;
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

                return Vector.Create(builder.ToMemory());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                builder.Add(reader.GetString() ?? string.Empty);
            }
        }

        throw new JsonException();
    }

    public static Vector<string?> ReadNullableStringVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<string?>.Empty;
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

                return Vector.Create(builder.ToMemory());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                builder.Add(reader.GetString());
            }
        }

        throw new JsonException();
    }

    public static Vector<T> ReadSpanParseableVector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : notnull, ISpanParsable<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<T>.Empty;
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

                return Vector.Create(builder.ToMemory());
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

        return Vector.Create(builder.ToMemory());
    }

    public static Vector<T?> ReadNullableSpanParseableVector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : ISpanParsable<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<T?>.Empty;
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

                return Vector.Create(builder.ToMemory());
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

        return Vector.Create(builder.ToMemory());
    }
}
