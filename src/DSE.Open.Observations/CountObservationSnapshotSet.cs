// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record CountObservationSnapshotSet : ObservationSnapshotSet<CountObservationSnapshot, CountObservation, Count>
{
    protected CountObservationSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<CountObservationSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected CountObservationSnapshotSet(
        ObservationSnapshotSetId id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<CountObservationSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static CountObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<CountObservationSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static CountObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<CountObservationSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new CountObservationSnapshotSet(now, now, trackerReference, snapshots);
    }
}
