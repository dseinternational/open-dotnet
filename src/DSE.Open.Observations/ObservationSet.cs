// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Text.Json.Serialization;
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
[JsonDerivedType(typeof(BinarySentenceObservationSet), typeDiscriminator: Schemas.BinarySentenceObservationSet)]
[JsonDerivedType(typeof(BinarySpeechSoundObservationSet), typeDiscriminator: Schemas.BinarySpeechSoundObservationSet)]
public abstract record ObservationSet
{
    private readonly DateTimeOffset _created;

    /// <summary>
    /// A randomly generated number between 0 and <see cref="NumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation set.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(-98000)]
    public ObservationSetId Id { get; init; } = ObservationSetId.GetRandomId();

    [JsonInclude]
    [JsonPropertyName("crt")]
    [JsonPropertyOrder(-97800)]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset Created
    {
        get => _created;
        init => _created = DateTimeOffset.FromUnixTimeMilliseconds(value.ToUnixTimeMilliseconds());
    }

    [JsonInclude]
    [JsonPropertyName("trk")]
    [JsonPropertyOrder(-90000)]
    public required Identifier TrackerReference { get; init; }

    [JsonInclude]
    [JsonPropertyName("obr")]
    [JsonPropertyOrder(-89000)]
    public required Identifier ObserverReference { get; init; }

    [JsonInclude]
    [JsonPropertyOrder(-60000)]
    [JsonPropertyName("src")]
    public required Uri Source { get; set; }

    [JsonInclude]
    [JsonPropertyName("loc")]
    [JsonPropertyOrder(-50000)]
    public required GroundPoint? Location { get; init; }
}

/// <summary>
/// A collection of observations of a particular type linked to a tracker (and, therefore, a single subject).
/// </summary>
public abstract record ObservationSet<TObs, TValue> : ObservationSet
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public required ReadOnlyValueCollection<TObs> Observations { get; init; } = [];
}
