// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface IObservation
{
    ObservationId Id { get; }

    MeasureId MeasureId { get; }

    DateTimeOffset Time { get; }

    ulong GetMeasurementHashCode();
}

public interface IObservation<TValue> : IObservation
    where TValue : struct, IEquatable<TValue>
{
    TValue Value { get; }
}

public interface IObservation<TValue, TParam> : IObservation
    where TValue : struct, IEquatable<TValue>
    where TParam : IEquatable<TParam>
{
    TParam Parameter { get; }

    TValue Value { get; }
}
