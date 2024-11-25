// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public abstract class SnapshotEqualityComparer : EqualityComparer<ISnapshot>
{
    public static SnapshotEqualityComparer Measurement { get; } = new MeasurementEqualityComparer();

    private class MeasurementEqualityComparer : SnapshotEqualityComparer
    {
        public override bool Equals(ISnapshot? x, ISnapshot? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.GetMeasurementHashCode() == y.GetMeasurementHashCode();
        }

        public override int GetHashCode(ISnapshot obj)
        {
            return obj.GetMeasurementHashCode();
        }
    }
}
