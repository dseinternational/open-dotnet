// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public abstract class ObservationEqualityComparer : EqualityComparer<IObservation>
{
    public static ObservationEqualityComparer Measurement { get; } = new MeasurementEqualityComparer();

    private class MeasurementEqualityComparer : ObservationEqualityComparer
    {
        public override bool Equals(IObservation? x, IObservation? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.GetMeasurementHashCode() == y.GetMeasurementHashCode();
        }

        public override int GetHashCode(IObservation obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}
