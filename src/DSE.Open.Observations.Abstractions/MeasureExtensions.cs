// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class MeasureExtensions
{
    public static TObs CreateObservation<TObs, TValue>(this IMeasure<TValue> measure, TValue value)
        where TObs : IObservation<TValue>, IObservationFactory<TObs, TValue>
        where TValue : struct, IEquatable<TValue>, IObservationValue
    {
        ArgumentNullException.ThrowIfNull(measure);
        return measure.CreateObservation<TObs>(value, TimeProvider.System);
    }

    public static TObs CreateObservation<TObs, TValue, TParam>(this IMeasure<TValue, TParam> measure, TValue value, TParam parameter)
        where TObs : IObservation<TValue, TParam>, IObservationFactory<TObs, TValue, TParam>
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
    {
        ArgumentNullException.ThrowIfNull(measure);
        return measure.CreateObservation<TObs>(parameter, value, TimeProvider.System);
    }
}
