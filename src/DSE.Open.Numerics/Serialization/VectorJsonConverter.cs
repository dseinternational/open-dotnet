// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Memory;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

public class VectorJsonConverter : JsonConverter<IReadOnlySeries>
{
    public static VectorJsonConverter Default { get; } = new();

    public VectorJsonConverter() : this(default)
    {
    }

    public VectorJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    public VectorJsonFormat Format { get; }

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(IReadOnlySeries));
    }

    public override Vector? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? dataType = null;
        var length = -1;
        object? categories = null;
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
            else if (propertyName == VectorJsonPropertyNames.Categories)
            {
                if (dataType is null)
                {
                    throw new JsonException("Cannot read categories without data type");
                }

                var dtype = VectorDataTypeHelper.GetSeriesDataType(dataType);

                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException("Expected start of categories object");
                }

                categories = ReadCategories(ref reader, dtype, -1);
            }
            else if (propertyName == VectorJsonPropertyNames.Values)
            {
                if (dataType is null)
                {
                    throw new JsonException("Cannot read values without data type");
                }

                var dtype = VectorDataTypeHelper.GetSeriesDataType(dataType);

                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected start of array");
                }

                if (categories is null)
                {
                    vector = dtype switch
                    {
                        VectorDataType.Float64 => VectorJsonReader.ReadSeries<double>(ref reader, length, Format),
                        VectorDataType.Float32 => VectorJsonReader.ReadSeries<float>(ref reader, length, Format),
                        VectorDataType.Int64 => VectorJsonReader.ReadSeries<long>(ref reader, length, Format),
                        VectorDataType.UInt64 => VectorJsonReader.ReadSeries<ulong>(ref reader, length, Format),
                        VectorDataType.Int32 => VectorJsonReader.ReadSeries<int>(ref reader, length, Format),
                        VectorDataType.UInt32 => VectorJsonReader.ReadSeries<uint>(ref reader, length, Format),
                        VectorDataType.Int16 => VectorJsonReader.ReadSeries<short>(ref reader, length, Format),
                        VectorDataType.UInt16 => VectorJsonReader.ReadSeries<ushort>(ref reader, length, Format),
                        VectorDataType.Int8 => VectorJsonReader.ReadSeries<sbyte>(ref reader, length, Format),
                        VectorDataType.UInt8 => VectorJsonReader.ReadSeries<byte>(ref reader, length, Format),
                        VectorDataType.Int128 => VectorJsonReader.ReadSeries<Int128>(ref reader, length, Format),
                        VectorDataType.UInt128 => VectorJsonReader.ReadSeries<UInt128>(ref reader, length, Format),
                        VectorDataType.DateTime64 => VectorJsonReader.ReadSeries<DateTime64>(ref reader, length, Format),
                        VectorDataType.DateTime => ReadSeries<DateTime>(ref reader, length),
                        VectorDataType.DateTimeOffset => ReadSeries<DateTimeOffset>(ref reader, length),
                        VectorDataType.Uuid => ReadSeries<Guid>(ref reader, length),
                        VectorDataType.Bool => ReadSeries<bool>(ref reader, length),
                        VectorDataType.Char => ReadSeries<char>(ref reader, length),
                        VectorDataType.String => VectorJsonReader.ReadStringSeries(ref reader, length),
                        _ => throw new JsonException($"Unsupported data type: {dtype}")
                    };
                }
                else
                {
#pragma warning disable IDE0072 // Add missing cases
                    vector = dtype switch
                    {
                        VectorDataType.Int64 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, long>[])categories, Format),
                        VectorDataType.UInt64 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, ulong>[])categories, Format),
                        VectorDataType.Int32 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, int>[])categories, Format),
                        VectorDataType.UInt32 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, uint>[])categories, Format),
                        VectorDataType.Int16 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, short>[])categories, Format),
                        VectorDataType.Int8 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, sbyte>[])categories, Format),
                        VectorDataType.UInt8 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, byte>[])categories, Format),
                        VectorDataType.Int128 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, Int128>[])categories, Format),
                        VectorDataType.UInt128 => VectorJsonReader.ReadCategorySeries(ref reader, length,
                            (KeyValuePair<string, UInt128>[])categories, Format),
                        _ => throw new JsonException($"Unsupported data type: {dtype}")
                    };
