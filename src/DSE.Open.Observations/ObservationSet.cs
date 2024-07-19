// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(ObservationSet<Observation<bool>>), typeDiscriminator: Schemas.BinaryObservationSet)]
[JsonDerivedType(typeof(ObservationSet<Observation<Count>>), typeDiscriminator: Schemas.CountObservationSet)]
[JsonDerivedType(typeof(ObservationSet<Observation<Amount>>), typeDiscriminator: Schemas.AmountObservationSet)]
[JsonDerivedType(typeof(ObservationSet<Observation<Ratio>>), typeDiscriminator: Schemas.RatioObservationSet)]
[JsonDerivedType(typeof(ObservationSet<BinaryWordObservation>), typeDiscriminator: Schemas.BinaryWordObservationSet)]
[JsonDerivedType(typeof(ObservationSet<BinarySpeechSoundObservation>), typeDiscriminator: Schemas.BinarySpeechSoundObservationSet)]
[JsonDerivedType(typeof(ObservationSet<Observation<int>>), typeDiscriminator: Schemas.IntegerObservationSet)]
[JsonDerivedType(typeof(ObservationSet<Observation<decimal>>), typeDiscriminator: Schemas.DecimalObservationSet)]
public abstract record ObservationSet : IObservationSet
{
    /// <summary>
    /// A randomly generated number between 0 and <see cref="RandomNumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation set.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(-98000)]
    public ulong Id { get; private init; }

    /// <summary>
    /// The time the observation set was created.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("crt")]
    [JsonPropertyOrder(-97800)]
    [JsonConverter(typeof(DateTimeOffsetUnixTimeMillisecondsJsonConverter))]
    public DateTimeOffset Created { get; private init; }

    [JsonInclude]
    [JsonPropertyName("trk")]
    [JsonPropertyOrder(-90000)]
    public Identifier TrackerReference { get; private init; }

    [JsonInclude]
    [JsonPropertyName("obr")]
    [JsonPropertyOrder(-89000)]
    public Identifier ObserverReference { get; private init; }

    [JsonInclude]
    [JsonPropertyOrder(-60000)]
    [JsonPropertyName("src")]
    public Uri Source { get; private init; } = default!;

    [JsonInclude]
    [JsonPropertyName("loc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(-50000)]
    public GroundPoint? Location { get; private init; }

    public static ObservationSet<T> Create<T>(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        IEnumerable<T> observations)
        where T : IObservation
    {
        return Create(trackerReference, observerReference, source, null, observations);
    }

    public static ObservationSet<T> Create<T>(
        Identifier trackerReference,
        Identifier observerReference,
        Uri source,
        GroundPoint? location,
        IEnumerable<T> observations)
        where T : IObservation
    {
        Guard.IsNotDefault(trackerReference);
        Guard.IsNotDefault(observerReference);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(observations);

        return new ObservationSet<T>
        {
            Id = RandomNumberHelper.GetJsonSafeInteger(),
            Created = DateTimeOffset.UtcNow,
            TrackerReference = trackerReference,
            ObserverReference = observerReference,
            Source = source,
            Location = location,
            Observations = observations.ToReadOnlyValueCollection()
        };
    }
}

public record ObservationSet<T> : ObservationSet, IObservationSet<T>
    where T : IObservation
{
    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public ReadOnlyValueCollection<T> Observations { get; init; } = [];

    IReadOnlyCollection<T> IObservationSet<T>.Observations => Observations;
}
