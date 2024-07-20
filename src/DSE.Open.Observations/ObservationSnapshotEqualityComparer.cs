// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class ObservationSnapshotEqualityComparer<TObs> : IEqualityComparer<ObservationSnapshot<TObs>>
    where TObs : IObservation
{
    public static readonly IEqualityComparer<ObservationSnapshot<TObs>> Discriminated =
        new DiscriminatedObservationSnapshotEqualityComparer();

    public abstract bool Equals(ObservationSnapshot<TObs>? x, ObservationSnapshot<TObs>? y);

    public abstract int GetHashCode([DisallowNull] ObservationSnapshot<TObs> obj);

    private sealed class DiscriminatedObservationSnapshotEqualityComparer : ObservationSnapshotEqualityComparer<TObs>
    {
        public override bool Equals(ObservationSnapshot<TObs>? x, ObservationSnapshot<TObs>? y)
        {
            if (x is null)
            {
                return y is null;
            }

            if (y is null)
            {
                return false;
            }

            return x.GetDiscriminatorCode().Equals(y.GetDiscriminatorCode());
        }

        public override int GetHashCode([DisallowNull] ObservationSnapshot<TObs> obj)
        {
            return obj.GetDiscriminatorCode();
        }
    }
}
