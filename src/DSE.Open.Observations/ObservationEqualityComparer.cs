// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public abstract class ObservationEqualityComparer<T> : IEqualityComparer<T>
    where T : Observation
{
    public static readonly IEqualityComparer<T> Default = EqualityComparer<T>.Default;

    /// <summary>
    /// Gets an equality comparer that compares observations by <see cref="Observation.GetDiscriminatorCode"/>
    /// </summary>
    public static readonly IEqualityComparer<T> Discriminator = new DiscriminatorObservationEqualityComparer();

    public abstract bool Equals(T? x, T? y);

    public abstract int GetHashCode([DisallowNull] T obj);

    private sealed class DiscriminatorObservationEqualityComparer : ObservationEqualityComparer<T>
    {
        public override bool Equals(T? x, T? y)
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

        public override int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetDiscriminatorCode().GetHashCode();
        }
    }
}
