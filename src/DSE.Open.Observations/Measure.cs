// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// A measure represents a statement about a subject that is assigned a value by an observation.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Measure<Binary>), (int)MeasureType.Binary)]
[JsonDerivedType(typeof(Measure<Binary, SpeechSound>), (int)MeasureType.BinarySpeechSound)]
[JsonDerivedType(typeof(Measure<Binary, WordId>), (int)MeasureType.BinaryWord)]
[JsonDerivedType(typeof(Measure<Binary, SentenceId>), (int)MeasureType.BinarySentence)]
[JsonDerivedType(typeof(Measure<BehaviorFrequency>), (int)MeasureType.BehaviorFrequency)]
[JsonDerivedType(typeof(Measure<BehaviorFrequency, SpeechSound>), (int)MeasureType.BehaviorFrequencySpeechSound)]
[JsonDerivedType(typeof(Measure<BehaviorFrequency, WordId>), (int)MeasureType.BehaviorFrequencyWord)]
[JsonDerivedType(typeof(Measure<BehaviorFrequency, SentenceId>), (int)MeasureType.BehaviorFrequencySentence)]
[JsonDerivedType(typeof(Measure<Count>), (int)MeasureType.Count)]
[JsonDerivedType(typeof(Measure<Amount>), (int)MeasureType.Amount)]
[JsonDerivedType(typeof(Measure<SpeechClarity>), (int)MeasureType.SpeechClarity)]
[JsonDerivedType(typeof(Measure<SpeechClarity, SpeechSound>), (int)MeasureType.SpeechClaritySpeechSound)]
[JsonDerivedType(typeof(Measure<SpeechClarity, WordId>), (int)MeasureType.SpeechClarityWord)]
[JsonDerivedType(typeof(Measure<SpeechClarity, SentenceId>), (int)MeasureType.SpeechClaritySentence)]
[JsonDerivedType(typeof(Measure<Completeness>), (int)MeasureType.Completeness)]
[JsonDerivedType(typeof(Measure<Completeness, SpeechSound>), (int)MeasureType.CompletenessSpeechSound)]
[JsonDerivedType(typeof(Measure<Completeness, WordId>), (int)MeasureType.CompletenessWord)]
[JsonDerivedType(typeof(Measure<Completeness, SentenceId>), (int)MeasureType.CompletenessSentence)]
public abstract class Measure : IMeasure, IEquatable<Measure>
{
    /// <summary>
    /// Initializes a new <see cref="Measure"/> with an identifier derived from <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">A URI that uniquely identifies the measure.</param>
    /// <param name="measurementLevel">The measurement level of the values produced by this measure.</param>
    /// <param name="name">A short human-readable name for the measure.</param>
    /// <param name="statement">The statement assigned a value by an observation of this measure.</param>
    /// <param name="sequence">The order in which the measure should be displayed.</param>
    protected Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement, uint sequence)
        : this(MeasureId.FromUri(uri), uri, measurementLevel, name, statement, sequence)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Measure"/> with an explicit identifier.
    /// </summary>
    /// <param name="id">The identifier for the measure.</param>
    /// <param name="uri">A URI that uniquely identifies the measure.</param>
    /// <param name="measurementLevel">The measurement level of the values produced by this measure.</param>
    /// <param name="name">A short human-readable name for the measure.</param>
    /// <param name="statement">The statement assigned a value by an observation of this measure.</param>
    /// <param name="sequence">The order in which the measure should be displayed.</param>
    protected Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement, uint sequence)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(statement);

        Id = id;
        Uri = uri;
        MeasurementLevel = measurementLevel;
        Name = name;
        Statement = statement;
        Sequence = sequence;
    }

    /// <inheritdoc/>
    [JsonPropertyName("id")]
    public MeasureId Id { get; }

    /// <inheritdoc/>
    [JsonPropertyName("uri")]
    public Uri Uri { get; }

    /// <inheritdoc/>
    [JsonPropertyName("level")]
    public MeasurementLevel MeasurementLevel { get; }

    /// <inheritdoc/>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <inheritdoc/>
    [JsonPropertyName("statement")]
    public string Statement { get; }

    /// <inheritdoc/>
    [JsonPropertyName("sequence")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint Sequence { get; }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    /// <inheritdoc/>
    public bool Equals(Measure? other)
    {
        return other is not null && Id == other.Id;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Measure);
    }
}

