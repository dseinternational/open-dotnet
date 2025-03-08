// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class ObservationValueExtensions
{
    public static double ConvertToDouble(this IObservationValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        switch (value.ValueType)
        {
            case MeasurementValueType.Binary:
                return value.GetBinary() ? 1 : 0;
            case MeasurementValueType.Ordinal:
                return value.GetOrdinal();
            case MeasurementValueType.Count:
                return value.GetCount();
            case MeasurementValueType.Amount:
                return (double)value.GetAmount();
            case MeasurementValueType.Ratio:
                return (double)value.GetRatio();
            case MeasurementValueType.Frequency:
                return (double)value.GetFrequency();
            default:
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
                return double.MinValue; // unreachable
        }
    }

    public static decimal ConvertToDecimal(this IObservationValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        switch (value.ValueType)
        {
            case MeasurementValueType.Binary:
                return value.GetBinary() ? 1 : 0;
            case MeasurementValueType.Ordinal:
                return value.GetOrdinal();
            case MeasurementValueType.Count:
                return value.GetCount();
            case MeasurementValueType.Amount:
                return value.GetAmount();
            case MeasurementValueType.Ratio:
                return value.GetRatio();
            case MeasurementValueType.Frequency:
                return value.GetFrequency();
            default:
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
                return decimal.MinValue; // unreachable
        }
    }
}
