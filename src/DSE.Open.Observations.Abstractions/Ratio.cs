// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Values.Text.Json.Serialization;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;

namespace DSE.Open.Observations;

/// <summary>
/// A value that expresses a ratio as a signed value between 0 and 1 (values between -1 and 1).
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
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static Ratio Zero { get; } = new(0);

    public Observations.ObservationValueType ValueType => Observations.ObservationValueType.Ratio;

    public Ratio(decimal value) : this(value, false) { }

    public static bool IsValidValue(decimal value)
    {
        return value is >= -1m and <= 1m;
    }

    public Percent ToPercent()
    {
        return (Percent)(_value * 100m);
    }

    public static explicit operator Ratio(Percent value)
    {
        return FromPercent(value);
    }

    public static Ratio FromPercent(Percent value)
    {
        return new((decimal)value / 100m);
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

    ulong IObservationValue.GetCount()
    {
        return IObservationValue.ThrowValueMismatchException<ulong>();
    }

    decimal IObservationValue.GetAmount()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    public decimal GetRatio()
    {
        return _value;
    }

    decimal IObservationValue.GetFrequency()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }
}
