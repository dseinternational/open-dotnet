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

public class SeriesJsonConverter : JsonConverter<Series>
{
    public static SeriesJsonConverter Default { get; } = new();

    public SeriesJsonConverter() : this(default)
    {
    }

    public SeriesJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    public VectorJsonFormat Format { get; }

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(Series));
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
                    // TODO
                }
                else
                {
                    // TODO
                }
            }
        }

        // SDebug.Assert(vector is not null);

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

    private static Series<T> ReadSeries<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        ref Utf8JsonReader reader,
        int length)
    {
        if (length == 0)
        {
            return new Series<T>();
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

    public override void Write(Utf8JsonWriter writer, Series value, JsonSerializerOptions options)
    {
        //VectorJsonWriter.Write(writer, value, options);
    }
}
