// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class VectorJsonReader
{
    public static NumericVector<T> ReadNumericVector<T>(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
        where T : struct, System.Numerics.INumber<T>
    {
        if (length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return NumericVector<T>.Empty;
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

                // if rented buffer (length < 0), ToMemory() copies to new array of correct length,
                // otherwise the Memory<T> simply references the owned buffer so no copying here
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

    public static Vector<string> ReadStringVector(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
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

    public static Vector<string?> ReadNullableStringVector(ref Utf8JsonReader reader, int length, VectorJsonFormat format = default)
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
}
