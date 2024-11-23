
// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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
        where TSnapshot : Snapshot<TObs>
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Where(s => s.Observation.Value.Equals(value));
    }

    public static IEnumerable<TSnapshot> WhereMeasureAndValue<TSnapshot, TObs, TValue>(
        this IEnumerable<TSnapshot> collection,
        Measure measure,
        TValue value)
        where TSnapshot : Snapshot<TObs>
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Where(s => s.HasMeasure(measure) && s.Observation.Value.Equals(value));
    }

    public static bool AnyWithMeasureAndValue<TSnapshot, TObs, TValue>(
        this IEnumerable<TSnapshot> collection,
        Measure measure,
        TValue value)
        where TSnapshot : Snapshot<TObs>
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Any(s => s.HasMeasure(measure) && s.Observation.Value.Equals(value));
    }

    public static IEnumerable<TSnapshot> WhereMeasureAndValue<TSnapshot, TObs, TValue>(
        this IEnumerable<TSnapshot> collection,
        MeasureId id,
        TValue value)
        where TSnapshot : Snapshot<TObs>
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Where(s => s.HasMeasureId(id) && s.Observation.Value.Equals(value));
    }
}
