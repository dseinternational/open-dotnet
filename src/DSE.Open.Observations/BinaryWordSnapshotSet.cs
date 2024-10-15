// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinaryWordSnapshotSet
    : SnapshotSet<BinaryWordSnapshot, BinaryWordObservation, bool, WordId>
{
    protected BinaryWordSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected BinaryWordSnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordSnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinaryWordSnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BinaryWordSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinaryWordSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BinaryWordSnapshotSet(now, now, trackerReference, snapshots);
    }
}
