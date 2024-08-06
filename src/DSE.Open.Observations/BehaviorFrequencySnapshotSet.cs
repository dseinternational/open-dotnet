// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BehaviorFrequencySnapshotSet : SnapshotSet<BehaviorFrequencySnapshot, BehaviorFrequencyObservation, BehaviorFrequency>
{
    protected BehaviorFrequencySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BehaviorFrequencySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected BehaviorFrequencySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BehaviorFrequencySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BehaviorFrequencySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BehaviorFrequencySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BehaviorFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BehaviorFrequencySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BehaviorFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BehaviorFrequencySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BehaviorFrequencySnapshotSet(now, now, trackerReference, snapshots);
    }
}
