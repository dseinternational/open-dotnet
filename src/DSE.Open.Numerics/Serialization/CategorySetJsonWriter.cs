// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class CategorySetJsonWriter
{
    public static void WriteCategorySet(
        Utf8JsonWriter writer,
        IReadOnlyCategorySet categorySet,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(categorySet);

        switch (categorySet)
        {
            case IReadOnlyCategorySet<int> intCategorySet:
                WriteCategorySet(writer, intCategorySet, options);
                return;
            case IReadOnlyCategorySet<long> longCategorySet:
                WriteCategorySet(writer, longCategorySet, options);
                return;
            case IReadOnlyCategorySet<float> floatCategorySet:
                WriteCategorySet(writer, floatCategorySet, options);
                return;
            case IReadOnlyCategorySet<double> doubleCategorySet:
                WriteCategorySet(writer, doubleCategorySet, options);
                return;
            case IReadOnlyCategorySet<uint> uintCategorySet:
                WriteCategorySet(writer, uintCategorySet, options);
                return;
            case IReadOnlyCategorySet<ulong> uuidCategorySet:
                WriteCategorySet(writer, uuidCategorySet, options);
                return;
            case IReadOnlyCategorySet<DateTime64> dateTime64CategorySet:
                WriteCategorySet(writer, dateTime64CategorySet, options);
                return;
            case IReadOnlyCategorySet<short> shortCategorySet:
                WriteCategorySet(writer, shortCategorySet, options);
                return;
            case IReadOnlyCategorySet<ushort> ushortCategorySet:
                WriteCategorySet(writer, ushortCategorySet, options);
                return;
            case IReadOnlyCategorySet<sbyte> sbyteCategorySet:
                WriteCategorySet(writer, sbyteCategorySet, options);
                return;
            case IReadOnlyCategorySet<byte> byteCategorySet:
                WriteCategorySet(writer, byteCategorySet, options);
                return;
            case IReadOnlyCategorySet<Int128> int128CategorySet:
                WriteCategorySet(writer, int128CategorySet, options);
                return;
            case IReadOnlyCategorySet<UInt128> uint128CategorySet:
                WriteCategorySet(writer, uint128CategorySet, options);
                return;
            case IReadOnlyCategorySet<string> stringCategorySet:
                WriteCategorySet(writer, stringCategorySet, options);
                return;
            case IReadOnlyCategorySet<char> charCategorySet:
                WriteCategorySet(writer, charCategorySet, options);
                return;
            case IReadOnlyCategorySet<bool> boolCategorySet:
                WriteCategorySet(writer, boolCategorySet, options);
                return;
            case IReadOnlyCategorySet<DateTime> dateTimeCategorySet:
                WriteCategorySet(writer, dateTimeCategorySet, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }

    public static void WriteCategorySet<T>(Utf8JsonWriter writer, IReadOnlyCategorySet<T> categorySet, JsonSerializerOptions options)
        where T : struct, IBinaryNumber<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(categorySet);

        writer.WriteStartObject();

        writer.WriteString(
            NumericsPropertyNames.DataType,
            Vector.GetLabel(Vector.GetVectorDataType<T>()));

        writer.WriteNumber(NumericsPropertyNames.Length, categorySet.Count);

        writer.WritePropertyName(NumericsPropertyNames.Values);

        writer.WriteStartArray();

        foreach (var value in categorySet)
        {
            writer.WriteNumberValue(value);
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
