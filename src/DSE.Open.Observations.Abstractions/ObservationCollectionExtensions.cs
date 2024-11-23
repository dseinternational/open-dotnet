// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

namespace DSE.Open.Observations;

public static class ObservationCollectionExtensions
{
    public static IEnumerable<T> WhereMeasure<T>(this IEnumerable<T> collection, Measure measure)
        where T : Observation
    {
        return collection.Where(o => o.MeasureId == measure.Id);
    }

    public static IEnumerable<T> WhereMeasureId<T>(this IEnumerable<T> collection, MeasureId id)
        where T : Observation
    {
        return collection.Where(o => o.MeasureId == id);
    }

    public static IEnumerable<TObs> WhereValue<TObs, TValue>(
        this IEnumerable<TObs> collection,
        TValue value)
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Where(o => o.Value.Equals(value));
    }

    public static IEnumerable<TObs> WhereMeasureAndValue<TObs, TValue>(
        this IEnumerable<TObs> collection,
        Measure measure,
        TValue value)
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Where(o => o.MeasureId == measure.Id && o.Value.Equals(value));
    }

    public static IEnumerable<TObs> WhereMeasureIdAndValue<TObs, TValue>(
        this IEnumerable<TObs> collection,
        MeasureId id,
        TValue value)
        where TObs : Observation<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        return collection.Where(o => o.MeasureId == id && o.Value.Equals(value));
    }

    public static IEnumerable<TObs> WhereMeasurement<TObs, TValue, TParam>(
        this IEnumerable<TObs> collection,
        Measure measure,
        TParam parameter)
        where TObs : Observation<TValue, TParam>
        where TValue : struct, IEquatable<TValue>
        where TParam : IEquatable<TParam>
    {
        return collection.Where(o => o.MeasureId == measure.Id && o.Parameter.Equals(parameter));
    }

    public static IEnumerable<TObs> WhereMeasurementAndValue<TObs, TValue, TParam>(
        this IEnumerable<TObs> collection,
        Measure measure,
        TParam parameter,
        TValue value)
        where TObs : Observation<TValue, TParam>
        where TValue : struct, IEquatable<TValue>
        where TParam : IEquatable<TParam>
    {
        return collection.Where(o => o.MeasureId == measure.Id
            && o.Parameter.Equals(parameter) && value.Equals(value));
    }
}
