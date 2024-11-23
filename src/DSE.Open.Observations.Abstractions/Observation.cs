// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

public abstract record Observation
{
    private static readonly DateTimeOffset s_minTime = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private const int TimeToleranceSeconds = 60;

    /// <summary>
    /// Initializes a new observation instance.
    /// </summary>
    /// <param name="measure"></param>
    /// <param name="timeProvider"></param>
    protected Observation(Measure measure, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        Id = ObservationId.GetRandomId();
        Time = DateTimeOffset.FromUnixTimeMilliseconds(timeProvider.GetUtcNow().ToUnixTimeMilliseconds());
        MeasureId = measure.Id;
    }

    /// <summary>
    /// Initializes a new observation instance when deserializing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    /// <param name="measureId"></param>
    /// <param name="timeProvider"></param>
    protected Observation(ObservationId id, DateTimeOffset time, MeasureId measureId, TimeProvider timeProvider)
    {
        Guard.IsNotDefault(id);
        Guard.IsNotDefault(measureId);
        ArgumentNullException.ThrowIfNull(timeProvider);
        Guard.IsInRange(time, s_minTime, timeProvider.GetUtcNow().AddSeconds(TimeToleranceSeconds));

        Id = id;
        Time = time;
        MeasureId = measureId;
    }

    /// <summary>
    /// A randomly generated number between 0 and <see cref="NumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation.
    /// </summary>
    [JsonPropertyName("i")]
    [JsonPropertyOrder(-91000)]
    public ObservationId Id { get; }

    /// <summary>
    /// The time of the observation.
    /// </summary>
    [JsonPropertyName("t")]
    [JsonPropertyOrder(-89800)]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public DateTimeOffset Time { get; }

    /// <summary>
    /// The identifier for the measure.
    /// </summary>
    [JsonPropertyName("m")]
    [JsonPropertyOrder(-88000)]
    public MeasureId MeasureId { get; }

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
/// An observation with a single measurement value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public record Observation<TValue> : Observation
    where TValue : struct, IEquatable<TValue>
{
    public Observation(Measure<TValue> measure, TValue value, TimeProvider timeProvider)
        : base(measure, timeProvider)
    {
        Value = value;
    }

    [JsonConstructor]
    protected Observation(ObservationId id, DateTimeOffset time, MeasureId measureId, TValue value, TimeProvider timeProvider)
        : base(id, time, measureId, timeProvider)
    {
        Value = value;
    }

    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public TValue Value { get; }
}

/// <summary>
/// An observation with a single measurement value and a single parameter.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TParam"></typeparam>
public record Observation<TValue, TParam> : Observation
    where TValue : struct, IEquatable<TValue>
    where TParam : IEquatable<TParam>
{
    public Observation(
        Measure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider) : base(measure, timeProvider)
    {
        Parameter = parameter;
        Value = value;
    }

    [JsonConstructor]
    protected Observation(
        ObservationId id,
        DateTimeOffset time,
        MeasureId measureId,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        : base(id, time, measureId, timeProvider)
    {
        Parameter = parameter;
        Value = value;
    }

    [JsonPropertyName("p")]
    [JsonPropertyOrder(-100)]
    public TParam Parameter { get; }

    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public TValue Value { get; }
}
