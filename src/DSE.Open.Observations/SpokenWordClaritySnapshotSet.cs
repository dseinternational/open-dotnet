// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SpokenWordClaritySnapshotSet
    : SnapshotSet<SpokenWordClaritySnapshot, SpokenWordClarityObservation, SpeechClarity, WordId>
{
    protected SpokenWordClaritySnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected SpokenWordClaritySnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SpokenWordClaritySnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static SpokenWordClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SpokenWordClaritySnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SpokenWordClaritySnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new SpokenWordClaritySnapshotSet(now, now, trackerReference, snapshots);
    }
}
