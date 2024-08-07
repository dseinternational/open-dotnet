// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SentenceFrequencySnapshotSet
    : SnapshotSet<SentenceFrequencySnapshot, SentenceFrequencyObservation, BehaviorFrequency, SentenceId>
{
    protected SentenceFrequencySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceFrequencySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected SentenceFrequencySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceFrequencySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SentenceFrequencySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceFrequencySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static SentenceFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceFrequencySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SentenceFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceFrequencySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new SentenceFrequencySnapshotSet(now, now, trackerReference, snapshots);
    }
}
