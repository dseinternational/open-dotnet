// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// A set of observation snapshots where equality is compared using
/// <see cref="ObservationSnapshotEqualityComparer{TObs}.Discriminated"/> (which compares snapshots using
/// <see cref="ObservationSnapshot{TObs}.GetDiscriminatorCode"/>).
/// </summary>
/// <typeparam name="TObs">The type of observation contained in the snapshot.</typeparam>
public class ObservationSnapshotSet<TObs> : HashSet<ObservationSnapshot<TObs>>
    where TObs : Observation
{
    public ObservationSnapshotSet()
        : base(ObservationSnapshotEqualityComparer<TObs>.Discriminated)
    {
    }

    public ObservationSnapshotSet(IEnumerable<ObservationSnapshot<TObs>> collection)
        : base(collection, ObservationSnapshotEqualityComparer<TObs>.Discriminated)
    {
    }
}
