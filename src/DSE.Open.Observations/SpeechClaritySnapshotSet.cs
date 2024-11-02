// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpeechClaritySnapshotSet : SnapshotSet<SpeechClaritySnapshot, SpeechClarityObservation, SpeechClarity>
{
    public static SpeechClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechClaritySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SpeechClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechClaritySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();

        return new SpeechClaritySnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
