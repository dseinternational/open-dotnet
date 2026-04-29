// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A (non-negative) count.
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<Count>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Count
    : IDivisibleValue<Count, ulong>,
      IUtf8SpanSerializable<Count>,
      IRepeatableHash64,
      IObservationValue
{
    /// <summary>
    /// The maximum value permitted for a <see cref="Count"/>.
    /// </summary>
    public const ulong MaxValue = NumberHelper.MaxJsonSafeInteger;

    /// <inheritdoc/>
    public static int MaxSerializedCharLength => 16;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// A <see cref="Count"/> with the value zero.
    /// </summary>
    public static Count Zero { get; } = new(0);

    /// <inheritdoc/>
    public MeasurementValueType ValueType => Observations.MeasurementValueType.Count;

    /// <summary>
    /// Initializes a new <see cref="Count"/> with the specified value.
    /// </summary>
    /// <param name="value">The count value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is greater than <see cref="MaxValue"/>.</exception>
    public Count(ulong value) : this(value, false) { }

    /// <summary>
    /// Initializes a new <see cref="Count"/> with the specified value.
    /// </summary>
    /// <param name="value">The count value.</param>
    public Count(uint value) : this(value, true) { }

    /// <summary>
    /// Initializes a new <see cref="Count"/> with the specified value.
    /// </summary>
    /// <param name="value">The count value.</param>
    public Count(ushort value) : this(value, true) { }

    private Count(ulong value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="Count"/> value
    /// (in the inclusive range zero to <see cref="MaxValue"/>).
    /// </summary>
    /// <param name="value">The value to check.</param>
    public static bool IsValidValue(long value)
    {
        return value >= (long)Zero._value && value <= (long)MaxValue;
    }

    /// <inheritdoc/>
    public static bool IsValidValue(ulong value)
    {
        return value >= Zero._value && value <= MaxValue;
    }

    private static void EnsureIsValidValue(long value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(Count)} value");
        }
    }

    /// <summary>
    /// Attempts to create a <see cref="Count"/> from the specified value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">When this method returns, contains the resulting <see cref="Count"/>
    /// if conversion succeeded, or the default value if it failed.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromValue(long value, out Count result)
    {
        if (IsValidValue(value))
        {
            result = new Count((ulong)value, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Creates a <see cref="Count"/> from the specified value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Count"/> with the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid <see cref="Count"/> value.</exception>
    public static Count FromValue(long value)
    {
        EnsureIsValidValue(value);
        return new((ulong)value, true);
    }

    /// <summary>
    /// Attempts to create a <see cref="Count"/> from the specified value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">When this method returns, contains the resulting <see cref="Count"/>
    /// if conversion succeeded, or the default value if it failed.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromValue(int value, out Count result)
    {
        if (IsValidValue(value))
        {
            result = new Count((ulong)value, true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Creates a <see cref="Count"/> from the specified value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Count"/> with the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid <see cref="Count"/> value.</exception>
    public static Count FromValue(int value)
    {
        EnsureIsValidValue((long)value);
        return new((ulong)value, true);
    }

    /// <summary>
    /// Creates a <see cref="Count"/> from the specified value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="Count"/> with the specified value.</returns>
    public static Count FromValue(uint value)
    {
        return new(value, true);
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

    /// <inheritdoc/>
    public ulong GetCount()
    {
        return _value;
    }

    decimal IObservationValue.GetAmount()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    decimal IObservationValue.GetRatio()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    decimal IObservationValue.GetFrequency()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a <see cref="long"/> value to a <see cref="Count"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid <see cref="Count"/> value.</exception>
    public static explicit operator Count(long value)
    {
        return FromValue(value);
    }

    /// <summary>
    /// Implicitly converts a <see cref="Count"/> to a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator long(Count value)
    {
        return (long)value._value;
    }

    /// <summary>
    /// Explicitly converts an <see cref="int"/> value to a <see cref="Count"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid <see cref="Count"/> value.</exception>
    public static explicit operator Count(int value)
    {
        return FromValue(value);
    }

    /// <summary>
    /// Explicitly converts a <see cref="Count"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="OverflowException">The value is greater than <see cref="int.MaxValue"/>.</exception>
    public static explicit operator int(Count value)
    {
        return checked((int)value._value);
    }

    /// <summary>
    /// Implicitly converts a <see cref="uint"/> value to a <see cref="Count"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Count(uint value)
    {
        return FromValue(value);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
