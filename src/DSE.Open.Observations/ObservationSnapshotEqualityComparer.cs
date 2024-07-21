// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class ObservationSnapshotEqualityComparer<TObs, TValue>
    : IEqualityComparer<ObservationSnapshot<TObs, TValue>>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    public static readonly IEqualityComparer<ObservationSnapshot<TObs, TValue>> Measurement =
        new DiscriminatedObservationSnapshotEqualityComparer();

    public abstract bool Equals(ObservationSnapshot<TObs, TValue>? x, ObservationSnapshot<TObs, TValue>? y);

    public abstract int GetHashCode([DisallowNull] ObservationSnapshot<TObs, TValue> obj);

    private sealed class DiscriminatedObservationSnapshotEqualityComparer : ObservationSnapshotEqualityComparer<TObs, TValue>
    {
        public override bool Equals(ObservationSnapshot<TObs, TValue>? x, ObservationSnapshot<TObs, TValue>? y)
        {
            if (x is null)
            {
                return y is null;
            }

            if (y is null)
            {
                return false;
            }

            return x.GetMeasurementCode().Equals(y.GetMeasurementCode());
        }

        public override int GetHashCode([DisallowNull] ObservationSnapshot<TObs, TValue> obj)
        {
            return obj.GetMeasurementCode();
        }
    }
}
