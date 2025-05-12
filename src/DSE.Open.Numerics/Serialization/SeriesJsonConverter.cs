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

public class SeriesJsonConverter : JsonConverter<IReadOnlySeries>
{
    public static SeriesJsonConverter Default { get; } = new();

    public SeriesJsonConverter() : this(default)
    {
    }

    public SeriesJsonConverter(SeriesJsonFormat format = default)
    {
        Format = format;
    }

    public SeriesJsonFormat Format { get; }

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(IReadOnlySeries));
    }

    public override Series? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? dataType = null;
        var length = -1;
        object? categories = null;
        Series? vector = null;

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

            if (propertyName == SeriesJsonPropertyNames.DataType)
            {
                _ = reader.Read();
                dataType = reader.GetString();

                if (dataType is null)
                {
                    throw new JsonException("Data type must be specified");
                }
            }
            else if (propertyName == SeriesJsonPropertyNames.Length)
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
            else if (propertyName == SeriesJsonPropertyNames.Categories)
            {
                if (dataType is null)
                {
                    throw new JsonException("Cannot read categories without data type");
                }

                var dtype = VectorDataTypeHelper.GetVectorDataType(dataType);

                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException("Expected start of categories object");
                }

                categories = ReadCategories(ref reader, dtype, -1);
            }
            else if (propertyName == SeriesJsonPropertyNames.Values)
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

                if (categories is null)
                {
                    vector = dtype switch
                    {
                        SeriesDataType.Float64 => SeriesJsonReader.ReadVector<double>(ref reader, length, Format),
                        SeriesDataType.Float32 => SeriesJsonReader.ReadVector<float>(ref reader, length, Format),
                        SeriesDataType.Int64 => SeriesJsonReader.ReadVector<long>(ref reader, length, Format),
                        SeriesDataType.UInt64 => SeriesJsonReader.ReadVector<ulong>(ref reader, length, Format),
                        SeriesDataType.Int32 => SeriesJsonReader.ReadVector<int>(ref reader, length, Format),
                        SeriesDataType.UInt32 => SeriesJsonReader.ReadVector<uint>(ref reader, length, Format),
                        SeriesDataType.Int16 => SeriesJsonReader.ReadVector<short>(ref reader, length, Format),
                        SeriesDataType.UInt16 => SeriesJsonReader.ReadVector<ushort>(ref reader, length, Format),
                        SeriesDataType.Int8 => SeriesJsonReader.ReadVector<sbyte>(ref reader, length, Format),
                        SeriesDataType.UInt8 => SeriesJsonReader.ReadVector<byte>(ref reader, length, Format),
                        SeriesDataType.Int128 => SeriesJsonReader.ReadVector<Int128>(ref reader, length, Format),
                        SeriesDataType.UInt128 => SeriesJsonReader.ReadVector<UInt128>(ref reader, length, Format),
                        SeriesDataType.DateTime64 =>
                            SeriesJsonReader.ReadVector<DateTime64>(ref reader, length, Format),
                        SeriesDataType.DateTime => ReadVector<DateTime>(ref reader, length),
                        SeriesDataType.DateTimeOffset => ReadVector<DateTimeOffset>(ref reader, length),
                        SeriesDataType.Uuid => ReadVector<Guid>(ref reader, length),
                        SeriesDataType.Bool => ReadVector<bool>(ref reader, length),
                        SeriesDataType.Char => ReadVector<char>(ref reader, length),
                        SeriesDataType.String => SeriesJsonReader.ReadStringVector(ref reader, length),
                        _ => throw new JsonException($"Unsupported data type: {dtype}")
                    };
                }
                else
                {
#pragma warning disable IDE0072 // Add missing cases
                    vector = dtype switch
                    {
                        SeriesDataType.Int64 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, long>)categories, Format),
                        SeriesDataType.UInt64 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, ulong>)categories, Format),
                        SeriesDataType.Int32 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, int>)categories, Format),
                        SeriesDataType.UInt32 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, uint>)categories, Format),
                        SeriesDataType.Int16 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, short>)categories, Format),
                        SeriesDataType.Int8 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, sbyte>)categories, Format),
                        SeriesDataType.UInt8 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, byte>)categories, Format),
                        SeriesDataType.Int128 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, Int128>)categories, Format),
                        SeriesDataType.UInt128 => SeriesJsonReader.ReadCategoryVector(ref reader, length,
                            (Dictionary<string, UInt128>)categories, Format),
                        _ => throw new JsonException($"Unsupported data type: {dtype}")
                    };
