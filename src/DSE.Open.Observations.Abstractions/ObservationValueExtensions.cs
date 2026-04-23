// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Conversion helpers that project an <see cref="IObservationValue"/> to a
/// uniform numeric representation, regardless of the underlying
/// <see cref="MeasurementValueType"/>.
/// </summary>
public static class ObservationValueExtensions
{
    /// <summary>
    /// Converts an observation value to <see cref="double"/>, dispatching on
    /// <see cref="IObservationValue.ValueType"/>.
    /// </summary>
    /// <param name="value">The observation value. Must not be null.</param>
    /// <returns>
    /// A <see cref="double"/> representation: <c>0</c> or <c>1</c> for
    /// <see cref="MeasurementValueType.Binary"/>; the underlying numeric value
    /// for ordinal, count, amount, ratio, and frequency types.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> reports a <see cref="MeasurementValueType"/>
    /// not handled by this method.
    /// </exception>
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

    /// <summary>
    /// Converts an observation value to <see cref="decimal"/>, dispatching on
    /// <see cref="IObservationValue.ValueType"/>.
    /// </summary>
    /// <param name="value">The observation value. Must not be null.</param>
    /// <returns>
    /// A <see cref="decimal"/> representation: <c>0</c> or <c>1</c> for
    /// <see cref="MeasurementValueType.Binary"/>; the underlying value for
    /// ordinal, count, amount, ratio, and frequency types.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> reports a <see cref="MeasurementValueType"/>
    /// not handled by this method.
    /// </exception>
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
