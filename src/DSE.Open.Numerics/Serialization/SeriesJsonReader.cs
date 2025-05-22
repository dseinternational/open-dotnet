// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class SeriesJsonReader
{
    public static Series? Read(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? name = null;
        ICategorySet? categories = null;
        Vector? values = null;

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

            if (propertyName == NumericsPropertyNames.Name)
            {
                _ = reader.Read();
                name = reader.GetString();
            }
            else if (propertyName == NumericsPropertyNames.Categories)
            {
                _ = reader.Read();
                categories = CategorySetJsonReader.Read(ref reader);
            }
            else if (propertyName == NumericsPropertyNames.Vector)
            {
                _ = reader.Read();
                values = VectorJsonReader.ReadVector(ref reader, VectorJsonFormat.Default);
            }
            else
            {
                throw new JsonException($"Unexpected property name: {propertyName}");
            }
        }

        if (values is null)
        {
            throw new JsonException("Cannot read series without values");
        }

        if (categories is not null)
        {
            // todo  return CategoricalSeries.CreateUntyped(values, name, null, categories, null);
        }

        return CreateSeries(name, values, null);
    }

    private static Series CreateSeries(
        string? name,
        Vector data,
        ICategorySet? categorySet) // todo
    {
        ArgumentNullException.ThrowIfNull(data);

        return data.DataType switch
        {
            VectorDataType.Float64 => CreateSeries<double>(data, name, categorySet),
            VectorDataType.Float32 => CreateSeries<float>(data, name, categorySet),
            VectorDataType.Float16 => CreateSeries<Half>(data, name, categorySet),
            VectorDataType.Int64 => CreateSeries<long>(data, name, categorySet),
            VectorDataType.UInt64 => CreateSeries<ulong>(data, name, categorySet),
            VectorDataType.Int32 => CreateSeries<int>(data, name, categorySet),
            VectorDataType.UInt32 => CreateSeries<uint>(data, name, categorySet),
            VectorDataType.Int16 => CreateSeries<short>(data, name, categorySet),
            VectorDataType.UInt16 => CreateSeries<ushort>(data, name, categorySet),
            VectorDataType.Int8 => CreateSeries<sbyte>(data, name, categorySet),
            VectorDataType.UInt8 => CreateSeries<byte>(data, name, categorySet),
            VectorDataType.DateTime64 => CreateSeries<DateTime64>(data, name, categorySet),
            VectorDataType.DateTime => CreateSeries<DateTime>(data, name, categorySet),
            VectorDataType.DateTimeOffset => CreateSeries<DateTimeOffset>(data, name, categorySet),
            VectorDataType.Bool => CreateSeries<bool>(data, name, categorySet),
            VectorDataType.Char => CreateSeries<char>(data, name, categorySet),
            VectorDataType.String => CreateSeries<string>(data, name, categorySet),
            VectorDataType.NaFloat64 => CreateNaFloatSeries<double>(data, name, categorySet),
            VectorDataType.NaFloat32 => CreateNaFloatSeries<float>(data, name, categorySet),
            VectorDataType.NaFloat16 => CreateNaFloatSeries<Half>(data, name, categorySet),
            VectorDataType.NaInt64 => CreateNaIntSeries<long>(data, name, categorySet),
            VectorDataType.NaUInt64 => CreateNaIntSeries<ulong>(data, name, categorySet),
            VectorDataType.NaInt32 => CreateNaIntSeries<int>(data, name, categorySet),
            VectorDataType.NaUInt32 => CreateNaIntSeries<ulong>(data, name, categorySet),
            VectorDataType.NaInt16 => CreateNaIntSeries<short>(data, name, categorySet),
            VectorDataType.NaUInt16 => CreateNaIntSeries<ushort>(data, name, categorySet),
            VectorDataType.NaInt8 => CreateNaIntSeries<sbyte>(data, name, categorySet),
            VectorDataType.NaUInt8 => CreateNaIntSeries<byte>(data, name, categorySet),
            VectorDataType.NaDateTime64 => CreateNaIntSeries<DateTime64>(data, name, categorySet),
            VectorDataType.NaDateTime => CreateNaValueSeries<DateTime>(data, name, categorySet),
            VectorDataType.NaDateTimeOffset => CreateNaValueSeries<DateTimeOffset>(data, name, categorySet),
            VectorDataType.NaBool => CreateNaValueSeries<bool>(data, name, categorySet),
            VectorDataType.NaChar => CreateNaValueSeries<char>(data, name, categorySet),
            VectorDataType.NaString => CreateNaValueSeries<string>(data, name, categorySet),
            _ => throw new InvalidOperationException("Unsupported data type: " + data.DataType),
        };

        static Series<T> CreateSeries<T>(Vector data, string? name, ICategorySet? categorySet)
            where T : IEquatable<T>
        {
            return new Series<T>((Vector<T>)data, name, (CategorySet<T>?)categorySet);
        }

        static Series<NaInt<T>> CreateNaIntSeries<T>(Vector data, string? name, ICategorySet? categorySet)
            where T : struct, IBinaryInteger<T>, IMinMaxValue<T>, IEquatable<T>
        {
            return CreateSeries<NaInt<T>>(data, name, categorySet);
        }

        static Series<NaFloat<T>> CreateNaFloatSeries<T>(Vector data, string? name, ICategorySet? categorySet)
            where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>, IEquatable<T>
        {
            return CreateSeries<NaFloat<T>>(data, name, categorySet);
        }

        static Series<NaValue<T>> CreateNaValueSeries<T>(Vector data, string? name, ICategorySet? categorySet)
            where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
        {
            return CreateSeries<NaValue<T>>(data, name, categorySet);
        }
    }
}
