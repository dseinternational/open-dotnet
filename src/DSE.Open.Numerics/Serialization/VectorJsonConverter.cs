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

public class VectorJsonConverter : JsonConverter<Vector>
{
    public static VectorJsonConverter Default { get; } = new();

    public VectorJsonConverter() : this(default) { }

    public VectorJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    public VectorJsonFormat Format { get; }

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(Vector));
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

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
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

                    var dtype = VectorDataTypeHelper.GetVectorDataType(dataType);

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
                            VectorDataType.Float64 => VectorJsonReader.ReadNumericVector<double>(ref reader, length, Format),
                            VectorDataType.Float32 => VectorJsonReader.ReadNumericVector<float>(ref reader, length, Format),
                            VectorDataType.Int64 => VectorJsonReader.ReadNumericVector<long>(ref reader, length, Format),
                            VectorDataType.UInt64 => VectorJsonReader.ReadNumericVector<ulong>(ref reader, length, Format),
                            VectorDataType.Int32 => VectorJsonReader.ReadNumericVector<int>(ref reader, length, Format),
                            VectorDataType.UInt32 => VectorJsonReader.ReadNumericVector<uint>(ref reader, length, Format),
                            VectorDataType.Int16 => VectorJsonReader.ReadNumericVector<short>(ref reader, length, Format),
                            VectorDataType.UInt16 => VectorJsonReader.ReadNumericVector<ushort>(ref reader, length, Format),
                            VectorDataType.Int8 => VectorJsonReader.ReadNumericVector<sbyte>(ref reader, length, Format),
                            VectorDataType.UInt8 => VectorJsonReader.ReadNumericVector<byte>(ref reader, length, Format),
                            VectorDataType.Int128 => VectorJsonReader.ReadNumericVector<Int128>(ref reader, length, Format),
                            VectorDataType.UInt128 => VectorJsonReader.ReadNumericVector<UInt128>(ref reader, length, Format),
                            VectorDataType.DateTime64 => VectorJsonReader.ReadNumericVector<DateTime64>(ref reader, length, Format),
                            VectorDataType.DateTime => ReadVector<DateTime>(ref reader, length),
                            VectorDataType.DateTimeOffset => ReadVector<DateTimeOffset>(ref reader, length),
                            VectorDataType.Uuid => ReadVector<Guid>(ref reader, length),
                            VectorDataType.Bool => ReadVector<bool>(ref reader, length),
                            VectorDataType.Char => ReadVector<char>(ref reader, length),
                            VectorDataType.String => VectorJsonReader.ReadStringVector(ref reader, length),
                            _ => throw new JsonException($"Unsupported data type: {dtype}")
                        };
                    }
                    else
                    {
#pragma warning disable IDE0072 // Add missing cases
                        vector = dtype switch
                        {
                            VectorDataType.Int64 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, long>>)categories, Format),
                            VectorDataType.UInt64 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, ulong>>)categories, Format),
                            VectorDataType.Int32 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, int>>)categories, Format),
                            VectorDataType.UInt32 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, uint>>)categories, Format),
                            VectorDataType.Int16 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, short>>)categories, Format),
                            VectorDataType.Int8 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, sbyte>>)categories, Format),
                            VectorDataType.UInt8 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, byte>>)categories, Format),
                            VectorDataType.Int128 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, Int128>>)categories, Format),
                            VectorDataType.UInt128 => VectorJsonReader.ReadCategoryVector(ref reader, length, (Memory<KeyValuePair<string, UInt128>>)categories, Format),
                            _ => throw new JsonException($"Unsupported data type: {dtype}")
                        };
