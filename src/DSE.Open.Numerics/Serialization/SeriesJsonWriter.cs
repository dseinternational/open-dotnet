// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class SeriesJsonWriter
{
    public static void Write(Utf8JsonWriter writer, IReadOnlySeries series, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(series);

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
            case IReadOnlySeries<Int128> int128Series:
                WriteSeries(writer, int128Series, options);
                return;
            case IReadOnlySeries<UInt128> uint128Series:
                WriteSeries(writer, uint128Series, options);
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
}
