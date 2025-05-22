// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class SeriesJsonWriter
{
    public static void Write(Utf8JsonWriter writer, IReadOnlySeries series, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(series);

        if (series.Vector.IsNullable)
        {
            WriteNa(writer, series, options);
            return;
        }

        switch (series)
        {
            case IReadOnlySeries<int> intSeries:
                WriteSeries(writer, intSeries, options);
                return;
            case IReadOnlySeries<long> longSeries:
                WriteSeries(writer, longSeries, options);
                return;
            case IReadOnlySeries<float> floatSeries:
                WriteSeries(writer, floatSeries, options);
                return;
            case IReadOnlySeries<double> doubleSeries:
                WriteSeries(writer, doubleSeries, options);
                return;
            case IReadOnlySeries<Half> halfSeries:
                WriteSeries(writer, halfSeries, options);
                return;
            case IReadOnlySeries<uint> uintSeries:
                WriteSeries(writer, uintSeries, options);
                return;
            case IReadOnlySeries<ulong> uuidSeries:
                WriteSeries(writer, uuidSeries, options);
                return;
            case IReadOnlySeries<DateTime64> dateTime64Series:
                WriteSeries(writer, dateTime64Series, options);
                return;
            case IReadOnlySeries<short> shortSeries:
                WriteSeries(writer, shortSeries, options);
                return;
            case IReadOnlySeries<ushort> ushortSeries:
                WriteSeries(writer, ushortSeries, options);
                return;
            case IReadOnlySeries<sbyte> sbyteSeries:
                WriteSeries(writer, sbyteSeries, options);
                return;
            case IReadOnlySeries<byte> byteSeries:
                WriteSeries(writer, byteSeries, options);
                return;
            case IReadOnlySeries<string> stringSeries:
                WriteSeries(writer, stringSeries, options);
                return;
            case IReadOnlySeries<char> charSeries:
                WriteSeries(writer, charSeries, options);
                return;
            case IReadOnlySeries<bool> boolSeries:
                WriteSeries(writer, boolSeries, options);
                return;
            case IReadOnlySeries<DateTime> dateTimeSeries:
                WriteSeries(writer, dateTimeSeries, options);
                return;
            case IReadOnlySeries<DateTimeOffset> dateTimeOffsetSeries:
                WriteSeries(writer, dateTimeOffsetSeries, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }

    private static void WriteNa(Utf8JsonWriter writer, IReadOnlySeries series, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(series);

        switch (series)
        {
            case IReadOnlySeries<NaInt<int>> intSeries:
                WriteNaSeries(writer, intSeries, options);
                return;
            case IReadOnlySeries<NaInt<long>> longSeries:
                WriteNaSeries(writer, longSeries, options);
                return;
            case IReadOnlySeries<NaFloat<float>> floatSeries:
                WriteNaSeries(writer, floatSeries, options);
                return;
            case IReadOnlySeries<NaFloat<Half>> halfSeries:
                WriteNaSeries(writer, halfSeries, options);
                return;
            case IReadOnlySeries<NaFloat<double>> doubleSeries:
                WriteNaSeries(writer, doubleSeries, options);
                return;
            case IReadOnlySeries<NaInt<uint>> uintSeries:
                WriteNaSeries(writer, uintSeries, options);
                return;
            case IReadOnlySeries<NaInt<ulong>> uuidSeries:
                WriteNaSeries(writer, uuidSeries, options);
                return;
            case IReadOnlySeries<NaInt<DateTime64>> dateTime64Series:
                WriteNaSeries(writer, dateTime64Series, options);
                return;
            case IReadOnlySeries<NaInt<short>> shortSeries:
                WriteNaSeries(writer, shortSeries, options);
                return;
            case IReadOnlySeries<NaInt<ushort>> ushortSeries:
                WriteNaSeries(writer, ushortSeries, options);
                return;
            case IReadOnlySeries<NaInt<sbyte>> sbyteSeries:
                WriteNaSeries(writer, sbyteSeries, options);
                return;
            case IReadOnlySeries<NaInt<byte>> byteSeries:
                WriteNaSeries(writer, byteSeries, options);
                return;
            case IReadOnlySeries<NaValue<string>> stringSeries:
                WriteNaSeries(writer, stringSeries, options);
                return;
            case IReadOnlySeries<NaValue<char>> charSeries:
                WriteNaSeries(writer, charSeries, options);
                return;
            case IReadOnlySeries<NaValue<bool>> boolSeries:
                WriteNaSeries(writer, boolSeries, options);
                return;
            case IReadOnlySeries<NaValue<DateTime>> dateTimeSeries:
                WriteNaSeries(writer, dateTimeSeries, options);
                return;
            case IReadOnlySeries<NaValue<DateTimeOffset>> dateTimeOffsetSeries:
                WriteNaSeries(writer, dateTimeOffsetSeries, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }

    public static void WriteSeries<T>(Utf8JsonWriter writer, IReadOnlySeries<T> series, JsonSerializerOptions options)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(series);

        writer.WriteStartObject();

        if (series.Name is not null)
        {
            writer.WriteString(NumericsPropertyNames.Name, series.Name);
        }

        if (series is IReadOnlyCategoricalSeries categorical)
        {
            writer.WritePropertyName(NumericsPropertyNames.Categories);
            CategorySetJsonWriter.WriteCategorySet(writer, categorical.Categories, options);
        }

        writer.WritePropertyName(NumericsPropertyNames.Vector);

        VectorJsonWriter.WriteVector(writer, series.Vector, options);

        writer.WriteEndObject();
    }

    private static void WriteNaSeries<T>(Utf8JsonWriter writer, IReadOnlySeries<T> series, JsonSerializerOptions options)
        where T : INaValue, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(series);

        writer.WriteStartObject();

        if (series.Name is not null)
        {
            writer.WriteString(NumericsPropertyNames.Name, series.Name);
        }

        if (series is IReadOnlyCategoricalSeries categorical)
        {
            writer.WritePropertyName(NumericsPropertyNames.Categories);
            CategorySetJsonWriter.WriteCategorySet(writer, categorical.Categories, options);
        }

        writer.WritePropertyName(NumericsPropertyNames.Vector);

        VectorJsonWriter.WriteVector(writer, series.Vector, options);

        writer.WriteEndObject();
    }
}