#pragma warning restore IDE0072 // Add missing cases
                }
            }
        }

        Debug.Assert(vector is not null);

        return vector;
    }

    private static object ReadCategories(ref Utf8JsonReader reader, VectorDataType dtype, int length)
    {
        return dtype switch
        {
            VectorDataType.Int32 => ReadNumberCategories<int>(ref reader, length),
            VectorDataType.UInt32 => ReadNumberCategories<uint>(ref reader, length),
            VectorDataType.Int64 => ReadNumberCategories<long>(ref reader, length),
            VectorDataType.UInt64 => ReadNumberCategories<ulong>(ref reader, length),
            VectorDataType.Int16 => ReadNumberCategories<short>(ref reader, length),
            VectorDataType.UInt16 => ReadNumberCategories<ushort>(ref reader, length),
            VectorDataType.Int8 => ReadNumberCategories<sbyte>(ref reader, length),
            VectorDataType.UInt8 => ReadNumberCategories<byte>(ref reader, length),
            VectorDataType.Int128 => ReadNumberCategories<Int128>(ref reader, length),
            VectorDataType.UInt128 => ReadNumberCategories<UInt128>(ref reader, length),
            _ => throw new JsonException($"Unsupported data type: {dtype}")
        };
    }

    // TODO: consider if we should use an array builder instead of a dictionary - less overhead,
    // but no duplicate keys check

    private static KeyValuePair<string, T>[] ReadNumberCategories<T>(ref Utf8JsonReader reader, int length)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        var dictionary = length > -1
            ? new Dictionary<string, T>(length, StringComparer.Ordinal)
            : new Dictionary<string, T>(StringComparer.Ordinal);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            var key = reader.GetString();

            if (key is null)
            {
                throw new JsonException("Category keys cannot be null.");
            }

            _ = reader.Read();

            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            if (reader.TryGetNumber(out T value))
            {
                dictionary.Add(key, value);
            }
        }

        return [.. dictionary];
    }

    private static Vector<T> ReadSeries<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        ref Utf8JsonReader reader,
        int length)
    {
        if (length == 0)
        {
            return new Vector<T>();
        }

        using var builder = length > -1
            ? new ArrayBuilder<T>(length, rentFromPool: false)
            : new ArrayBuilder<T>(rentFromPool: true);

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndArray:
                {
                    if (length > -1 && builder.Count != length)
                    {
                        throw new JsonException();
                    }

                    return Vector.Create(builder.ToArray());
                }
                case JsonTokenType.Number:
                    // todo
                    break;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlySeries value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        switch (value)
        {
            case IReadOnlyVector<int> intSeries:
                VectorJsonWriter.Write(writer, intSeries, options);
                return;
            case IReadOnlyVector<long> longSeries:
                VectorJsonWriter.Write(writer, longSeries, options);
                return;
            case IReadOnlyVector<float> floatSeries:
                VectorJsonWriter.Write(writer, floatSeries, options);
                return;
            case IReadOnlyVector<double> doubleSeries:
                VectorJsonWriter.Write(writer, doubleSeries, options);
                return;
            case IReadOnlyVector<uint> uintSeries:
                VectorJsonWriter.Write(writer, uintSeries, options);
                return;
            case IReadOnlyVector<ulong> uuidSeries:
                VectorJsonWriter.Write(writer, uuidSeries, options);
                return;
            case IReadOnlyVector<DateTime64> dateTime64Series:
                VectorJsonWriter.Write(writer, dateTime64Series, options);
                return;
            case IReadOnlyVector<short> shortSeries:
                VectorJsonWriter.Write(writer, shortSeries, options);
                return;
            case IReadOnlyVector<ushort> ushortSeries:
                VectorJsonWriter.Write(writer, ushortSeries, options);
                return;
            case IReadOnlyVector<sbyte> sbyteSeries:
                VectorJsonWriter.Write(writer, sbyteSeries, options);
                return;
            case IReadOnlyVector<byte> byteSeries:
                VectorJsonWriter.Write(writer, byteSeries, options);
                return;
            case IReadOnlyVector<Int128> int128Series:
                VectorJsonWriter.Write(writer, int128Series, options);
                return;
            case IReadOnlyVector<UInt128> uint128Series:
                VectorJsonWriter.Write(writer, uint128Series, options);
                return;
            case IReadOnlyVector<string> stringSeries:
                VectorJsonWriter.Write(writer, stringSeries, options);
                return;
            case IReadOnlyVector<char> charSeries:
                VectorJsonWriter.Write(writer, charSeries, options);
                return;
            case IReadOnlyVector<bool> boolSeries:
                VectorJsonWriter.Write(writer, boolSeries, options);
                return;
            case IReadOnlyVector<DateTime> dateTimeSeries:
                VectorJsonWriter.Write(writer, dateTimeSeries, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }
}
