// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public interface IObservation
{
    ObservationId Id { get; }

    MeasureId MeasureId { get; }

    DateTimeOffset Time { get; }

    int GetMeasurementHashCode();

    object Value { get; }

    object? Parameter1 { get; }

    object? Parameter2 { get; }
}

public interface IObservation<out TValue> : IObservation
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    new TValue Value { get; }
}

public interface IObservation<out TValue, out TParam> : IObservation
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    new TParam Parameter1 { get; }

    new TValue Value { get; }
}

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

public interface IObservation<out TValue, out TParam1, out TParam2> : IObservation
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam1 : IEquatable<TParam1>
    where TParam2 : IEquatable<TParam2>
{
    new TParam1 Parameter1 { get; }

    new TParam2 Parameter2 { get; }

    new TValue Value { get; }
}
