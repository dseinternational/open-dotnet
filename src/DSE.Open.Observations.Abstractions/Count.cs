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
    public const ulong MaxValue = NumberHelper.MaxJsonSafeInteger;

    public static int MaxSerializedCharLength => 10;

    public static int MaxSerializedByteLength => 10;

    public static Count Zero { get; } = new(0);

    public ObservationValueType ValueType => Observations.ObservationValueType.Count;

    public Count(ulong value) : this(value, false) { }

    public Count(uint value) : this(value, true) { }

    public Count(ushort value) : this(value, true) { }

    private Count(ulong value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    public static bool IsValidValue(long value)
    {
        return value >= (long)Zero._value && value <= (long)MaxValue;
    }

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

    public static Count FromValue(long value)
    {
        EnsureIsValidValue(value);
        return new((ulong)value, true);
    }

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

    public static Count FromValue(int value)
    {
        EnsureIsValidValue((ulong)value);
        return new((ulong)value, true);
    }

    public static Count FromValue(uint value)
    {
        return new(value, true);
    }

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

    public static explicit operator Count(long value)
    {
        return FromValue(value);
    }

    public static implicit operator long(Count value)
    {
        return (long)value._value;
    }

    public static explicit operator Count(int value)
    {
        return FromValue(value);
    }

    public static explicit operator int(Count value)
    {
        return (int)value._value;
    }

    public static implicit operator Count(uint value)
    {
        return FromValue(value);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
