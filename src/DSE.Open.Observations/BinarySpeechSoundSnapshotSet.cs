// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinarySpeechSoundSnapshotSet
    : SnapshotSet<BinarySpeechSoundSnapshot, BinarySpeechSoundObservation, bool, SpeechSound>
{
    public static BinarySpeechSoundSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinarySpeechSoundSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();

        return new BinarySpeechSoundSnapshotSet
        {
            Created = now,
            Updated = now,
            TrackerReference = trackerReference,
            Snapshots = snapshots
        };
    }
}
