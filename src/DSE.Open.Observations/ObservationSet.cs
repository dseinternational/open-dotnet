// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

/// <summary>
/// A collection of observations linked to a tracker (and, therefore, a single subject).
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(BinaryObservationSet), typeDiscriminator: Schemas.BinaryObservationSet)]
[JsonDerivedType(typeof(CountObservationSet), typeDiscriminator: Schemas.CountObservationSet)]
[JsonDerivedType(typeof(AmountObservationSet), typeDiscriminator: Schemas.AmountObservationSet)]
[JsonDerivedType(typeof(RatioObservationSet), typeDiscriminator: Schemas.RatioObservationSet)]
[JsonDerivedType(typeof(BinaryWordObservationSet), typeDiscriminator: Schemas.BinaryWordObservationSet)]
[JsonDerivedType(typeof(BinarySpeechSoundObservationSet), typeDiscriminator: Schemas.BinarySpeechSoundObservationSet)]
public abstract record ObservationSet
{
    protected ObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location)
    {
        ArgumentNullException.ThrowIfNull(source);

        Id = ObservationSetId.GetRandomId();
        CreatedTimestamp = created.ToUnixTimeMilliseconds();
        TrackerReference = trackerReference;
        ObserverReference = observerReference;
        Source = source;
        Location = location;
    }

    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected ObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location)
    {
        ArgumentNullException.ThrowIfNull(source);

        Id = id;
        CreatedTimestamp = createdTimestamp;
        TrackerReference = trackerReference;
        ObserverReference = observerReference;
        Source = source;
        Location = location;
    }

    /// <summary>
    /// A randomly generated number between 0 and <see cref="NumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation set.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(-98000)]
    public ObservationSetId Id { get; }

    /// <summary>
    /// The time the observation set was created.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset Created => DateTimeOffset.FromUnixTimeMilliseconds(CreatedTimestamp);

    // this ensures equality tests are the same before/after serialization

    [JsonInclude]
    [JsonPropertyName("crt")]
    [JsonPropertyOrder(-97800)]
    protected long CreatedTimestamp { get; }

    [JsonInclude]
    [JsonPropertyName("trk")]
    [JsonPropertyOrder(-90000)]
    public Identifier TrackerReference { get; }

    [JsonInclude]
    [JsonPropertyName("obr")]
    [JsonPropertyOrder(-89000)]
    public Identifier ObserverReference { get; }

    [JsonInclude]
    [JsonPropertyOrder(-60000)]
    [JsonPropertyName("src")]
    public Uri Source { get; } = default!;

    [JsonInclude]
    [JsonPropertyName("loc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(-50000)]
    public GroundPoint? Location { get; }
}

/// <summary>
/// A collection of observations of a particular type linked to a tracker (and, therefore, a single subject).
/// </summary>
public abstract record ObservationSet<TObs, TValue> : ObservationSet
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    protected ObservationSet(
        DateTimeOffset created,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<TObs> observations)
        : base(created, trackerReference, observerReference, source, location)
    {
        ArgumentNullException.ThrowIfNull(observations);
        Observations = observations;
    }

    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected ObservationSet(
        ObservationSetId id,
        long createdTimestamp,
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        ReadOnlyValueCollection<TObs> observations)
        : base(id, createdTimestamp, trackerReference, observerReference, source, location)
    {
        ArgumentNullException.ThrowIfNull(observations);
        Observations = observations;
    }

    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public ReadOnlyValueCollection<TObs> Observations { get; } = [];
}
