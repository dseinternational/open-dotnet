// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

namespace DSE.Open.Observations;

public static class SnapshotCollectionExtensions
{
    public static IEnumerable<T> WhereMeasure<T>(this IEnumerable<T> collection, Measure measure)
        where T : Snapshot
    {
        return collection.Where(s => s.HasMeasure(measure));
    }

    public static IEnumerable<T> WhereMeasure<T>(this IEnumerable<T> collection, MeasureId id)
        where T : Snapshot
    {
        return collection.Where(s => s.HasMeasureId(id));
    }

    public static IEnumerable<TSnapshot> WhereValue<TSnapshot, TObs, TValue>(
        this IEnumerable<TSnapshot> collection,
        TValue value)
        where TSnapshot : Snapshot<TObs, TValue>
        where TObs : Observation<TValue>
        where TValue : IEquatable<TValue>
    {
        return collection.Where(s => s.Observation.Value.Equals(value));
    }

    public static IEnumerable<TSnapshot> WhereMeasureAndValue<TSnapshot, TObs, TValue>(
        this IEnumerable<TSnapshot> collection,
        Measure meaure,
        TValue value)
        where TSnapshot : Snapshot<TObs, TValue>
        where TObs : Observation<TValue>
        where TValue : IEquatable<TValue>
    {
        return collection.Where(s => s.HasMeasure(meaure) && s.Observation.Value.Equals(value));
    }

    public static IEnumerable<TSnapshot> WhereMeasureAndValue<TSnapshot, TObs, TValue>(
        this IEnumerable<TSnapshot> collection,
        MeasureId id,
        TValue value)
        where TSnapshot : Snapshot<TObs, TValue>
        where TObs : Observation<TValue>
        where TValue : IEquatable<TValue>
    {
        return collection.Where(s => s.HasMeasureId(id) && s.Observation.Value.Equals(value));
    }

    public static IEnumerable<TSnapshot> WhereMeasurementAndValue<TSnapshot, TObs, TValue, TDisc>(
        this IEnumerable<TSnapshot> collection,
        Measure measure,
        TDisc discriminator,
        TValue value)
        where TSnapshot : Snapshot<TObs, TValue, TDisc>
        where TObs : Observation<TValue, TDisc>
        where TValue : IEquatable<TValue>
        where TDisc : IEquatable<TDisc>
    {
        return collection.Where(s => s.HasMeasurement(measure, discriminator) && s.Observation.Value.Equals(value));
    }
}
