// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A (non-negative) amount.
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonDecimalValueConverter<Amount>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Amount
    : IDivisibleValue<Amount, decimal>,
      IUtf8SpanSerializable<Amount>,
      IRepeatableHash64,
      IObservationValue
{
    /// <inheritdoc/>
    public static int MaxSerializedCharLength => 32;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => 32;

    /// <summary>
    /// An <see cref="Amount"/> with the value zero.
    /// </summary>
    public static Amount Zero { get; } = new(decimal.Zero);

    /// <summary>
    /// Initializes a new <see cref="Amount"/> with the specified value.
    /// </summary>
    /// <param name="value">The amount value.</param>
    public Amount(decimal value) : this(value, false) { }

    /// <summary>
    /// Initializes a new <see cref="Amount"/> from the specified <see cref="Half"/> value.
    /// </summary>
    /// <param name="value">The amount value.</param>
    public Amount(Half value) : this((decimal)value) { }

    /// <inheritdoc/>
    public MeasurementValueType ValueType => Observations.MeasurementValueType.Amount;

    /// <inheritdoc/>
    public static bool IsValidValue(decimal value)
    {
        return value >= Zero._value && value <= IObservationValue.MaxAmount;
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IObservationValue.GetBinary()
    {
        return IObservationValue.ThrowValueMismatchException<bool>();
    }

    byte IObservationValue.GetOrdinal()
    {
        return IObservationValue.ThrowValueMismatchException<byte>();
    }

    ulong IObservationValue.GetCount()
    {
        return IObservationValue.ThrowValueMismatchException<ulong>();
    }

    /// <inheritdoc/>
    public decimal GetAmount()
    {
        return _value;
    }

    decimal IObservationValue.GetRatio()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    decimal IObservationValue.GetFrequency()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }
}
