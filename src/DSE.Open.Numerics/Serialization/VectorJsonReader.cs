// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using DSE.Open.Memory;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public static class VectorJsonReader
{
    public static Vector? ReadVector(ref Utf8JsonReader reader, VectorJsonFormat format)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? dataType = null;
        var length = -1;
        Vector? vector = null;

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

            if (propertyName == VectorJsonPropertyNames.DataType)
            {
                _ = reader.Read();
                dataType = reader.GetString();

                if (dataType is null)
                {
                    throw new JsonException("Data type must be specified");
                }
            }
            else if (propertyName == VectorJsonPropertyNames.Length)
            {
                _ = reader.Read();
                length = reader.GetInt32();

                if (length < 0)
                {
                    throw new JsonException("Length must be greater than or equal to zero");
                }

                if (length > Array.MaxLength)
                {
                    throw new JsonException("Length must be less than or equal to " + Array.MaxLength);
                }
            }
            else if (propertyName == VectorJsonPropertyNames.Values)
            {
                if (dataType is null)
                {
                    throw new JsonException("Cannot read values without data type");
                }

                var dtype = VectorDataTypeHelper.GetVectorDataType(dataType);

                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected start of array");
                }

                vector = dtype switch
                {
                    VectorDataType.Float64 => ReadNumberVector<double>(ref reader, length, format),
                    VectorDataType.Float32 => ReadNumberVector<float>(ref reader, length, format),
                    VectorDataType.Int64 => ReadNumberVector<long>(ref reader, length, format),
                    VectorDataType.UInt64 => ReadNumberVector<ulong>(ref reader, length, format),
                    VectorDataType.Int32 => ReadNumberVector<int>(ref reader, length, format),
                    VectorDataType.UInt32 => ReadNumberVector<uint>(ref reader, length, format),
                    VectorDataType.Int16 => ReadNumberVector<short>(ref reader, length, format),
                    VectorDataType.UInt16 => ReadNumberVector<ushort>(ref reader, length, format),
                    VectorDataType.Int8 => ReadNumberVector<sbyte>(ref reader, length, format),
                    VectorDataType.UInt8 => ReadNumberVector<byte>(ref reader, length, format),
                    VectorDataType.Int128 => ReadNumberVector<Int128>(ref reader, length, format),
                    VectorDataType.UInt128 => ReadNumberVector<UInt128>(ref reader, length, format),
                    VectorDataType.DateTime64 => ReadNumberVector<DateTime64>(ref reader, length, format),
                    VectorDataType.DateTime => throw new NotImplementedException(),
                    VectorDataType.DateTimeOffset => throw new NotImplementedException(),
                    VectorDataType.Uuid => throw new NotImplementedException(),
                    VectorDataType.Bool => ReadBooleanVector(ref reader, length),
                    VectorDataType.Char => throw new NotImplementedException(),
                    VectorDataType.String => ReadStringVector(ref reader, length),
                    _ => throw new JsonException($"Unsupported data type: {dtype}")
                };
            }
        }

        Debug.Assert(vector is not null);

        return vector;
    }

    public static Vector<T> ReadNumberVector<T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : struct, INumber<T>
    {
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

                return Vector.Create(builder.ToArray());
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
            return [];
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

                return Vector.Create(builder.ToArray());
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                builder.Add(reader.GetString() ?? string.Empty);
            }
        }

        throw new JsonException();
    }

    public static Vector<bool> ReadBooleanVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        if (length == 0)
        {
            return [];
        }

        using var builder = length > -1
            ? new ArrayBuilder<bool>(length, rentFromPool: false)
            : new ArrayBuilder<bool>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (length > -1 && builder.Count != length)
                {
                    throw new JsonException();
                }

                return Vector.Create(builder.ToArray());
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                builder.Add(true);
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                builder.Add(false);
            }
        }

        throw new JsonException();
    }
}
