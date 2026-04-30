// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Provides <see cref="IComparer{T}"/> implementations for <see cref="IObservation"/>.
/// </summary>
public abstract class ObservationComparer : Comparer<IObservation>
{
    /// <summary>
    /// A comparer that orders observations by measurement: first by <see cref="IObservation.MeasureId"/>,
    /// then by <see cref="IObservation.Parameter"/>, then by <see cref="IObservation.Parameter2"/>.
    /// </summary>
    public static ObservationComparer Measurement { get; } = new MeasurementComparer();

    private class MeasurementComparer : ObservationComparer
    {
        public override int Compare(IObservation? x, IObservation? y)
        {
            if (x is null)
            {
                return y is null ? 0 : -1;
            }

            if (y is null)
            {
                return 1;
            }

            var c = x.MeasureId.ToUInt64().CompareTo(y.MeasureId.ToUInt64());
            if (c != 0)
            {
                return c;
            }

            c = CompareParameter(x.Parameter, y.Parameter);
            if (c != 0)
            {
                return c;
            }

            return CompareParameter(x.Parameter2, y.Parameter2);
        }

        private static int CompareParameter(object? x, object? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            if (Equals(x, y))
            {
                return 0;
            }

            if (x.GetType() == y.GetType() && x is IComparable xc)
            {
                return xc.CompareTo(y);
            }

            return string.CompareOrdinal(x.ToString(), y.ToString());
        }
    }
}
