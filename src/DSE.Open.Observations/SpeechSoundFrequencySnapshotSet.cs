// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpeechSoundFrequencySnapshotSet
    : SnapshotSet<SpeechSoundFrequencySnapshot, SpeechSoundFrequencyObservation, BehaviorFrequency, SpeechSound>
{
    public static SpeechSoundFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundFrequencySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SpeechSoundFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundFrequencySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new SpeechSoundFrequencySnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
