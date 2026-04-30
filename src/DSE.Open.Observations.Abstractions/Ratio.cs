// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Values.Text.Json.Serialization;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;

namespace DSE.Open.Observations;

/// <summary>
/// A value that expresses a ratio as a signed value in the inclusive range -1 to 1.
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonDecimalValueConverter<Ratio>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Ratio
    : IDivisibleValue<Ratio, decimal>,
      IUtf8SpanSerializable<Ratio>,
      IRepeatableHash64,
      IObservationValue
{
    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of a <see cref="Ratio"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of a <see cref="Ratio"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 32;

    /// <summary>
    /// Gets a <see cref="Ratio"/> representing zero.
    /// </summary>
    public static Ratio Zero { get; } = new(0);

    /// <inheritdoc/>
    public MeasurementValueType ValueType => Observations.MeasurementValueType.Ratio;

    /// <summary>
    /// Initializes a new <see cref="Ratio"/> from the specified value.
    /// </summary>
    /// <param name="value">The underlying value, in the inclusive range -1 to 1.</param>
    public Ratio(decimal value) : this(value, false) { }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="Ratio"/> value.
    /// </summary>
    public static bool IsValidValue(decimal value)
    {
        return value is >= -1m and <= 1m;
    }

    /// <summary>
    /// Returns this ratio expressed as a <see cref="Percent"/>.
    /// </summary>
    public Percent ToPercent()
    {
        return (Percent)(_value * 100m);
    }

    /// <summary>
    /// Converts a <see cref="Percent"/> to a <see cref="Ratio"/>.
    /// </summary>
    public static explicit operator Ratio(Percent value)
    {
        return FromPercent(value);
    }

    /// <summary>
    /// Returns the specified <see cref="Percent"/> expressed as a <see cref="Ratio"/>.
    /// </summary>
    public static Ratio FromPercent(Percent value)
    {
        return new((decimal)value / 100m);
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

    decimal IObservationValue.GetAmount()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    /// <inheritdoc/>
    public decimal GetRatio()
    {
        return _value;
    }

    decimal IObservationValue.GetFrequency()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }
}
