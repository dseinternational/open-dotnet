// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public abstract class SnapshotComparer : Comparer<ISnapshot>
{
    public static SnapshotComparer Measurement { get; } = new MeasurementComparer();

    private class MeasurementComparer : SnapshotComparer
    {
        public override int Compare(ISnapshot? x, ISnapshot? y)
        {
            if (x is null)
            {
                return y is null ? 0 : -1;
            }

            if (y is null)
            {
                return 1;
            }

            return x.Observation.GetMeasurementHashCode().CompareTo(y.Observation.GetMeasurementHashCode());
        }
    }
}
