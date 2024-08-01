// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinarySpeechSoundSnapshotSet
    : SnapshotSet<BinarySpeechSoundSnapshot, BinarySpeechSoundObservation, bool, SpeechSound>
{
    protected BinarySpeechSoundSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected BinarySpeechSoundSnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundSnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinarySpeechSoundSnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

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
        return new BinarySpeechSoundSnapshotSet(now, now, trackerReference, snapshots);
    }
}
