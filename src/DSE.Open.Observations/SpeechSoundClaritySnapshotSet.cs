// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpeechSoundClaritySnapshotSet
    : SnapshotSet<SpeechSoundClaritySnapshot, SpeechSoundClarityObservation, SpeechClarity, SpeechSound>
{
    public static SpeechSoundClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundClaritySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SpeechSoundClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundClaritySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();

        return new SpeechSoundClaritySnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
