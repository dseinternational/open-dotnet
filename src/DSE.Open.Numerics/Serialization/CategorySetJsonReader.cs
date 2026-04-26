// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON deserialization helpers for <see cref="ICategorySet"/> /
/// <see cref="CategorySet{T}"/>. Reads the data-type tag from the JSON object
/// and dispatches to the appropriate strongly-typed reader.
/// </summary>
public static class CategorySetJsonReader
{
    /// <summary>Reads a category set from <paramref name="reader"/>; returns <see langword="null"/> when the input lacks values.</summary>
    public static ICategorySet? Read(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? dataType = null;
        var length = -1;
        ICategorySet? categorySet = null;

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
                    throw new JsonException("Cannot read category set without data type");
                }

                var dtype = Vector.GetDataType(dataType);

                _ = reader.Read();

                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected start of array");
                }

                categorySet = dtype switch
                {
                    VectorDataType.Int64 => new CategorySet<long>(reader.ReadNumberArray<long>(length)),
                    VectorDataType.UInt64 => new CategorySet<ulong>(reader.ReadNumberArray<ulong>(length)),
                    VectorDataType.Int32 => new CategorySet<int>(reader.ReadNumberArray<int>(length)),
                    VectorDataType.UInt32 => new CategorySet<uint>(reader.ReadNumberArray<uint>(length)),
                    VectorDataType.Int16 => new CategorySet<short>(reader.ReadNumberArray<short>(length)),
                    VectorDataType.UInt16 => new CategorySet<ushort>(reader.ReadNumberArray<ushort>(length)),
                    VectorDataType.Int8 => new CategorySet<sbyte>(reader.ReadNumberArray<sbyte>(length)),
                    VectorDataType.UInt8 => new CategorySet<byte>(reader.ReadNumberArray<byte>(length)),
                    VectorDataType.Float64 => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.Float32 => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.Float16 => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.DateTime64 => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.DateTime => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.DateTimeOffset => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.Bool => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.Char => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.String => throw new JsonException("Category sets should only contain binary integer values"),
                    VectorDataType.NaFloat64 => throw new NotImplementedException(),
                    VectorDataType.NaFloat32 => throw new NotImplementedException(),
                    VectorDataType.NaFloat16 => throw new NotImplementedException(),
                    VectorDataType.NaInt64 => throw new NotImplementedException(),
                    VectorDataType.NaUInt64 => throw new NotImplementedException(),
                    VectorDataType.NaInt32 => throw new NotImplementedException(),
                    VectorDataType.NaUInt32 => throw new NotImplementedException(),
                    VectorDataType.NaInt16 => throw new NotImplementedException(),
                    VectorDataType.NaUInt16 => throw new NotImplementedException(),
                    VectorDataType.NaInt8 => throw new NotImplementedException(),
                    VectorDataType.NaUInt8 => throw new NotImplementedException(),
                    VectorDataType.NaDateTime64 => throw new NotImplementedException(),
                    VectorDataType.NaDateTime => throw new NotImplementedException(),
                    VectorDataType.NaDateTimeOffset => throw new NotImplementedException(),
                    VectorDataType.NaBool => throw new NotImplementedException(),
                    VectorDataType.NaChar => throw new NotImplementedException(),
                    VectorDataType.NaString => throw new NotImplementedException(),
                    _ => throw new JsonException($"Unsupported data type: {dtype}")
                };
            }
            else
            {
                throw new JsonException($"Unexpected property name: {propertyName}");
            }
        }

        if (categorySet is null)
        {
            throw new JsonException("Cannot read category set");
        }

        return categorySet;
    }

    /// <summary>Reads a strongly-typed numeric category set of <typeparamref name="T"/> from <paramref name="reader"/>.</summary>
    public static CategorySet<T>? Read<T>(ref Utf8JsonReader reader)
        where T : struct, IBinaryNumber<T>
    {
        return (CategorySet<T>?)Read(ref reader);
    }
}
