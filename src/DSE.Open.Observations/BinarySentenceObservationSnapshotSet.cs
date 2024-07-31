// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinarySentenceObservationSnapshotSet
    : ObservationSnapshotSet<BinarySentenceObservationSnapshot, BinarySentenceObservation, bool, SentenceId>
{
    protected BinarySentenceObservationSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySentenceObservationSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinarySentenceObservationSnapshotSet(
        ObservationSnapshotSetId id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySentenceObservationSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BinarySentenceObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySentenceObservationSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinarySentenceObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySentenceObservationSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BinarySentenceObservationSnapshotSet(now, now, trackerReference, snapshots);
    }
}
