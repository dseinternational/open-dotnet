// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinarySnapshotSet : SnapshotSet<BinarySnapshot, BinaryObservation, bool>
{
    protected BinarySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected BinarySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinarySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BinarySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinarySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BinarySnapshotSet(now, now, trackerReference, snapshots);
    }
}
