// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// An observation records a value observed and the time of the observation, the meaning of which
/// is defined by a measure and, optionally, one or two parameters.
/// </summary>
public interface IObservation
{
    /// <summary>
    /// Gets the identifier for this observation.
    /// </summary>
    [JsonPropertyName("i")]
    ObservationId Id { get; }

    /// <summary>
    /// Gets the identifier of the measure that defines the meaning of the observation.
    /// </summary>
    [JsonPropertyName("m")]
    MeasureId MeasureId { get; }

    /// <summary>
    /// Gets the time of the observation.
    /// </summary>
    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    DateTimeOffset Time { get; }

    /// <summary>
    /// Returns a hash code that incorporates the measure, parameter(s) and value of the
    /// observation, and that may be used to compare observations for measurement equivalence.
    /// </summary>
    int GetMeasurementHashCode();

    /// <summary>
    /// Gets the observed value.
    /// </summary>
    [JsonPropertyName("v")]
    object Value { get; }

    /// <summary>
    /// Gets the parameter associated with the observation, if any.
    /// </summary>
    [JsonPropertyName("p")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    object? Parameter { get; }

    /// <summary>
    /// Gets the second parameter associated with the observation, if any.
    /// </summary>
    [JsonPropertyName("p2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    object? Parameter2 { get; }

    /// <summary>
    /// Converts the observed value to a <see cref="double"/>.
    /// </summary>
    double ConvertValueToDouble();

    /// <summary>
    /// Converts the observed value to a <see cref="decimal"/>.
    /// </summary>
    decimal ConvertValueToDecimal();
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
    /// <summary>
    /// Gets the observed value.
    /// </summary>
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
    /// <summary>
    /// Gets the parameter associated with the observation.
    /// </summary>
    new TParam Parameter { get; }

    /// <summary>
    /// Gets the observed value.
    /// </summary>
    new TValue Value { get; }
}
