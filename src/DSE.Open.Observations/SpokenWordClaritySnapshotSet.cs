// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpokenWordClaritySnapshotSet
    : SnapshotSet<SpokenWordClaritySnapshot, SpokenWordClarityObservation, SpeechClarity, WordId>
{
    public static SpokenWordClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SpokenWordClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();

        return new SpokenWordClaritySnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
