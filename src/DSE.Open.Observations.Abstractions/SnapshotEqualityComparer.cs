// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class SnapshotEqualityComparer<TObs, TValue>
    : IEqualityComparer<Snapshot<TObs>>
    where TObs : Observation<TValue>
    where TValue : struct, IEquatable<TValue>
{
    public static readonly IEqualityComparer<Snapshot<TObs>> Measurement =
        new DiscriminatedSnapshotEqualityComparer();

    public abstract bool Equals(Snapshot<TObs>? x, Snapshot<TObs>? y);

    public abstract int GetHashCode([DisallowNull] Snapshot<TObs> obj);

    private sealed class DiscriminatedSnapshotEqualityComparer : SnapshotEqualityComparer<TObs, TValue>
    {
        public override bool Equals(Snapshot<TObs>? x, Snapshot<TObs>? y)
        {
            if (x is null)
            {
                return y is null;
            }

            if (y is null)
            {
                return false;
            }

            return x.GetMeasurementHashCode().Equals(y.GetMeasurementHashCode());
        }

        public override int GetHashCode([DisallowNull] Snapshot<TObs> obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}
