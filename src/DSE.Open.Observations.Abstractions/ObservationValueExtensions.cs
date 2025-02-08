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
            case ObservationValueType.Binary:
                return value.GetBinary() ? 1 : 0;
            case ObservationValueType.Ordinal:
                return value.GetOrdinal();
            case ObservationValueType.Count:
                return value.GetCount();
            case ObservationValueType.Amount:
                return (double)value.GetAmount();
            case ObservationValueType.Ratio:
                return (double)value.GetRatio();
            case ObservationValueType.Frequency:
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
            case ObservationValueType.Binary:
                return value.GetBinary() ? 1 : 0;
            case ObservationValueType.Ordinal:
                return value.GetOrdinal();
            case ObservationValueType.Count:
                return value.GetCount();
            case ObservationValueType.Amount:
                return value.GetAmount();
            case ObservationValueType.Ratio:
                return value.GetRatio();
            case ObservationValueType.Frequency:
                return value.GetFrequency();
            default:
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
                return decimal.MinValue; // unreachable
        }
    }
}
