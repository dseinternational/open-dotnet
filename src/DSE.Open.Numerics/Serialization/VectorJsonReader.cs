// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using DSE.Open.Memory;
using DSE.Open.Text.Json;

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

            if (propertyName == NumericsPropertyNames.DataType)
            {
                _ = reader.Read();
                dataType = reader.GetString();

                if (dataType is null)
                {
                    throw new JsonException("Data type must be specified");
                }
            }
            else if (propertyName == NumericsPropertyNames.Length)
            {
                _ = reader.Read();
                length = reader.GetInt32();

                if (length < 0)
                {
                    throw new JsonException("Length must be greater than or equal to zero");
                }

                if (length > VectorJsonConstants.MaximumSerializedLength)
                {
                    throw new JsonException("Length must be less than or equal to "
                        + VectorJsonConstants.MaximumSerializedLength);
                }
            }
            else if (propertyName == NumericsPropertyNames.Values)
            {
                if (dataType is null)
                {
                    throw new JsonException("Cannot read vector without data type");
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
                    VectorDataType.DateTime => ReadDateTimeVector(ref reader, length, format),
                    VectorDataType.DateTimeOffset => ReadDateTimeOffsetVector(ref reader, length, format),
                    VectorDataType.Uuid => ReadGuidVector(ref reader, length, format),
                    VectorDataType.Bool => ReadBooleanVector(ref reader, length),
                    VectorDataType.Char => ReadCharVector(ref reader, length, format),
                    VectorDataType.String => ReadStringVector(ref reader, length),
                    _ => throw new JsonException($"Unsupported data type: {dtype}")
                };
            }
            else
            {
                throw new JsonException($"Unexpected property name: {propertyName}");
            }
        }

        Debug.Assert(vector is not null);

        return vector;
    }

    internal static Vector<T> ReadVector<T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format,
        JsonValueReader<T> valueReader)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(valueReader);

        if (format != VectorJsonFormat.Default)
        {
            throw new NotSupportedException($"Unsupported format: {format}");
        }

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

                return Vector.Create(builder.ToMemory());
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

    public static Vector<T> ReadNumberVector<T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : struct, INumber<T>
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            if (r.TryGetNumber<T>(out var num))
            {
                return num;
            }

            throw new JsonException("Expected number value");
        });
    }

    public static Vector<string> ReadStringVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => r.GetString() ?? throw new JsonException());
    }

    public static Vector<DateTime> ReadDateTimeVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => r.GetDateTime());
    }

    public static Vector<DateTimeOffset> ReadDateTimeOffsetVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => r.GetDateTimeOffset());
    }

    public static Vector<Guid> ReadGuidVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => r.GetGuid());
    }

    public static Vector<char> ReadCharVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            var s = r.GetString();

            if (s is null || s.Length != 1)
            {
                throw new JsonException("Expected non-null string with single character.");
            }

            return s[0];
        });
    }

    public static Vector<bool> ReadBooleanVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            if (r.TokenType == JsonTokenType.True)
            {
                return true;
            }

            if (r.TokenType == JsonTokenType.False)
            {
                return false;
            }

            throw new JsonException("Expected boolean value");
        });
    }
}
