// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Collections.Generic;

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
        ValueLabelCollection? valueLabelCollection = null;
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
            else if (propertyName == NumericsPropertyNames.Labels)
            {
                _ = reader.Read();
                valueLabelCollection = ValueLabelCollectionJsonReader.ReadLabels(ref reader);
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

        return CreateSeries(name, values, categories, valueLabelCollection);
    }

    private static Series CreateSeries(
        string? name,
        Vector data,
        ICategorySet? categorySet,
        ValueLabelCollection? valueLabelCollection)
    {
        ArgumentNullException.ThrowIfNull(data);

        return data.DataType switch
        {
            VectorDataType.Float64 => CreateSeries<double>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Float32 => CreateSeries<float>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Float16 => CreateSeries<Half>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Int64 => CreateSeries<long>(data, name, categorySet, valueLabelCollection),
            VectorDataType.UInt64 => CreateSeries<ulong>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Int32 => CreateSeries<int>(data, name, categorySet, valueLabelCollection),
            VectorDataType.UInt32 => CreateSeries<uint>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Int16 => CreateSeries<short>(data, name, categorySet, valueLabelCollection),
            VectorDataType.UInt16 => CreateSeries<ushort>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Int8 => CreateSeries<sbyte>(data, name, categorySet, valueLabelCollection),
            VectorDataType.UInt8 => CreateSeries<byte>(data, name, categorySet, valueLabelCollection),
            VectorDataType.DateTime64 => CreateSeries<DateTime64>(data, name, categorySet, valueLabelCollection),
            VectorDataType.DateTime => CreateSeries<DateTime>(data, name, categorySet, valueLabelCollection),
            VectorDataType.DateTimeOffset => CreateSeries<DateTimeOffset>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Bool => CreateSeries<bool>(data, name, categorySet, valueLabelCollection),
            VectorDataType.Char => CreateSeries<char>(data, name, categorySet, valueLabelCollection),
            VectorDataType.String => CreateSeries<string>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaFloat64 => CreateNaFloatSeries<double>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaFloat32 => CreateNaFloatSeries<float>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaFloat16 => CreateNaFloatSeries<Half>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaInt64 => CreateNaIntSeries<long>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaUInt64 => CreateNaIntSeries<ulong>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaInt32 => CreateNaIntSeries<int>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaUInt32 => CreateNaIntSeries<ulong>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaInt16 => CreateNaIntSeries<short>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaUInt16 => CreateNaIntSeries<ushort>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaInt8 => CreateNaIntSeries<sbyte>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaUInt8 => CreateNaIntSeries<byte>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaDateTime64 => CreateNaIntSeries<DateTime64>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaDateTime => CreateNaValueSeries<DateTime>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaDateTimeOffset => CreateNaValueSeries<DateTimeOffset>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaBool => CreateNaValueSeries<bool>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaChar => CreateNaValueSeries<char>(data, name, categorySet, valueLabelCollection),
            VectorDataType.NaString => CreateNaValueSeries<string>(data, name, categorySet, valueLabelCollection),
            _ => throw new InvalidOperationException("Unsupported data type: " + data.DataType),
        };

        static Series<T> CreateSeries<T>(Vector data, string? name, ICategorySet? categorySet, ValueLabelCollection? valueLabelCollection)
            where T : IEquatable<T>
        {
            return new Series<T>((Vector<T>)data, name, (CategorySet<T>?)categorySet, (ValueLabelCollection<T>?)valueLabelCollection);
        }

        static Series<NaInt<T>> CreateNaIntSeries<T>(Vector data, string? name, ICategorySet? categorySet, ValueLabelCollection? valueLabelCollection)
            where T : struct, IBinaryInteger<T>, IMinMaxValue<T>, IEquatable<T>
        {
            return CreateSeries<NaInt<T>>(data, name, categorySet, valueLabelCollection);
        }

        static Series<NaFloat<T>> CreateNaFloatSeries<T>(Vector data, string? name, ICategorySet? categorySet, ValueLabelCollection? valueLabelCollection)
            where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>, IEquatable<T>
        {
            return CreateSeries<NaFloat<T>>(data, name, categorySet, valueLabelCollection);
        }

        static Series<NaValue<T>> CreateNaValueSeries<T>(Vector data, string? name, ICategorySet? categorySet, ValueLabelCollection? valueLabelCollection)
            where T : IEquatable<T>, IComparable<T>, ISpanParsable<T>
        {
            return CreateSeries<NaValue<T>>(data, name, categorySet, valueLabelCollection);
        }
    }
}
