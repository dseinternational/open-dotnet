// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class ObservationEqualityComparer<TObs, TValue> : IEqualityComparer<TObs>
    where TObs : Observation<TValue>
    where TValue : struct, IEquatable<TValue>
{
    public static readonly IEqualityComparer<TObs> Default = EqualityComparer<TObs>.Default;

    /// <summary>
    /// Gets an equality comparer that compares observations by <see cref="Observation{TObs, TValue}.GetMeasurementHashCode"/>
    /// </summary>
    public static readonly IEqualityComparer<TObs> Measurement = new DiscriminatorObservationEqualityComparer();

    public abstract bool Equals(TObs? x, TObs? y);

    public abstract int GetHashCode([DisallowNull] TObs obj);

    private sealed class DiscriminatorObservationEqualityComparer : ObservationEqualityComparer<TObs, TValue>
    {
        public override bool Equals(TObs? x, TObs? y)
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

        public override int GetHashCode([DisallowNull] TObs obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}
