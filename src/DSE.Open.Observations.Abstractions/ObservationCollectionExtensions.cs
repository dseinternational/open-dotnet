// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

namespace DSE.Open.Observations;

public static class ObservationCollectionExtensions
{
    public static IEnumerable<T> WhereMeasure<T>(this IEnumerable<T> collection, Measure measure)
        where T : Observation
    {
        return collection.Where(o => o.HasMeasure(measure));
    }

    public static IEnumerable<T> WhereMeasure<T>(this IEnumerable<T> collection, MeasureId id)
        where T : Observation
    {
        return collection.Where(o => o.HasMeasureId(id));
    }

    public static IEnumerable<TObs> WhereValue<TObs, TValue>(
        this IEnumerable<TObs> collection,
        TValue value)
        where TObs : Observation<TValue>
        where TValue : IEquatable<TValue>
    {
        return collection.Where(o => o.Value.Equals(value));
    }

    public static IEnumerable<TObs> WhereMeasureAndValue<TObs, TValue>(
        this IEnumerable<TObs> collection,
        MeasureId id,
        TValue value)
        where TObs : Observation<TValue>
        where TValue : IEquatable<TValue>
    {
        return collection.Where(o => o.HasMeasureId(id) && o.Value.Equals(value));
    }

    public static IEnumerable<TObs> WhereMeasurementAndValue<TObs, TValue, TDisc>(
        this IEnumerable<TObs> collection,
        Measure measure,
        TDisc discriminator,
        TValue value)
        where TObs : Observation<TValue, TDisc>
        where TValue : IEquatable<TValue>
        where TDisc : IEquatable<TDisc>
    {
        return collection.Where(o => o.HasMeasurement(measure, discriminator) && o.Value.Equals(value));
    }
}
