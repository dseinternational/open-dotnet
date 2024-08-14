// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;
using MessagePack;

namespace DSE.Open.Observations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(BinarySnapshotSet), typeDiscriminator: Schemas.BinarySnapshotSet)]
[JsonDerivedType(typeof(CountSnapshotSet), typeDiscriminator: Schemas.CountSnapshotSet)]
[JsonDerivedType(typeof(AmountSnapshotSet), typeDiscriminator: Schemas.AmountSnapshotSet)]
[JsonDerivedType(typeof(RatioSnapshotSet), typeDiscriminator: Schemas.RatioSnapshotSet)]
[JsonDerivedType(typeof(BinaryWordSnapshotSet), typeDiscriminator: Schemas.BinaryWordSnapshotSet)]
[JsonDerivedType(typeof(BinarySpeechSoundSnapshotSet), typeDiscriminator: Schemas.BinarySpeechSoundSnapshotSet)]
[Union(-1, typeof(BinaryWordSnapshotSet))]
public abstract record SnapshotSet
{
    protected SnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference)
        : this(Identifier.New(36, "snp"u8), created, updated, trackerReference)
    {
    }

    protected SnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference)
    {
        Id = id;
        CreatedTimestamp = created.ToUnixTimeMilliseconds();
        UpdatedTimestamp = updated.ToUnixTimeMilliseconds();
        TrackerReference = trackerReference;
    }

    [Obsolete("For deserialization only", true)]
    [SerializationConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference)
    {
        Id = id;
        CreatedTimestamp = createdTimestamp;
        UpdatedTimestamp = updatedTimestamp;
        TrackerReference = trackerReference;
    }

    /// <summary>
    /// A randomly generated number between 0 and <see cref="NumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation set.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(-98000)]
    [Key(0)]
    public Identifier Id { get; }

    [JsonIgnore]
    [IgnoreMember]
    public DateTimeOffset Created => DateTimeOffset.FromUnixTimeMilliseconds(CreatedTimestamp);

    [JsonIgnore]
    [IgnoreMember]
    public DateTimeOffset Updated => DateTimeOffset.FromUnixTimeMilliseconds(UpdatedTimestamp);

    // this ensures equality tests are the same before/after serialization

    [JsonInclude]
    [JsonPropertyName("crt")]
    [JsonPropertyOrder(-97800)]
    [Key(1)]
    public long CreatedTimestamp { get; }

    [JsonInclude]
    [JsonPropertyName("upd")]
    [JsonPropertyOrder(-97800)]
    [Key(2)]
    public long UpdatedTimestamp { get; }

    [JsonInclude]
    [JsonPropertyName("trk")]
    [JsonPropertyOrder(-90000)]
    [Key(3)]
    public Identifier TrackerReference { get; }
}

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

public abstract record SnapshotSet<TSnapshot, TObs, TValue> : SnapshotSet
    where TSnapshot : Snapshot<TObs, TValue>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    protected SnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<TSnapshot> snapshots)
        : base(created, updated, trackerReference)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        Snapshots = snapshots;
    }

    protected SnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<TSnapshot> snapshots)
        : base(id, created, updated, trackerReference)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        Snapshots = snapshots;
    }

    [SerializationConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<TSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        Snapshots = snapshots;
    }

    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    public ReadOnlyValueCollection<TSnapshot> Snapshots { get; } = [];
}

[Union(0, typeof(BinaryWordSnapshotSet))]
public abstract record SnapshotSet<TSnapshot, TObs, TValue, TDisc> : SnapshotSet
    where TSnapshot : Snapshot<TObs, TValue, TDisc>
    where TObs : Observation<TValue, TDisc>
    where TValue : IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    protected SnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<TSnapshot> snapshots)
        : base(created, updated, trackerReference)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        Snapshots = snapshots;
    }

    protected SnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<TSnapshot> snapshots)
        : base(id, created, updated, trackerReference)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        Snapshots = snapshots;
    }

    [Obsolete("For deserialization only", true)]
    [SerializationConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<TSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference)
    {
        ArgumentNullException.ThrowIfNull(snapshots);
        Snapshots = snapshots;
    }

    [JsonPropertyName("obs")]
    [JsonPropertyOrder(900000)]
    [Key(4)]
    public ReadOnlyValueCollection<TSnapshot> Snapshots { get; } = [];
}
