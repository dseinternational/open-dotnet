// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class ObservationComparer<TObs> : IEqualityComparer<TObs>
    where TObs : IObservation
{

#pragma warning disable CA1000 // Do not declare static members on generic types

    public static IEqualityComparer<TObs> Default { get; } = new DefaultObservationComparer();

    public static IEqualityComparer<TObs> Measurement { get; } = new ObservationMeasurementComparer();

#pragma warning restore CA1000 // Do not declare static members on generic types

    public abstract bool Equals(TObs? x, TObs? y);

    public abstract int GetHashCode([DisallowNull] TObs obj);

    private sealed class DefaultObservationComparer : ObservationComparer<TObs>
    {
        public override bool Equals(TObs? x, TObs? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public override int GetHashCode(TObs obj)
        {
            return obj.GetHashCode();
        }
    }

    private sealed class ObservationMeasurementComparer : ObservationComparer<TObs>
    {
        public override bool Equals(TObs? x, TObs? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.GetMeasurementHashCode() == y.GetMeasurementHashCode();
        }

        public override int GetHashCode(TObs obj)
        {
            return obj.GetMeasurementHashCode().GetHashCode();
        }
    }
}
