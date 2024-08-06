// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record SentenceCompletenessSnapshotSet
    : SnapshotSet<SentenceCompletenessSnapshot, SentenceCompletenessObservation, Completeness, SentenceId>
{
    protected SentenceCompletenessSnapshotSet(
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots)
        : base(created, updated, trackerReference, snapshots)
    {
    }

    protected SentenceCompletenessSnapshotSet(
        Identifier id,
        DateTimeOffset created,
        DateTimeOffset updated,
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots)
        : base(id, created, updated, trackerReference, snapshots)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SentenceCompletenessSnapshotSet(
        Identifier id,
        long createdTimestamp,
        long updatedTimestamp,
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots)
        : base(id, createdTimestamp, updatedTimestamp, trackerReference, snapshots)
    {
    }

    public static SentenceCompletenessSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots)
    {
        return Create(trackerReference, snapshots, TimeProvider.System);
    }

    public static SentenceCompletenessSnapshotSet Create(
        Identifier trackerReference,
        ReadOnlyValueCollection<SentenceCompletenessSnapshot> snapshots,
        TimeProvider timeProvider)
    {
        Guard.IsNotDefault(trackerReference);
        ArgumentNullException.ThrowIfNull(snapshots);
        ArgumentNullException.ThrowIfNull(timeProvider);

        var now = timeProvider.GetUtcNow();
        return new SentenceCompletenessSnapshotSet(now, now, trackerReference, snapshots);
    }
}
