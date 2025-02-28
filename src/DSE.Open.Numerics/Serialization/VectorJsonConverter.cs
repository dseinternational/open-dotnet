// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class VectorJsonConverter : JsonConverter<Vector>
{
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
            }
        }

        Debug.Assert(vector is not null);

        return vector;
    }

    private static Vector<T> ReadVector<T>(ref Utf8JsonReader reader, int length)
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

        if (value is NumericVector<int> intVector)
        {
            VectorJsonWriter.Write(writer, intVector, options);
            return;
        }

        if (value is NumericVector<long> longVector)
        {
            VectorJsonWriter.Write(writer, longVector, options);
            return;
        }

        if (value is NumericVector<float> floatVector)
        {
            VectorJsonWriter.Write(writer, floatVector, options);
            return;
        }

        if (value is NumericVector<double> doubleVector)
        {
            VectorJsonWriter.Write(writer, doubleVector, options);
            return;
        }

        if (value is NumericVector<uint> uintVector)
        {
            VectorJsonWriter.Write(writer, uintVector, options);
            return;
        }

        if (value is NumericVector<ulong> uuidVector)
        {
            VectorJsonWriter.Write(writer, uuidVector, options);
            return;
        }

        if (value is NumericVector<DateTime64> dateTime64Vector)
        {
            VectorJsonWriter.Write(writer, dateTime64Vector, options);
            return;
        }

        if (value is NumericVector<short> shortVector)
        {
            VectorJsonWriter.Write(writer, shortVector, options);
            return;
        }

        if (value is NumericVector<ushort> ushortVector)
        {
            VectorJsonWriter.Write(writer, ushortVector, options);
            return;
        }

        if (value is NumericVector<sbyte> sbyteVector)
        {
            VectorJsonWriter.Write(writer, sbyteVector, options);
            return;
        }

        if (value is NumericVector<byte> byteVector)
        {
            VectorJsonWriter.Write(writer, byteVector, options);
            return;
        }

        if (value is NumericVector<Int128> int128Vector)
        {
            VectorJsonWriter.Write(writer, int128Vector, options);
            return;
        }

        if (value is NumericVector<UInt128> uint128Vector)
        {
            VectorJsonWriter.Write(writer, uint128Vector, options);
            return;
        }

        if (value is Vector<string> stringVector)
        {
            VectorJsonWriter.Write(writer, stringVector, options);
            return;
        }

        if (value is Vector<char> charVector)
        {
            VectorJsonWriter.Write(writer, charVector, options);
            return;
        }

        if (value is Vector<bool> boolVector)
        {
            VectorJsonWriter.Write(writer, boolVector, options);
            return;
        }

        if (value is Vector<DateTime> dateTimeVector)
        {
            VectorJsonWriter.Write(writer, dateTimeVector, options);
            return;
        }

        throw new JsonException("Unsupported vector type");
    }
}
