// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// A set of observation snapshots, compared using a <see cref="ObservationSnapshotEqualityComparer{TObs, TValue}.Measurement"/>
/// equality comparer. This implies that only one snapshot for each measurement is allowed in the set.
/// </summary>
/// <typeparam name="TSnapshot"></typeparam>
/// <typeparam name="TObs"></typeparam>
/// <typeparam name="TValue"></typeparam>
#pragma warning disable CA1005 // Avoid excessive parameters on generic types
public class MeasurementSnapshotSet<TSnapshot, TObs, TValue> : HashSet<TSnapshot>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
    where TSnapshot : ObservationSnapshot<TObs, TValue>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    public MeasurementSnapshotSet()
        : base(ObservationSnapshotEqualityComparer<TObs, TValue>.Measurement)
    {
    }

    public MeasurementSnapshotSet(IEnumerable<TSnapshot> collection)
        : base(collection, ObservationSnapshotEqualityComparer<TObs, TValue>.Measurement)
    {
    }
}
