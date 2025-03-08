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

/// <summary>
/// An observation records a value observed and the time of the observation, the meaning of which
/// is defined by a measure and, optionally, one or two parameters.
/// </summary>
/// <remarks>
/// Serialization and deserialization is supported for a variety of pre-specified closed generic
/// types derived from <see cref="Observation{TValue}"/> and <see cref="Observation{TValue, TParam}"/>.
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "d")]
[JsonDerivedType(typeof(Observation<Binary>), (int)MeasureType.Binary)]
[JsonDerivedType(typeof(Observation<Binary, SpeechSound>), (int)MeasureType.BinarySpeechSound)]
[JsonDerivedType(typeof(Observation<Binary, WordId>), (int)MeasureType.BinaryWord)]
[JsonDerivedType(typeof(Observation<Binary, SentenceId>), (int)MeasureType.BinarySentence)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency>), (int)MeasureType.BehaviorFrequency)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency, SpeechSound>), (int)MeasureType.BehaviorFrequencySpeechSound)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency, WordId>), (int)MeasureType.BehaviorFrequencyWord)]
[JsonDerivedType(typeof(Observation<BehaviorFrequency, SentenceId>), (int)MeasureType.BehaviorFrequencySentence)]
[JsonDerivedType(typeof(Observation<Count>), (int)MeasureType.Count)]
[JsonDerivedType(typeof(Observation<Amount>), (int)MeasureType.Amount)]
[JsonDerivedType(typeof(Observation<SpeechClarity>), (int)MeasureType.SpeechClarity)]
[JsonDerivedType(typeof(Observation<SpeechClarity, SpeechSound>), (int)MeasureType.SpeechClaritySpeechSound)]
[JsonDerivedType(typeof(Observation<SpeechClarity, WordId>), (int)MeasureType.SpeechClarityWord)]
[JsonDerivedType(typeof(Observation<SpeechClarity, SentenceId>), (int)MeasureType.SpeechClaritySentence)]
[JsonDerivedType(typeof(Observation<Completeness>), (int)MeasureType.Completeness)]
[JsonDerivedType(typeof(Observation<Completeness, SpeechSound>), (int)MeasureType.CompletenessSpeechSound)]
[JsonDerivedType(typeof(Observation<Completeness, WordId>), (int)MeasureType.CompletenessWord)]
[JsonDerivedType(typeof(Observation<Completeness, SentenceId>), (int)MeasureType.CompletenessSentence)]
public abstract class Observation : IObservation, IEquatable<Observation>, IRepeatableHash64
{
    public static readonly DateTimeOffset MinimumObservationTime =
        new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

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
        Time = timeProvider.GetUtcNow().Truncate(DateTimeTruncation.Millisecond);
        MeasureId = measure.Id;
    }

    /// <summary>
    /// Initializes a new historic observation instance.
    /// </summary>
    /// <param name="measure"></param>
    /// <param name="time"></param>
    /// <param name="timeProvider"></param>
    protected Observation(IMeasure measure, DateTimeOffset time, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);
        Guard.IsInRange(time, MinimumObservationTime, timeProvider.GetUtcNow().AddSeconds(TimeToleranceSeconds));

        Id = ObservationId.GetRandomId();
        Time = time.Truncate(DateTimeTruncation.Millisecond);
        Recorded = timeProvider.GetUtcNow().Truncate(DateTimeTruncation.Millisecond);
        MeasureId = measure.Id;
    }

    /// <summary>
    /// Initializes a new observation instance when deserializing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    /// <param name="recorded"></param>
    /// <param name="measureId"></param>
    /// <param name="timeProvider"></param>
    protected Observation(
        ObservationId id,
        DateTimeOffset time,
        DateTimeOffset? recorded,
        MeasureId measureId,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(id);
        Guard.IsNotDefault(measureId);
        ArgumentNullException.ThrowIfNull(timeProvider);
        Guard.IsInRange(time, MinimumObservationTime, timeProvider.GetUtcNow().AddSeconds(TimeToleranceSeconds));

        if (recorded.HasValue)
        {
            Guard.IsLessThanOrEqualTo(time, recorded.Value);
        }

        Id = id;
        Time = time;
        Recorded = recorded;
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
    /// The time of the observation. This is the time when the observation was made.
    /// </summary>
    [JsonPropertyName("t")]
    [JsonPropertyOrder(-89800)]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public DateTimeOffset Time { get; }

    /// <summary>
    /// The time that the observation was recorded. May be later than <see cref="Time"/> if recording
    /// a historical observation. If <see langword="null"/>, we assume the observation was recorded
    /// at the same time as it was made.
    /// </summary>
    [JsonPropertyName("r")]
    [JsonPropertyOrder(-89000)]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? Recorded { get; }

    /// <summary>
    /// The identifier for the measure.
    /// </summary>
    [JsonPropertyName("m")]
    [JsonPropertyOrder(-88000)]
    public MeasureId MeasureId { get; }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    object IObservation.Value
    {
        get
        {
            var value = GetValueCore();

            if (value is null)
            {
                return ThrowHelper.ThrowInvalidOperationException<object>(
                    $"{nameof(Observation)}.{nameof(GetValueCore)} requires a non-null return value.");
            }

            return value;
        }
    }

    object? IObservation.Parameter => GetParameterCore();

    object? IObservation.Parameter2 => GetParameter2Core();

#pragma warning restore CA1033 // Interface methods should be callable by child types

    protected abstract object GetValueCore();

    protected abstract object? GetParameterCore();

    protected abstract object? GetParameter2Core();

    public abstract double ConvertValueToDouble();

    public abstract decimal ConvertValueToDecimal();

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
        where TValue : struct, IEquatable<TValue>, IObservationValue
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
        where TValue : struct, IEquatable<TValue>, IObservationValue
    {
        return new Observation<TValue>(measure, value, timeProvider);
    }

    public static Observation<TValue> CreateHistorical<TValue>(
        IMeasure<TValue> measure,
        TValue value,
        DateTimeOffset time)
        where TValue : struct, IEquatable<TValue>, IObservationValue
    {
        return CreateHistorical(measure, value, time, TimeProvider.System);
    }

    public static Observation<TValue> CreateHistorical<TValue>(
        IMeasure<TValue> measure,
        TValue value,
        DateTimeOffset time,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>, IObservationValue
    {
        return new Observation<TValue>(measure, value, time, timeProvider);
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
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
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
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
    {
        return new Observation<TValue, TParam>(measure, parameter, value, timeProvider);
    }

    public static Observation<TValue, TParam> CreateHistorical<TValue, TParam>(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        DateTimeOffset time)
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
    {
        return CreateHistorical(measure, parameter, value, time, TimeProvider.System);
    }

    public static Observation<TValue, TParam> CreateHistorical<TValue, TParam>(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        DateTimeOffset time,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
    {
        return new Observation<TValue, TParam>(measure, parameter, value, time, timeProvider);
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
/// An observation that records a value observed and the time of the observation, the meaning of which
/// is defined by a measure.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <remarks>
/// An example of this type of measure is a yes/not measure of "[subject] can sit unsupported".
/// </remarks>
public sealed class Observation<TValue>
    : Observation,
      IObservation<TValue>,
      IObservationFactory<Observation<TValue>, TValue>,
      IEquatable<Observation<TValue>>
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    public Observation(IMeasure measure, TValue value, TimeProvider timeProvider)
        : base(measure, timeProvider)
    {
        Value = value;
    }

    public Observation(IMeasure measure, TValue value, DateTimeOffset time, TimeProvider timeProvider)
        : base(measure, time, timeProvider)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new observation instance when deserializing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    /// <param name="recorded"></param>
    /// <param name="measureId"></param>
    /// <param name="value"></param>
    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Observation(
        ObservationId id,
        DateTimeOffset time,
        DateTimeOffset? recorded,
        MeasureId measureId,
        TValue value)
        : base(id, time, recorded, measureId, TimeProvider.System)
    {
        Value = value;
    }

    internal Observation(
        ObservationId id,
        DateTimeOffset time,
        MeasureId measureId,
        TValue value,
        TimeProvider timeProvider)
        : this(id, time, null, measureId, value, timeProvider)
    {
    }

    internal Observation(
        ObservationId id,
        DateTimeOffset time,
        DateTimeOffset? recorded,
        MeasureId measureId,
        TValue value,
        TimeProvider timeProvider)
        : base(id, time, recorded, measureId, timeProvider)
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

    protected override object GetValueCore()
    {
        return Value;
    }

    protected override object? GetParameterCore()
    {
        return null;
    }

    protected override object? GetParameter2Core()
    {
        return null;
    }

    public override double ConvertValueToDouble()
    {
        return Value.ConvertToDouble();
    }

    public override decimal ConvertValueToDecimal()
    {
        return Value.ConvertToDecimal();
    }
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
public sealed class Observation<TValue, TParam>
    : Observation,
      IObservation<TValue, TParam>,
      IObservationFactory<Observation<TValue, TParam>, TValue, TParam>,
      IEquatable<Observation<TValue, TParam>>
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    public Observation(
        IMeasure measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        : base(measure, timeProvider)
    {
        Parameter = parameter;
        Value = value;
    }

    public Observation(
        IMeasure measure,
        TParam parameter,
        TValue value,
        DateTimeOffset time,
        TimeProvider timeProvider)
        : base(measure, time, timeProvider)
    {
        Parameter = parameter;
        Value = value;
    }

    /// <summary>
    /// Initializes a new observation instance when deserializing.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    /// <param name="recorded"></param>
    /// <param name="measureId"></param>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Observation(
        ObservationId id,
        DateTimeOffset time,
        DateTimeOffset? recorded,
        MeasureId measureId,
        TParam parameter,
        TValue value)
        : base(id, time, recorded, measureId, TimeProvider.System)
    {
        Parameter = parameter;
        Value = value;
    }

    public Observation(
        ObservationId id,
        DateTimeOffset time,
        MeasureId measureId,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        : this(id, time, null, measureId, parameter, value, timeProvider)
    {
    }

    public Observation(
        ObservationId id,
        DateTimeOffset time,
        DateTimeOffset? recorded,
        MeasureId measureId,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
        : base(id, time, recorded, measureId, timeProvider)
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
        return HashCode.Combine(base.GetMeasurementHashCodeCore(), Parameter);
    }

    static Observation<TValue, TParam> IObservationFactory<Observation<TValue, TParam>, TValue, TParam>.Create(
        IMeasure<TValue, TParam> measure,
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
    {
        return Create(measure, parameter, value, timeProvider);
    }

    protected override object GetValueCore()
    {
        return Value;
    }

    protected override object? GetParameterCore()
    {
        return Parameter;
    }

    protected override object? GetParameter2Core()
    {
        return null;
    }

    public override double ConvertValueToDouble()
    {
        return Value.ConvertToDouble();
    }

    public override decimal ConvertValueToDecimal()
    {
        return Value.ConvertToDecimal();
    }
}
