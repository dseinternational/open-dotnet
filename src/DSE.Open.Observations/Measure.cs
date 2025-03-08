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
public abstract class Measure : IMeasure
{
    protected Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : this(MeasureId.FromUri(uri), uri, measurementLevel, name, statement)
    {
    }

    protected Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(statement);

        Id = id;
        Uri = uri;
        MeasurementLevel = measurementLevel;
        Name = name;
        Statement = statement;
    }

    /// <inheritdoc/>
    [JsonPropertyName("id")]
    public MeasureId Id { get; }

    /// <inheritdoc/>
    [JsonPropertyName("uri")]
    public Uri Uri { get; }

    [JsonPropertyName("level")]
    public MeasurementLevel MeasurementLevel { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("statement")]
    public string Statement { get; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

public sealed class Measure<TValue> : Measure, IMeasure<TValue>
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    public Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(uri, measurementLevel, name, statement)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

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

public sealed class Measure<TValue, TParam> : Measure, IMeasure<TValue, TParam>
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    public Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(uri, measurementLevel, name, statement)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

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
