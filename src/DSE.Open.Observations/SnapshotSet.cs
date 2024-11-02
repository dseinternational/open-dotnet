// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(BinarySnapshotSet), typeDiscriminator: Schemas.BinarySnapshotSet)]
[JsonDerivedType(typeof(CountSnapshotSet), typeDiscriminator: Schemas.CountSnapshotSet)]
[JsonDerivedType(typeof(AmountSnapshotSet), typeDiscriminator: Schemas.AmountSnapshotSet)]
[JsonDerivedType(typeof(RatioSnapshotSet), typeDiscriminator: Schemas.RatioSnapshotSet)]
[JsonDerivedType(typeof(BinaryWordSnapshotSet), typeDiscriminator: Schemas.BinaryWordSnapshotSet)]
[JsonDerivedType(typeof(BinarySpeechSoundSnapshotSet), typeDiscriminator: Schemas.BinarySpeechSoundSnapshotSet)]
public abstract record SnapshotSet
{
    private readonly DateTimeOffset _created;
    private readonly DateTimeOffset _updated;

    [JsonInclude]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(-98000)]
    public Identifier Id { get; init; } = Identifier.New(36, "snp"u8);

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
    [JsonPropertyName("upd")]
    [JsonPropertyOrder(-97800)]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset Updated
    {
        get => _updated;
        init => _updated = DateTimeOffset.FromUnixTimeMilliseconds(value.ToUnixTimeMilliseconds());
    }

    [JsonInclude]
    [JsonPropertyName("trk")]
    [JsonPropertyOrder(-90000)]
    public required Identifier TrackerReference { get; init; }
}

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

public abstract record SnapshotSet<TSnapshot, TObs, TValue> : SnapshotSet
    where TSnapshot : Snapshot<TObs, TValue>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public required ReadOnlyValueCollection<TSnapshot> Snapshots { get; init; } = [];
}

public abstract record SnapshotSet<TSnapshot, TObs, TValue, TDisc> : SnapshotSet
    where TSnapshot : Snapshot<TObs, TValue, TDisc>
    where TObs : Observation<TValue, TDisc>
    where TValue : IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public required ReadOnlyValueCollection<TSnapshot> Snapshots { get; init; } = [];
}
