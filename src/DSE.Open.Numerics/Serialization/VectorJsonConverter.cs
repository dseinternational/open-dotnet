// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public class VectorJsonConverter : JsonConverter<Vector>
{
    public const string DataTypePropertyName = "dtype";
    public const string LengthPropertyName = "length";
    public const string ValuesPropertyName = "values";

    public VectorJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    public VectorJsonFormat Format { get; }

    public override Vector? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

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
                return null;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();

                if (propertyName == DataTypePropertyName)
                {
                    _ = reader.Read();
                    dataType = reader.GetString();

                    if (dataType is null)
                    {
                        throw new JsonException("Data type must be specified");
                    }
                }
                else if (propertyName == LengthPropertyName)
                {
                    _ = reader.Read();
                    length = reader.GetInt32();

                    if (length < 0)
                    {
                        throw new JsonException("Length must be greater than or equal to zero");
                    }
                }
                else if (propertyName == ValuesPropertyName)
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
                        VectorDataType.Float64 => JsonVectorReader.ReadNumericVector<double>(ref reader, length, Format),
                        VectorDataType.Float32 => JsonVectorReader.ReadNumericVector<float>(ref reader, length, Format),
                        VectorDataType.Int64 => JsonVectorReader.ReadNumericVector<long>(ref reader, length, Format),
                        VectorDataType.UInt64 => JsonVectorReader.ReadNumericVector<ulong>(ref reader, length, Format),
                        VectorDataType.Int32 => JsonVectorReader.ReadNumericVector<int>(ref reader, length, Format),
                        VectorDataType.UInt32 => JsonVectorReader.ReadNumericVector<uint>(ref reader, length, Format),
                        VectorDataType.Int16 => JsonVectorReader.ReadNumericVector<short>(ref reader, length, Format),
                        VectorDataType.UInt16 => JsonVectorReader.ReadNumericVector<ushort>(ref reader, length, Format),
                        VectorDataType.Int8 => JsonVectorReader.ReadNumericVector<sbyte>(ref reader, length, Format),
                        VectorDataType.UInt8 => JsonVectorReader.ReadNumericVector<byte>(ref reader, length, Format),
                        VectorDataType.Int128 => JsonVectorReader.ReadNumericVector<Int128>(ref reader, length, Format),
                        VectorDataType.UInt128 => JsonVectorReader.ReadNumericVector<UInt128>(ref reader, length, Format),
                        VectorDataType.DateTime64 => JsonVectorReader.ReadNumericVector<DateTime64>(ref reader, length, Format),
                        VectorDataType.DateTime => ReadVector<DateTime>(ref reader, length),
                        VectorDataType.DateTimeOffset => ReadVector<DateTimeOffset>(ref reader, length),
                        VectorDataType.Uuid => ReadVector<Guid>(ref reader, length),
                        VectorDataType.Bool => ReadVector<bool>(ref reader, length),
                        VectorDataType.Char => ReadVector<char>(ref reader, length),
                        VectorDataType.String => JsonVectorReader.ReadStringVector(ref reader, length),
                        _ => throw new JsonException()
                    };
                }
            }
        }

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
        throw new NotImplementedException();
    }
}