/// <summary>
/// A measure that produces observations carrying a single value of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue">The type of value assigned by an observation.</typeparam>
public sealed class Measure<TValue> : Measure, IMeasure<TValue>
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    /// <summary>
    /// Initializes a new <see cref="Measure{TValue}"/> with an identifier derived from <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">A URI that uniquely identifies the measure.</param>
    /// <param name="measurementLevel">The measurement level of the values produced by this measure.</param>
    /// <param name="name">A short human-readable name for the measure.</param>
    /// <param name="statement">The statement assigned a value by an observation of this measure.</param>
    /// <param name="sequence">The order in which the measure should be displayed.</param>
    public Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement, uint sequence = default)
        : base(uri, measurementLevel, name, statement, sequence)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Measure{TValue}"/> with an explicit identifier. This constructor
    /// is intended for deserialization scenarios.
    /// </summary>
    /// <param name="id">The identifier for the measure.</param>
    /// <param name="uri">A URI that uniquely identifies the measure.</param>
    /// <param name="measurementLevel">The measurement level of the values produced by this measure.</param>
    /// <param name="name">A short human-readable name for the measure.</param>
    /// <param name="statement">The statement assigned a value by an observation of this measure.</param>
    /// <param name="sequence">The order in which the measure should be displayed.</param>
    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement, uint sequence)
        : base(id, uri, measurementLevel, name, statement, sequence)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Observation{TValue}"/> for this measure with the specified value
    /// and a timestamp obtained from the supplied <paramref name="timeProvider"/> (or
    /// <see cref="TimeProvider.System"/> if <see langword="null"/>).
    /// </summary>
    /// <param name="value">The observed value.</param>
    /// <param name="timeProvider">The time provider used to obtain the observation time.</param>
    /// <returns>A new observation.</returns>
    public Observation<TValue> CreateObservation(TValue value, TimeProvider? timeProvider = null)
    {
        timeProvider ??= TimeProvider.System;
        return Observation.Create(this, value, timeProvider);
    }

    IObservation<TValue> IMeasure<TValue>.CreateObservation(TValue value, TimeProvider timeProvider)
    {
        return CreateObservation(value, timeProvider);
    }
}

/// <summary>
/// A measure that produces observations carrying a value of type <typeparamref name="TValue"/>
/// qualified by a parameter of type <typeparamref name="TParam"/>.
/// </summary>
/// <typeparam name="TValue">The type of value assigned by an observation.</typeparam>
/// <typeparam name="TParam">The type of the parameter that qualifies the measure.</typeparam>
public sealed class Measure<TValue, TParam> : Measure, IMeasure<TValue, TParam>
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    /// <summary>
    /// Initializes a new <see cref="Measure{TValue, TParam}"/> with an identifier derived from <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">A URI that uniquely identifies the measure.</param>
    /// <param name="measurementLevel">The measurement level of the values produced by this measure.</param>
    /// <param name="name">A short human-readable name for the measure.</param>
    /// <param name="statement">The statement assigned a value by an observation of this measure.</param>
    /// <param name="sequence">The order in which the measure should be displayed.</param>
    public Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement, uint sequence = default)
        : base(uri, measurementLevel, name, statement, sequence)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Measure{TValue, TParam}"/> with an explicit identifier. This
    /// constructor is intended for deserialization scenarios.
    /// </summary>
    /// <param name="id">The identifier for the measure.</param>
    /// <param name="uri">A URI that uniquely identifies the measure.</param>
    /// <param name="measurementLevel">The measurement level of the values produced by this measure.</param>
    /// <param name="name">A short human-readable name for the measure.</param>
    /// <param name="statement">The statement assigned a value by an observation of this measure.</param>
    /// <param name="sequence">The order in which the measure should be displayed.</param>
    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement, uint sequence)
        : base(id, uri, measurementLevel, name, statement, sequence)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Observation{TValue, TParam}"/> for this measure with the specified
    /// parameter and value, and a timestamp obtained from the supplied <paramref name="timeProvider"/>
    /// (or <see cref="TimeProvider.System"/> if <see langword="null"/>).
    /// </summary>
    /// <param name="parameter">The parameter qualifying the observation.</param>
    /// <param name="value">The observed value.</param>
    /// <param name="timeProvider">The time provider used to obtain the observation time.</param>
    /// <returns>A new observation.</returns>
    public Observation<TValue, TParam> CreateObservation(
        TParam parameter,
        TValue value,
        TimeProvider? timeProvider = null)
    {
        timeProvider ??= TimeProvider.System;
        return Observation.Create(this, parameter, value, timeProvider);
    }

    IObservation<TValue, TParam> IMeasure<TValue, TParam>.CreateObservation(
        TParam parameter,
        TValue value,
        TimeProvider timeProvider)
    {
        return CreateObservation(parameter, value, timeProvider);
    }
}
