// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// An observation records a value observed and the time of the observation, the meaning of which
/// is defined by a measure and, optionally, one or two parameters.
/// </summary>
public interface IObservation
{
    ObservationId Id { get; }

    MeasureId MeasureId { get; }

    DateTimeOffset Time { get; }

    int GetMeasurementHashCode();

    object Value { get; }

    object? Parameter { get; }

    object? Parameter2 { get; }
}

/// <summary>
/// An observation that records a value observed and the time of the observation, the meaning of which
/// is defined by a measure.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// An example of this type of measure is a yes/not measure of "[subject] can sit unsupported".
/// </remarks>
public interface IObservation<out TValue> : IObservation
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    new TValue Value { get; }
}

/// <summary>
/// An observation that records a value observed and the time of the observation, the meaning of which
/// is defined by a measure and a single parameter.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TParam"></typeparam>
/// <remarks>
/// An example of this type of measure is a yes/no measure of "[subject] can say [word] independently"
/// (where [word] is the parameter value).
/// </remarks>
public interface IObservation<out TValue, out TParam> : IObservation
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    new TParam Parameter { get; }

    new TValue Value { get; }
}
