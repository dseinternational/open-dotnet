// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public abstract class ObservationEqualityComparer<T> : EqualityComparer<T>
    where T : IObservation
{
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static IEqualityComparer<T> Measurement { get; } = new MeasurementEqualityComparer();
#pragma warning restore CA1000 // Do not declare static members on generic types

    private class MeasurementEqualityComparer : ObservationEqualityComparer<T>
    {
        public override bool Equals(T? x, T? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.GetMeasurementHashCode() == y.GetMeasurementHashCode();
        }

        public override int GetHashCode(T obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}

public static class ObservationEqualityComparer
{
    public static IEqualityComparer<IObservation> Measurement => ObservationEqualityComparer<IObservation>.Measurement;
}
