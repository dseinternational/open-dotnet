// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

public abstract record Observation
{
    private readonly DateTimeOffset _time;

    /// <summary>
    /// A randomly generated number between 0 and <see cref="NumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("i")]
    [JsonPropertyOrder(-91000)]
    public ObservationId Id { get; init; } = ObservationId.GetRandomId();

    /// <summary>
    /// The time of the observation.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("t")]
    [JsonPropertyOrder(-89800)]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset Time
    {
        get => _time;
        init => _time = DateTimeOffset.FromUnixTimeMilliseconds(value.ToUnixTimeMilliseconds());
    }

    /// <summary>
    /// The identifier for the measure.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("m")]
    [JsonPropertyOrder(-88000)]
    public required MeasureId MeasureId { get; init; }

    public virtual bool HasMeasure(Measure measure)
    {
        ArgumentNullException.ThrowIfNull(measure);
        return HasMeasureId(measure.Id);
    }

    public bool HasMeasureId(MeasureId measureId)
    {
        return MeasureId == measureId;
    }

    /// <summary>
    /// Gets a value that discriminates between measurement types.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// For 'simple' measures, this may simply be derived from the <see cref="MeasureId"/>. For types with additional
    /// identifying/discriminating state, additional values may need to be taken into account.
    /// </remarks>
    public virtual ulong GetMeasurementId()
    {
        return MeasureId;
    }

    /// <summary>
    /// Gets a code that discriminates between measurement types and is suitable for use as a hash code.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// For 'simple' measures, this may simply be derived from the <see cref="MeasureId"/>. For types with additional
    /// identifying/discriminating state, additional values may need to be taken into account.
    /// </remarks>
    public virtual int GetMeasurementHashCode()
    {
        return HashCode.Combine(GetMeasurementId());
    }
}

/// <summary>
/// An observation for a measure that records a single measurement value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract record Observation<TValue> : Observation
    where TValue : struct, IEquatable<TValue>
{
    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public required TValue Value { get; init; }
}

/// <summary>
/// An observation for a measure that records a single discriminated measurement value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TDisc"></typeparam>
public abstract record Observation<TValue, TDisc> : Observation<Discriminated<TValue, TDisc>>
    where TValue : struct, IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    public virtual bool HasMeasurement(Measure measure, TDisc discriminator)
    {
        ArgumentNullException.ThrowIfNull(measure);
        return HasMeasureId(measure.Id) && Value.Discriminator.Equals(discriminator);
    }

    protected abstract ulong GetDiscriminatorId();

    public override ulong GetMeasurementId()
    {
        return MeasureId ^ GetDiscriminatorId();
    }

    public override int GetMeasurementHashCode()
    {
        return HashCode.Combine(MeasureId, Value.Discriminator);
    }
}
