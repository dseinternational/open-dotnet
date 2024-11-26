// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Observations;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

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
      IValueProvider
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public static Amount Zero { get; } = new(decimal.Zero);

    public Amount(decimal value) : this(value, false) { }

    public Amount(Half value) : this((decimal)value) { }

    public Observations.ValueType ValueType => Observations.ValueType.Amount;

    public static bool IsValidValue(decimal value)
    {
        return value >= Zero._value;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IValueProvider.GetBinary()
    {
        return IValueProvider.ThrowValueMismatchException<bool>();
    }

    byte IValueProvider.GetOrdinal()
    {
        return IValueProvider.ThrowValueMismatchException<byte>();
    }

    public ulong GetCount()
    {
        return IValueProvider.ThrowValueMismatchException<ulong>();
    }

    decimal IValueProvider.GetAmount()
    {
        return _value;
    }

    decimal IValueProvider.GetRatio()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    decimal IValueProvider.GetFrequency()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }
}
