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
    [JsonPropertyName("i")]
    ObservationId Id { get; }

    [JsonPropertyName("m")]
    MeasureId MeasureId { get; }

    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    DateTimeOffset Time { get; }

    int GetMeasurementHashCode();

    [JsonPropertyName("v")]
    object Value { get; }

    [JsonPropertyName("p")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    object? Parameter { get; }

    [JsonPropertyName("p2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    object? Parameter2 { get; }

    double ConvertValueToDouble();

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
