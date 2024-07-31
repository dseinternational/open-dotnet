// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class SnapshotEqualityComparer<TObs, TValue>
    : IEqualityComparer<Snapshot<TObs, TValue>>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    public static readonly IEqualityComparer<Snapshot<TObs, TValue>> Measurement =
        new DiscriminatedSnapshotEqualityComparer();

    public abstract bool Equals(Snapshot<TObs, TValue>? x, Snapshot<TObs, TValue>? y);

    public abstract int GetHashCode([DisallowNull] Snapshot<TObs, TValue> obj);

    private sealed class DiscriminatedSnapshotEqualityComparer : SnapshotEqualityComparer<TObs, TValue>
    {
        public override bool Equals(Snapshot<TObs, TValue>? x, Snapshot<TObs, TValue>? y)
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

        public override int GetHashCode([DisallowNull] Snapshot<TObs, TValue> obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}
