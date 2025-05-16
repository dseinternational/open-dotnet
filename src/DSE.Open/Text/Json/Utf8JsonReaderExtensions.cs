// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Memory;

namespace DSE.Open.Text.Json;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public static class Utf8JsonReaderExtensions
{
    public static bool TryGetNumber<T>(this ref Utf8JsonReader reader, out T value)
        where T : struct, INumber<T>
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out var int64Value))
            {
                value = T.CreateChecked(int64Value);
                return true;
            }

            if (reader.TryGetUInt64(out var uint64Value))
            {
                value = T.CreateChecked(uint64Value);
                return true;
            }

            if (reader.TryGetDouble(out var doubleValue))
            {
                value = T.CreateChecked(doubleValue);
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Reads the contents of a JSON array into a <see cref="Memory{T}"/>.
    /// Starting location is expected to be <see cref="JsonTokenType.StartArray"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader"></param>
    /// <param name="length">The lenght of the array, if known. A negative value if unknown.</param>
    /// <param name="valueReader"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static T[] ReadArray<T>(
        this ref Utf8JsonReader reader,
        int length,
        JsonValueReader<T> valueReader)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(valueReader);

        if (length == 0)
        {
            return [];
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

                return builder.ToArray();
            }

            if (reader.TokenType is JsonTokenType.Number
                or JsonTokenType.String
                or JsonTokenType.True
                or JsonTokenType.False)
            {
                builder.Add(valueReader(ref reader));
            }
        }

        throw new JsonException("Expected end of array");
    }

    /// <summary>
    /// Reads the contents of a JSON array into a <see cref="Memory{T}"/>.
    /// Starting location is expected to be <see cref="JsonTokenType.StartArray"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader"></param>
    /// <param name="length">The lenght of the array, if known. A negative value if unknown.</param>
    /// <param name="valueReader"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static Memory<T> ReadArrayAsMemory<T>(
        this ref Utf8JsonReader reader,
        int length,
        JsonValueReader<T> valueReader)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(valueReader);

        if (length == 0)
        {
            return default;
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

                return builder.ToMemory();
            }

            if (reader.TokenType is JsonTokenType.Number
                or JsonTokenType.String
                or JsonTokenType.True
                or JsonTokenType.False)
            {
                builder.Add(valueReader(ref reader));
            }
        }

        throw new JsonException("Expected end of array");
    }

    public static T[] ReadNumberArray<T>(
        this ref Utf8JsonReader reader,
        int length)
        where T : struct, INumber<T>
    {
        return reader.ReadArray(length, (ref r) =>
        {
            if (r.TryGetNumber<T>(out var num))
            {
                return num;
            }
            else
            {
                throw new JsonException("Expected number value");
            }
        });
    }

    public static Memory<T> ReadNumberArrayAsMemory<T>(
        this ref Utf8JsonReader reader,
        int length)
        where T : struct, INumber<T>
    {
        return reader.ReadArrayAsMemory(length, (ref r) =>
        {
            if (r.TryGetNumber<T>(out var num))
            {
                return num;
            }
            else
            {
                throw new JsonException("Expected number value");
            }
        });
    }
}
