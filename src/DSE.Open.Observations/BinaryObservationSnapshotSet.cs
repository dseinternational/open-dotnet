// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinaryObservationSnapshotSet : ObservationSnapshotSet<BinaryObservationSnapshot, BinaryObservation, bool>
{
    protected BinaryObservationSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryObservationSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinaryObservationSnapshotSet(
        ObservationSnapshotSetId id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryObservationSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BinaryObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryObservationSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinaryObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryObservationSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BinaryObservationSnapshotSet(now, now, trackerReference, snapshots);
    }
}
