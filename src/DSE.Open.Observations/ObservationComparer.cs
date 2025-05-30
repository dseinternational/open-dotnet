// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public abstract class ObservationComparer : Comparer<IObservation>
{
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

            return x.GetMeasurementHashCode().CompareTo(y.GetMeasurementHashCode());
        }
    }
}
