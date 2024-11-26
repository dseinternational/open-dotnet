// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Language;
using DSE.Open.Speech;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "d")]
[JsonDerivedType(typeof(Observation<Binary>), (int)ObservationType.Binary)]
[JsonDerivedType(typeof(Observation<Binary, SpeechSound>), (int)ObservationType.BinarySpeechSound)]
[JsonDerivedType(typeof(Observation<Binary, WordId>), (int)ObservationType.BinaryWord)]
[JsonDerivedType(typeof(Observation<Binary, SentenceId>), (int)ObservationType.BinarySentence)]
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
public abstract class Observation : IObservation, IEquatable<Observation>, IRepeatableHash64
{
    public static readonly DateTimeOffset MinimumObservationTime = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
    internal const int TimeToleranceSeconds = 60;

    private int? _measurementHashCode;

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
        Guard.IsInRange(time, MinimumObservationTime, timeProvider.GetUtcNow().AddSeconds(TimeToleranceSeconds));

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
    /// Gets a repeatable hash value that represents the measurement.
    /// </summary>
    /// <returns>A repeatable hash value that represents the measurement</returns>
    /// <remarks>
    /// For an observation with no paramaters, this is simply a repeatable hash of the <see cref="MeasureId"/>.
    /// <para>For observations with parameters, the hash should incorporate parameters with the measure id.</para>
    /// </remarks>
    public int GetMeasurementHashCode()
    {
        return _measurementHashCode ??= GetMeasurementHashCodeCore();
    }

    protected virtual int GetMeasurementHashCodeCore()
    {
        return HashCode.Combine(MeasureId);
    }

    public virtual bool Equals([NotNullWhen(true)] Observation? other)
    {
        return other is not null &&
               Id == other.Id &&
               Time == other.Time &&
               MeasureId == other.MeasureId;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return Equals(obj as Observation);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Time, MeasureId);
    }

    public override string ToString()
    {
        return $"{{ id: {Id}, time: {Time:u}, measure: {MeasureId} }}";
    }

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
        where TValue : struct, IEquatable<TValue>, IValueProvider
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
    internal static Observation<TValue> Create<TValue>(
        IMeasure<TValue> measure,
        TValue value,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>, IValueProvider
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
        where TValue : struct, IEquatable<TValue>, IValueProvider
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
    internal static Observation<TValue, TParam> Create<TValue, TParam>(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>, IValueProvider
        where TParam : IEquatable<TParam>
    {
        return new Observation<TValue, TParam>(measure, parameter, value, timeProvider);
    }

    public virtual ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.CombineHashCodes(
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Id),
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Time),
            RepeatableHash64Provider.Default.GetRepeatableHashCode(MeasureId));
    }
}

/// <summary>
/// An observation with a single measurement value.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public sealed class Observation<TValue>
    : Observation,
      IObservation<TValue>,
      IObservationFactory<Observation<TValue>, TValue>,
      IEquatable<Observation<TValue>>
    where TValue : struct, IEquatable<TValue>, IValueProvider
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
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Observation(ObservationId id, DateTimeOffset time, MeasureId measureId, TValue value)
        : base(id, time, measureId, TimeProvider.System)
    {
        Value = value;
    }

    internal Observation(
        ObservationId id,
        DateTimeOffset time,
        MeasureId measureId,
        TValue value,
        TimeProvider timeProvider)
        : base(id, time, measureId, timeProvider)
    {
        Value = value;
    }

    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public TValue Value { get; }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return Equals(obj as Observation<TValue>);
    }

    public override bool Equals([NotNullWhen(true)] Observation? other)
    {
        return Equals(other as Observation<TValue>);
    }

    public bool Equals([NotNullWhen(true)] Observation<TValue>? other)
    {
        return other is not null &&
               Value.Equals(other.Value) &&
               base.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, base.GetHashCode());
    }

    public override ulong GetRepeatableHashCode()
    {
        if (!RepeatableHash64Provider.Default.TryGetRepeatableHashCode(Value, out var valueHash))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"The {typeof(TValue).Name} type does not support repeatable hashing.");
            return 0u;
        }

        return RepeatableHash64Provider.Default.CombineHashCodes(
            base.GetRepeatableHashCode(),
            valueHash);
    }

    public override string ToString()
    {
        return $"{{ id: {Id}, time: {Time:u}, measure: {MeasureId}, value: {Value} }}";
    }

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
public sealed class Observation<TValue, TParam>
    : Observation,
      IObservation<TValue, TParam>,
      IObservationFactory<Observation<TValue, TParam>, TValue, TParam>,
      IEquatable<Observation<TValue, TParam>>
    where TValue : struct, IEquatable<TValue>, IValueProvider
    where TParam : IEquatable<TParam>
{
    internal Observation(
        IMeasure measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        : base(measure, timeProvider)
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
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Observation(
        ObservationId id,
        DateTimeOffset time,
        MeasureId measureId,
        TParam parameter,
        TValue value)
        : base(id, time, measureId, TimeProvider.System)
    {
        Parameter = parameter;
        Value = value;
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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return Equals(obj as Observation<TValue, TParam>);
    }

    public override bool Equals([NotNullWhen(true)] Observation? other)
    {
        return Equals(other as Observation<TValue, TParam>);
    }

    public bool Equals([NotNullWhen(true)] Observation<TValue, TParam>? other)
    {
        return other is not null &&
               Parameter.Equals(other.Parameter) &&
               Value.Equals(other.Value) &&
               base.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, base.GetHashCode());
    }

    public override ulong GetRepeatableHashCode()
    {
        if (!RepeatableHash64Provider.Default.TryGetRepeatableHashCode(Parameter, out var paramHash))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"The {typeof(TParam).Name} type does not support repeatable hashing.");
            return 0u;
        }

        return RepeatableHash64Provider.Default.CombineHashCodes(
            base.GetRepeatableHashCode(),
            paramHash);
    }

    public override string ToString()
    {
        return $"{{ id: {Id}, time: {Time:u}, measure: {MeasureId}, parameter: {Parameter}, value: {Value} }}";
    }

    /// <inheritdoc />
    protected override int GetMeasurementHashCodeCore()
    {
        return HashCode.Combine(base.GetMeasurementHashCodeCore, Parameter);
    }

    static Observation<TValue, TParam> IObservationFactory<Observation<TValue, TParam>, TValue, TParam>.Create(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
    {
        return Create(measure, parameter, value, timeProvider);
    }
}