#pragma warning restore IDE0072 // Add missing cases
                }
            }
        }

        Debug.Assert(vector is not null);

        return vector;
    }

    private static object ReadCategories(ref Utf8JsonReader reader, SeriesDataType dtype, int length)
    {
        return dtype switch
        {
            SeriesDataType.Int32 => ReadNumberCategories<int>(ref reader, length),
            SeriesDataType.UInt32 => ReadNumberCategories<uint>(ref reader, length),
            SeriesDataType.Int64 => ReadNumberCategories<long>(ref reader, length),
            SeriesDataType.UInt64 => ReadNumberCategories<ulong>(ref reader, length),
            SeriesDataType.Int16 => ReadNumberCategories<short>(ref reader, length),
            SeriesDataType.UInt16 => ReadNumberCategories<ushort>(ref reader, length),
            SeriesDataType.Int8 => ReadNumberCategories<sbyte>(ref reader, length),
            SeriesDataType.UInt8 => ReadNumberCategories<byte>(ref reader, length),
            SeriesDataType.Int128 => ReadNumberCategories<Int128>(ref reader, length),
            SeriesDataType.UInt128 => ReadNumberCategories<UInt128>(ref reader, length),
            _ => throw new JsonException($"Unsupported data type: {dtype}")
        };
    }

    private static Dictionary<string, T> ReadNumberCategories<T>(ref Utf8JsonReader reader, int length)
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

        return dictionary;
    }

    private static Series<T> ReadVector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        ref Utf8JsonReader reader,
        int length)
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
            switch (reader.TokenType)
            {
                case JsonTokenType.EndArray:
                {
                    if (length > -1 && builder.Count != length)
                    {
                        throw new JsonException();
                    }

                    return Series.Create(builder.ToArray());
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
            case IReadOnlySeries<int> intVector:
                SeriesJsonWriter.Write(writer, intVector, options);
                return;
            case IReadOnlySeries<long> longVector:
                SeriesJsonWriter.Write(writer, longVector, options);
                return;
            case IReadOnlySeries<float> floatVector:
                SeriesJsonWriter.Write(writer, floatVector, options);
                return;
            case IReadOnlySeries<double> doubleVector:
                SeriesJsonWriter.Write(writer, doubleVector, options);
                return;
            case IReadOnlySeries<uint> uintVector:
                SeriesJsonWriter.Write(writer, uintVector, options);
                return;
            case IReadOnlySeries<ulong> uuidVector:
                SeriesJsonWriter.Write(writer, uuidVector, options);
                return;
            case IReadOnlySeries<DateTime64> dateTime64Vector:
                SeriesJsonWriter.Write(writer, dateTime64Vector, options);
                return;
            case IReadOnlySeries<short> shortVector:
                SeriesJsonWriter.Write(writer, shortVector, options);
                return;
            case IReadOnlySeries<ushort> ushortVector:
                SeriesJsonWriter.Write(writer, ushortVector, options);
                return;
            case IReadOnlySeries<sbyte> sbyteVector:
                SeriesJsonWriter.Write(writer, sbyteVector, options);
                return;
            case IReadOnlySeries<byte> byteVector:
                SeriesJsonWriter.Write(writer, byteVector, options);
                return;
            case IReadOnlySeries<Int128> int128Vector:
                SeriesJsonWriter.Write(writer, int128Vector, options);
                return;
            case IReadOnlySeries<UInt128> uint128Vector:
                SeriesJsonWriter.Write(writer, uint128Vector, options);
                return;
            case IReadOnlySeries<string> stringVector:
                SeriesJsonWriter.Write(writer, stringVector, options);
                return;
            case IReadOnlySeries<char> charVector:
                SeriesJsonWriter.Write(writer, charVector, options);
                return;
            case IReadOnlySeries<bool> boolVector:
                SeriesJsonWriter.Write(writer, boolVector, options);
                return;
            case IReadOnlySeries<DateTime> dateTimeVector:
                SeriesJsonWriter.Write(writer, dateTimeVector, options);
                return;
            default:
                throw new JsonException("Unsupported vector type");
        }
    }
}
