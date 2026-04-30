// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Defines a measure - the meaning of an observed value.
/// </summary>
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

    /// <summary>
    /// Gets the level of measurement of the measure.
    /// </summary>
    MeasurementLevel MeasurementLevel { get; }

    /// <summary>
    /// Gets the name of the measure.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a statement describing what is being measured.
    /// </summary>
    string Statement { get; }

    /// <summary>
    /// The order in which the measure should be displayed.
    /// </summary>
    uint Sequence { get; }
}

/// <summary>
/// Defines a measure that produces observations of values of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue">The type of value observed.</typeparam>
public interface IMeasure<TValue> : IMeasure
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    /// <summary>
    /// Creates an observation of the specified value with the current time taken from
    /// <see cref="TimeProvider.System"/>.
    /// </summary>
    /// <param name="value">The observed value.</param>
    /// <returns>A new <see cref="IObservation{TValue}"/>.</returns>
    IObservation<TValue> CreateObservation(TValue value)
    {
        return CreateObservation(value, TimeProvider.System);
    }

    /// <summary>
    /// Creates an observation of the specified value with the current time taken from
    /// the specified <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="value">The observed value.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> used to obtain the observation time.</param>
    /// <returns>A new <see cref="IObservation{TValue}"/>.</returns>
    IObservation<TValue> CreateObservation(TValue value, TimeProvider timeProvider);
}

/// <summary>
/// Defines a measure that produces observations of values of type <typeparamref name="TValue"/>
/// with a parameter of type <typeparamref name="TParam"/>.
/// </summary>
/// <typeparam name="TValue">The type of value observed.</typeparam>
/// <typeparam name="TParam">The type of the parameter.</typeparam>
public interface IMeasure<TValue, TParam> : IMeasure
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    /// <summary>
    /// Creates an observation of the specified value and parameter with the current time taken
    /// from <see cref="TimeProvider.System"/>.
    /// </summary>
    /// <param name="parameter">The parameter value.</param>
    /// <param name="value">The observed value.</param>
    /// <returns>A new <see cref="IObservation{TValue, TParam}"/>.</returns>
    IObservation<TValue, TParam> CreateObservation(TParam parameter, TValue value)
    {
        return CreateObservation(parameter, value, TimeProvider.System);
    }

    /// <summary>
    /// Creates an observation of the specified value and parameter with the current time taken
    /// from the specified <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="parameter">The parameter value.</param>
    /// <param name="value">The observed value.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> used to obtain the observation time.</param>
    /// <returns>A new <see cref="IObservation{TValue, TParam}"/>.</returns>
    IObservation<TValue, TParam> CreateObservation(TParam parameter, TValue value, TimeProvider timeProvider);
}
