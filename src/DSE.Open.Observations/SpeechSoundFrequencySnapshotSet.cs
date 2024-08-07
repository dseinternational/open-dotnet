// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpeechSoundFrequencySnapshotSet
    : SnapshotSet<SpeechSoundFrequencySnapshot, SpeechSoundFrequencyObservation, BehaviorFrequency, SpeechSound>
{
    protected SpeechSoundFrequencySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundFrequencySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected SpeechSoundFrequencySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundFrequencySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SpeechSoundFrequencySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundFrequencySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

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
        return new SpeechSoundFrequencySnapshotSet(now, now, trackerReference, snapshots);
    }
}
