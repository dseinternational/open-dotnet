// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record RatioObservationSnapshotSet : ObservationSnapshotSet<RatioObservationSnapshot, RatioObservation, Ratio>
{
    protected RatioObservationSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<RatioObservationSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected RatioObservationSnapshotSet(
        ObservationSnapshotSetId id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<RatioObservationSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static RatioObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<RatioObservationSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static RatioObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<RatioObservationSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new RatioObservationSnapshotSet(now, now, trackerReference, snapshots);
    }
}
