// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinaryWordObservationSnapshotSet
    : ObservationSnapshotSet<BinaryWordObservationSnapshot, BinaryWordObservation, bool, WordId>
{
    protected BinaryWordObservationSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordObservationSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinaryWordObservationSnapshotSet(
        ObservationSnapshotSetId id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordObservationSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BinaryWordObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordObservationSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinaryWordObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinaryWordObservationSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BinaryWordObservationSnapshotSet(now, now, trackerReference, snapshots);
    }
}
