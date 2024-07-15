// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
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
    [JsonPropertyName("trk")]
    [JsonPropertyOrder(-90000)]
    public required Identifier TrackerReference { get; init; }

    [JsonPropertyName("obr")]
    [JsonPropertyOrder(-89000)]
    public required Identifier ObserverReference { get; init; }

    [JsonPropertyOrder(-60000)]
    [JsonPropertyName("src")]
    public required Uri Source { get; init; }

    [JsonPropertyName("loc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(-50000)]
    public GroundPoint? Location { get; init; }
}

public record ObservationSet<T> : ObservationSet, IObservationSet<T>
    where T : IObservation
{
    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public ReadOnlyValueCollection<T> Observations { get; init; } = [];

    IReadOnlyCollection<T> IObservationSet<T>.Observations => Observations;
}
