// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpeechSoundClaritySnapshotSet
    : SnapshotSet<SpeechSoundClaritySnapshot, SpeechSoundClarityObservation, SpeechClarity, SpeechSound>
{
    protected SpeechSoundClaritySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundClaritySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected SpeechSoundClaritySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundClaritySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SpeechSoundClaritySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpeechSoundClaritySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

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
        return new SpeechSoundClaritySnapshotSet(now, now, trackerReference, snapshots);
    }
}
