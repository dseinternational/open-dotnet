// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;
using DSE.Open.Speech;
using DSE.Open.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "d")]
[JsonDerivedType(typeof(Observation<bool>), (int)ObservationType.Binary)]
[JsonDerivedType(typeof(Observation<bool, SpeechSound>), (int)ObservationType.BinarySpeechSound)]
[JsonDerivedType(typeof(Observation<bool, WordId>), (int)ObservationType.BinaryWord)]
[JsonDerivedType(typeof(Observation<bool, SentenceId>), (int)ObservationType.BinarySentence)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency>), (int)ObservationType.BehaviorFrequency)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency, SpeechSound>), (int)ObservationType.BehaviorFrequencySpeechSound)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency, WordId>), (int)ObservationType.BehaviorFrequencyWord)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency, SentenceId>), (int)ObservationType.BehaviorFrequencySentence)]
[JsonDerivedType(typeof(Observation<Count>), (int)ObservationType.Count)]
[JsonDerivedType(typeof(Observation<SpeechClarity>), (int)ObservationType.SpeechClarity)]
[JsonDerivedType(typeof(Observation<SpeechClarity, SpeechSound>), (int)ObservationType.SpeechClaritySpeechSound)]
[JsonDerivedType(typeof(Observation<SpeechClarity, WordId>), (int)ObservationType.SpeechClarityWord)]
[JsonDerivedType(typeof(Observation<SpeechClarity, SentenceId>), (int)ObservationType.SpeechClaritySentence)]
[JsonDerivedType(typeof(Observation<Completeness>), (int)ObservationType.Completeness)]
[JsonDerivedType(typeof(Observation<Completeness, SpeechSound>), (int)ObservationType.CompletenessSpeechSound)]
[JsonDerivedType(typeof(Observation<Completeness, WordId>), (int)ObservationType.CompletenessWord)]
[JsonDerivedType(typeof(Observation<Completeness, SentenceId>), (int)ObservationType.CompletenessSentence)]
public abstract record Observation : IObservation
{
    private static readonly DateTimeOffset s_minTime = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private const int TimeToleranceSeconds = 60;

    /// <summary>
    /// Initializes a new observation instance.
    /// </summary>
    /// <param name="measure"></param>
    /// <param name="timeProvider"></param>
    protected Observation(IMeasure measure, TimeProvider timeProvider)
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
    /// Creates an observation with a single measurement value.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="measure"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Observation<TValue> Create<TValue>(
        IMeasure<TValue> measure,
        TValue value)
        where TValue : struct, IEquatable<TValue>
    {
        return Create(measure, value, TimeProvider.System);
    }

    /// <summary>
    /// Creates an observation with a single measurement value.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="measure"></param>
    /// <param name="value"></param>
    /// <param name="timeProvider"></param>
    /// <returns></returns>
    public static Observation<TValue> Create<TValue>(
        IMeasure<TValue> measure,
        TValue value,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>
    {
        return new Observation<TValue>(measure, value, timeProvider);
    }

    /// <summary>
    /// Creates an observation with a single measurement value and a single parameter.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <param name="measure"></param>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Observation<TValue, TParam> Create<TValue, TParam>(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value)
        where TValue : struct, IEquatable<TValue>
        where TParam : IEquatable<TParam>
    {
        return Create(measure, parameter, value, TimeProvider.System);
    }

    /// <summary>
    /// Creates an observation with a single measurement value and a single parameter.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <param name="measure"></param>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    /// <param name="timeProvider"></param>
    /// <returns></returns>
    public static Observation<TValue, TParam> Create<TValue, TParam>(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>
        where TParam : IEquatable<TParam>
    {
        return new Observation<TValue, TParam>(measure, parameter, value, timeProvider);
    }
}

/// <summary>
/// An observation with a single measurement value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public sealed record Observation<TValue>
    : Observation,
      IObservation<TValue>,
      IObservationFactory<Observation<TValue>, TValue>
    where TValue : struct, IEquatable<TValue>
{
    public Observation(IMeasure measure, TValue value, TimeProvider timeProvider)
        : base(measure, timeProvider)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new observation instance when deserializing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    /// <param name="measureId"></param>
    /// <param name="value"></param>
    [JsonConstructor]
    public Observation(ObservationId id, DateTimeOffset time, MeasureId measureId, TValue value)
        : this(id, time, measureId, value, TimeProvider.System)
    {
    }

    internal Observation(ObservationId id, DateTimeOffset time, MeasureId measureId, TValue value, TimeProvider timeProvider)
        : base(id, time, measureId, timeProvider)
    {
        Value = value;
    }

    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public TValue Value { get; }

    static Observation<TValue> IObservationFactory<Observation<TValue>, TValue>.Create(
        IMeasure<TValue> measure,
        TValue value,
        TimeProvider timeProvider)
    {
        return Create(measure, value, timeProvider);
    }
}

/// <summary>
/// An observation with a single measurement value and a single parameter.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TParam"></typeparam>
public sealed record Observation<TValue, TParam>
    : Observation,
      IObservation<TValue, TParam>,
      IObservationFactory<Observation<TValue, TParam>, TValue, TParam>
    where TValue : struct, IEquatable<TValue>
    where TParam : IEquatable<TParam>
{
    internal Observation(
        IMeasure measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider) : base(measure, timeProvider)
    {
        Parameter = parameter;
        Value = value;
    }

    /// <summary>
    /// Initializes a new observation instance when deserializing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    /// <param name="measureId"></param>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    [JsonConstructor]
    public Observation(
        ObservationId id,
        DateTimeOffset time,
        MeasureId measureId,
        TParam parameter,
        TValue value)
        : this(id, time, measureId, parameter, value, TimeProvider.System)
    {
    }

    internal Observation(
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

    static Observation<TValue, TParam> IObservationFactory<Observation<TValue, TParam>, TValue, TParam>.Create(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
    {
        return Create(measure, parameter, value, timeProvider);
    }
}
