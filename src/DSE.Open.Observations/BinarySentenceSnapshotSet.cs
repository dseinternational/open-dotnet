// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinarySentenceSnapshotSet
    : SnapshotSet<BinarySentenceSnapshot, BinarySentenceObservation, bool, SentenceId>
{
    public static BinarySentenceSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySentenceSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinarySentenceSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySentenceSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();

        return new BinarySentenceSnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