#pragma warning restore IDE0072 // Add missing cases
                    }
                }
            }
        }

        Debug.Assert(vector is not null);

        return vector;
    }

    private static object ReadCategories(ref Utf8JsonReader reader, VectorDataType dtype, int length)
    {
        if (dtype == VectorDataType.Int32)
        {
            return ReadNumberCategories<int>(ref reader, length);
        }

        if (dtype == VectorDataType.UInt32)
        {
            return ReadNumberCategories<uint>(ref reader, length);
        }

        if (dtype == VectorDataType.Int64)
        {
            return ReadNumberCategories<long>(ref reader, length);
        }

        if (dtype == VectorDataType.UInt64)
        {
            return ReadNumberCategories<ulong>(ref reader, length);
        }

        if (dtype == VectorDataType.Int16)
        {
            return ReadNumberCategories<short>(ref reader, length);
        }

        if (dtype == VectorDataType.UInt16)
        {
            return ReadNumberCategories<ushort>(ref reader, length);
        }

        if (dtype == VectorDataType.Int8)
        {
            return ReadNumberCategories<sbyte>(ref reader, length);
        }

        if (dtype == VectorDataType.UInt8)
        {
            return ReadNumberCategories<byte>(ref reader, length);
        }

        if (dtype == VectorDataType.Int128)
        {
            return ReadNumberCategories<Int128>(ref reader, length);
        }

        if (dtype == VectorDataType.UInt128)
        {
            return ReadNumberCategories<UInt128>(ref reader, length);
        }

        throw new JsonException($"Unsupported data type: {dtype}");
    }

    private static Memory<KeyValuePair<string, T>> ReadNumberCategories<T>(ref Utf8JsonReader reader, int length)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        using var builder = length > -1
            ? new ArrayBuilder<KeyValuePair<string, T>>(length, rentFromPool: false)
            : new ArrayBuilder<KeyValuePair<string, T>>(rentFromPool: true);

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
                builder.Add(new KeyValuePair<string, T>(key, value));
            }
        }

        return builder.ToMemory();
    }

    private static Vector<T> ReadVector<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(ref Utf8JsonReader reader, int length)
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

            if (reader.TokenType == JsonTokenType.Number)
            {
                // todo
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Vector value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        if (value is IReadOnlyNumericVector<int> intVector)
        {
            VectorJsonWriter.Write(writer, intVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<long> longVector)
        {
            VectorJsonWriter.Write(writer, longVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<float> floatVector)
        {
            VectorJsonWriter.Write(writer, floatVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<double> doubleVector)
        {
            VectorJsonWriter.Write(writer, doubleVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<uint> uintVector)
        {
            VectorJsonWriter.Write(writer, uintVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<ulong> uuidVector)
        {
            VectorJsonWriter.Write(writer, uuidVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<DateTime64> dateTime64Vector)
        {
            VectorJsonWriter.Write(writer, dateTime64Vector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<short> shortVector)
        {
            VectorJsonWriter.Write(writer, shortVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<ushort> ushortVector)
        {
            VectorJsonWriter.Write(writer, ushortVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<sbyte> sbyteVector)
        {
            VectorJsonWriter.Write(writer, sbyteVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<byte> byteVector)
        {
            VectorJsonWriter.Write(writer, byteVector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<Int128> int128Vector)
        {
            VectorJsonWriter.Write(writer, int128Vector, options);
            return;
        }

        if (value is IReadOnlyNumericVector<UInt128> uint128Vector)
        {
            VectorJsonWriter.Write(writer, uint128Vector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<int> intCategoricalVector)
        {
            VectorJsonWriter.Write(writer, intCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<long> longCategoricalVector)
        {
            VectorJsonWriter.Write(writer, longCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<uint> uintCategoricalVector)
        {
            VectorJsonWriter.Write(writer, uintCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<ulong> uint64CategoricalVector)
        {
            VectorJsonWriter.Write(writer, uint64CategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<short> shortCategoricalVector)
        {
            VectorJsonWriter.Write(writer, shortCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<ushort> ushortCategoricalVector)
        {
            VectorJsonWriter.Write(writer, ushortCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<sbyte> sbyteCategoricalVector)
        {
            VectorJsonWriter.Write(writer, sbyteCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<byte> byteCategoricalVector)
        {
            VectorJsonWriter.Write(writer, byteCategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<Int128> int128CategoricalVector)
        {
            VectorJsonWriter.Write(writer, int128CategoricalVector, options);
            return;
        }

        if (value is IReadOnlyCategoricalVector<UInt128> uint128CategoricalVector)
        {
            VectorJsonWriter.Write(writer, uint128CategoricalVector, options);
            return;
        }

        if (value is IReadOnlyVector<string> stringVector)
        {
            VectorJsonWriter.Write(writer, stringVector, options);
            return;
        }

        if (value is IReadOnlyVector<char> charVector)
        {
            VectorJsonWriter.Write(writer, charVector, options);
            return;
        }

        if (value is IReadOnlyVector<bool> boolVector)
        {
            VectorJsonWriter.Write(writer, boolVector, options);
            return;
        }

        if (value is IReadOnlyVector<DateTime> dateTimeVector)
        {
            VectorJsonWriter.Write(writer, dateTimeVector, options);
            return;
        }

        throw new JsonException("Unsupported vector type");
    }
}
