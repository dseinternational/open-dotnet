// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SentenceCompletenessSnapshotSet
    : SnapshotSet<SentenceCompletenessSnapshot, SentenceCompletenessObservation, Completeness, SentenceId>
{
    public static SentenceCompletenessSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SentenceCompletenessSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new SentenceCompletenessSnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
