// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Provides <see cref="IEqualityComparer{T}"/> implementations for observation types.
/// </summary>
/// <typeparam name="T">The observation type being compared.</typeparam>
public abstract class ObservationEqualityComparer<T> : EqualityComparer<T>
    where T : IObservation
{
#pragma warning disable CA1000 // Do not declare static members on generic types
    /// <summary>
    /// An equality comparer that treats two observations as equal when they share the same
    /// <see cref="IObservation.MeasureId"/> and parameters (i.e. they describe the same measurement).
    /// </summary>
    public static IEqualityComparer<T> Measurement { get; } = new MeasurementEqualityComparer();
#pragma warning restore CA1000 // Do not declare static members on generic types

    private class MeasurementEqualityComparer : ObservationEqualityComparer<T>
    {
        public override bool Equals(T? x, T? y)
        {
            // IEqualityComparer<T>.Equals contract: both-null is equal; either-null
            // with the other non-null is not equal.
            if (x is null)
            {
                return y is null;
            }

            if (y is null)
            {
                return false;
            }

            return x.MeasureId == y.MeasureId
                && Equals(x.Parameter, y.Parameter)
                && Equals(x.Parameter2, y.Parameter2);
        }

        public override int GetHashCode(T obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}

/// <summary>
/// Provides non-generic access to the equality comparers exposed by <see cref="ObservationEqualityComparer{T}"/>.
/// </summary>
public static class ObservationEqualityComparer
{
    /// <summary>
    /// An equality comparer that treats two <see cref="IObservation"/> instances as equal when they
    /// describe the same measurement (same <see cref="IObservation.MeasureId"/> and parameters).
    /// </summary>
    public static IEqualityComparer<IObservation> Measurement => ObservationEqualityComparer<IObservation>.Measurement;
}
