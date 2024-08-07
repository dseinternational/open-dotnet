// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record WordFrequencySnapshotSet
    : SnapshotSet<WordFrequencySnapshot, WordFrequencyObservation, BehaviorFrequency, WordId>
{
    protected WordFrequencySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<WordFrequencySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected WordFrequencySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<WordFrequencySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected WordFrequencySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<WordFrequencySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static WordFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<WordFrequencySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static WordFrequencySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<WordFrequencySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new WordFrequencySnapshotSet(now, now, trackerReference, snapshots);
    }
}
