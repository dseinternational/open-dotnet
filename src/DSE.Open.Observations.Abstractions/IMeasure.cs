// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface IMeasure
{
    /// <summary>
    /// An identifier for the measure. This is generated from a predictable hash of the <see cref="Uri" />.
    /// </summary>
    MeasureId Id { get; }

    /// <summary>
    /// A URI that uniquely identifies the measure.
    /// </summary>
    Uri Uri { get; }

    MeasurementLevel MeasurementLevel { get; }

    string Name { get; }

    string Statement { get; }
}

public interface IMeasure<TValue> : IMeasure
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    IObservation<TValue> CreateObservation(TValue value)
    {
        return CreateObservation(value, TimeProvider.System);
    }

    IObservation<TValue> CreateObservation(TValue value, TimeProvider timeProvider);
}

public interface IMeasure<TValue, TParam> : IMeasure
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    IObservation<TValue, TParam> CreateObservation(TParam parameter, TValue value)
    {
        return CreateObservation(parameter, value, TimeProvider.System);
    }

    IObservation<TValue, TParam> CreateObservation(TParam parameter, TValue value, TimeProvider timeProvider);
}
