// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Serialization.DataTransfer;

namespace DSE.Open.Observations;

public abstract record Observation : ImmutableDataTransferObject
{
    protected Observation(Measure measure, DateTimeOffset time)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ObservationsValidator.EnsureMinimumObservationTime(time);

        Id = ObservationId.GetRandomId();
        MeasureId = measure.Id;
        Timestamp = time.ToUnixTimeMilliseconds();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected Observation(ObservationId id, MeasureId measureId, long timestamp)
    {
        Id = id;
        MeasureId = measureId;
        Timestamp = timestamp;
    }

    /// <summary>
    /// A randomly generated number between 0 and <see cref="NumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("i")]
    [JsonPropertyOrder(-91000)]
    public ObservationId Id { get; }

    /// <summary>
    /// The time of the observation.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset Time => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp);

    // this ensures equality tests are the same before/after serialization

    [JsonInclude]
    [JsonPropertyName("t")]
    [JsonPropertyOrder(-89800)]
    internal long Timestamp { get; }

    /// <summary>
    /// The identifier for the measure.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("m")]
    [JsonPropertyOrder(-88000)]
    public MeasureId MeasureId { get; }

    public bool HasMeasure(Measure measure)
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
    where TValue : IEquatable<TValue>
{
    protected Observation(Measure measure, DateTimeOffset time, TValue value)
        : base(measure, time)
    {
        Value = value;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected Observation(ObservationId id, MeasureId measureId, long timestamp, TValue value)
        : base(id, measureId, timestamp)
    {
        Value = value;
    }

    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public TValue Value { get; }
}

/// <summary>
/// An observation for a measure that records a single measurement value and a discriminator value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TDisc"></typeparam>
public abstract record Observation<TValue, TDisc> : Observation<TValue>
    where TValue : IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    protected Observation(Measure measure, TDisc discriminator, DateTimeOffset time, TValue value)
        : base(measure, time, value)
    {
        Discriminator = discriminator;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected Observation(ObservationId id, MeasureId measureId, TDisc discriminator, long timestamp, TValue value)
        : base(id, measureId, timestamp, value)
    {
        Discriminator = discriminator;
    }

    [JsonPropertyName("d")]
    [JsonPropertyOrder(-100)]
    public TDisc Discriminator { get; }

    public virtual bool HasMeasurement(Measure measure, TDisc discriminator)
    {
        ArgumentNullException.ThrowIfNull(measure);
        return HasMeasureId(measure.Id) && Discriminator.Equals(discriminator);
    }

    protected abstract ulong GetDiscriminatorId();

    public override ulong GetMeasurementId()
    {
        return MeasureId ^ GetDiscriminatorId();
    }

    public override int GetMeasurementHashCode()
    {
        return HashCode.Combine(MeasureId, Discriminator);
    }
}
