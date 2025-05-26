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

                var dtype = Vector.GetDataType(dataType);

                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected start of array");
                }

                vector = dtype switch
                {
                    VectorDataType.Float64 => ReadNumberVector<double>(ref reader, length, format),
                    VectorDataType.Float32 => ReadNumberVector<float>(ref reader, length, format),
                    VectorDataType.Float16 => ReadNumberVector<Half>(ref reader, length, format),
                    VectorDataType.Int64 => ReadNumberVector<long>(ref reader, length, format),
                    VectorDataType.UInt64 => ReadNumberVector<ulong>(ref reader, length, format),
                    VectorDataType.Int32 => ReadNumberVector<int>(ref reader, length, format),
                    VectorDataType.UInt32 => ReadNumberVector<uint>(ref reader, length, format),
                    VectorDataType.Int16 => ReadNumberVector<short>(ref reader, length, format),
                    VectorDataType.UInt16 => ReadNumberVector<ushort>(ref reader, length, format),
                    VectorDataType.Int8 => ReadNumberVector<sbyte>(ref reader, length, format),
                    VectorDataType.UInt8 => ReadNumberVector<byte>(ref reader, length, format),
                    VectorDataType.DateTime64 => ReadNumberVector<DateTime64>(ref reader, length, format),
                    VectorDataType.DateTime => ReadDateTimeVector(ref reader, length, format),
                    VectorDataType.DateTimeOffset => ReadDateTimeOffsetVector(ref reader, length, format),
                    VectorDataType.Bool => ReadBooleanVector(ref reader, length),
                    VectorDataType.Char => ReadCharVector(ref reader, length, format),
                    VectorDataType.String => ReadStringVector(ref reader, length),
                    VectorDataType.NaFloat64 => ReadNaNumberVector<NaFloat<double>, double>(ref reader, length, format),
                    VectorDataType.NaFloat32 => ReadNaNumberVector<NaFloat<float>, float>(ref reader, length, format),
                    VectorDataType.NaFloat16 => ReadNaNumberVector<NaFloat<Half>, Half>(ref reader, length, format),
                    VectorDataType.NaInt64 => ReadNaNumberVector<NaInt<long>, long>(ref reader, length, format),
                    VectorDataType.NaUInt64 => ReadNaNumberVector<NaInt<ulong>, ulong>(ref reader, length, format),
                    VectorDataType.NaInt32 => ReadNaNumberVector<NaInt<int>, int>(ref reader, length, format),
                    VectorDataType.NaUInt32 => ReadNaNumberVector<NaInt<uint>, uint>(ref reader, length, format),
                    VectorDataType.NaInt16 => ReadNaNumberVector<NaInt<short>, short>(ref reader, length, format),
                    VectorDataType.NaUInt16 => ReadNaNumberVector<NaInt<ushort>, ushort>(ref reader, length, format),
                    VectorDataType.NaInt8 => ReadNaNumberVector<NaInt<sbyte>, sbyte>(ref reader, length, format),
                    VectorDataType.NaUInt8 => ReadNaNumberVector<NaInt<byte>, byte>(ref reader, length, format),
                    VectorDataType.NaDateTime64 => ReadNaNumberVector<NaInt<DateTime64>, DateTime64>(ref reader, length, format),
                    VectorDataType.NaDateTime => ReadNaDateTimeVector(ref reader, length, format),
                    VectorDataType.NaDateTimeOffset => ReadNaDateTimeOffsetVector(ref reader, length, format),
                    VectorDataType.NaBool => ReadNaBooleanVector(ref reader, length, format),
                    VectorDataType.NaChar => ReadNaCharVector(ref reader, length, format),
                    VectorDataType.NaString => ReadNaStringVector(ref reader, length, format),
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
                or JsonTokenType.False
                or JsonTokenType.Null)
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

            if (TryGetFloatingPointFromString<T>(ref r, out var value))
            {
                return value;
            }

            throw new JsonException("Expected number value");
        });
    }

    public static Vector<TSelf> ReadNaNumberVector<TSelf, T>(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
        where T : struct, INumber<T>, IMinMaxValue<T>
        where TSelf : struct, INaNumber<TSelf, T>
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            if (r.TokenType == JsonTokenType.Null)
            {
                return TSelf.Na;
            }

            if (r.TryGetNumber<T>(out var num))
            {
                return TSelf.FromValue(num);
            }

            if (TryGetFloatingPointFromString<T>(ref r, out var value))
            {
                return TSelf.FromValue(value);
            }

            if (r.TokenType == JsonTokenType.String)
            {
                var str = r.GetString();

                Debug.Assert(str is not null);

                if (str.Equals(NaValue.NaValueLabel, StringComparison.OrdinalIgnoreCase)
                    || str.Equals(NaValue.NanValueLabel, StringComparison.OrdinalIgnoreCase)
                    || str.Equals(NaValue.NullValueLabel, StringComparison.OrdinalIgnoreCase))
                {
                    return TSelf.Na;
                }

                throw new JsonException($"Invalid string value for number: {str}");
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

    public static Vector<NaValue<string>> ReadNaStringVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => (NaValue<string>)r.GetString());
    }

    public static Vector<DateTime> ReadDateTimeVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => r.GetDateTime());
    }

    public static Vector<NaValue<DateTime>> ReadNaDateTimeVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            if (r.TokenType == JsonTokenType.Null)
            {
                return NaValue<DateTime>.Na;
            }

            return (NaValue<DateTime>)r.GetDateTime();
        });
    }

    public static Vector<DateTimeOffset> ReadDateTimeOffsetVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) => r.GetDateTimeOffset());
    }

    public static Vector<NaValue<DateTimeOffset>> ReadNaDateTimeOffsetVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            if (r.TokenType == JsonTokenType.Null)
            {
                return NaValue<DateTimeOffset>.Na;
            }

            return (NaValue<DateTimeOffset>)r.GetDateTimeOffset();
        });
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

    public static Vector<NaValue<char>> ReadNaCharVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            var s = r.GetString();

            if (s is null)
            {
                return NaValue<char>.Na;
            }

            if (s.Length != 1)
            {
                throw new JsonException("Expected non-null string with single character.");
            }

            return (NaValue<char>)s[0];
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

    public static Vector<NaValue<bool>> ReadNaBooleanVector(
        ref Utf8JsonReader reader,
        int length,
        VectorJsonFormat format = default)
    {
        return ReadVector(ref reader, length, format, (ref r) =>
        {
            if (r.TokenType == JsonTokenType.Null)
            {
                return NaValue<bool>.Na;
            }

            if (r.TokenType == JsonTokenType.True)
            {
                return (NaValue<bool>)true;
            }

            if (r.TokenType == JsonTokenType.False)
            {
                return (NaValue<bool>)false;
            }

            throw new JsonException("Expected boolean value");
        });
    }


    private static bool TryGetFloatingPointFromString<T>(ref Utf8JsonReader r, out T value)
        where T : struct, INumber<T>
    {
        if (NumericsNumberHelper.IsKnownFloatingPointIeee754Type<T>() && r.TokenType == JsonTokenType.String)
        {
            var str = r.GetString();

            if (str is not null)
            {
                if (str.Equals(NaValue.NanValueLabel, StringComparison.OrdinalIgnoreCase)
                    || str.Equals(NaValue.NaValueLabel, StringComparison.OrdinalIgnoreCase))
                {
                    if (typeof(T) == typeof(float))
                    {
                        value = T.CreateChecked(float.NaN);
                        return true;
                    }

                    if (typeof(T) == typeof(double))
                    {
                        value = T.CreateChecked(double.NaN);
                        return true;
                    }

                    if (typeof(T) == typeof(Half))
                    {
                        value = T.CreateChecked(Half.NaN);
                        return true;
                    }
                }

                if (str.Equals("Infinity", StringComparison.OrdinalIgnoreCase))
                {
                    if (typeof(T) == typeof(float))
                    {
                        value = T.CreateChecked(float.PositiveInfinity);
                        return true;
                    }

                    if (typeof(T) == typeof(double))
                    {
                        value = T.CreateChecked(double.PositiveInfinity);
                        return true;
                    }

                    if (typeof(T) == typeof(Half))
                    {
                        value = T.CreateChecked(Half.PositiveInfinity);
                        return true;
                    }
                }

                if (str.Equals("-Infinity", StringComparison.OrdinalIgnoreCase))
                {
                    if (typeof(T) == typeof(float))
                    {
                        value = T.CreateChecked(float.NegativeInfinity);
                        return true;
                    }

                    if (typeof(T) == typeof(double))
                    {
                        value = T.CreateChecked(double.NegativeInfinity);
                        return true;
                    }

                    if (typeof(T) == typeof(Half))
                    {
                        value = T.CreateChecked(Half.NegativeInfinity);
                        return true;
                    }
                }
            }
        }

        value = default!;
        return false;
    }
}
