// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record BinarySpeechSoundObservationSnapshotSet
    : ObservationSnapshotSet<BinarySpeechSoundObservationSnapshot, BinarySpeechSoundObservation, bool, SpeechSound>
{
    protected BinarySpeechSoundObservationSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundObservationSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinarySpeechSoundObservationSnapshotSet(
        ObservationSnapshotSetId id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundObservationSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static BinarySpeechSoundObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundObservationSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static BinarySpeechSoundObservationSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<BinarySpeechSoundObservationSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new BinarySpeechSoundObservationSnapshotSet(now, now, trackerReference, snapshots);
    }
}
