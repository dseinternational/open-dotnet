// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A rating of completeness.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonByteValueConverter<Completeness>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Completeness
    : IComparableValue<Completeness, byte>,
      IUtf8SpanSerializable<Completeness>,
      IRepeatableHash64,
      IValueProvider
{
    private const byte PartialValue = 10;
    private const byte DevelopingValue = 50;
    private const byte CompleteValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public ValueType ValueType => ValueType.Ordinal;

    public static bool IsValidValue(byte value)
    {
        return value is PartialValue or DevelopingValue or CompleteValue;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IValueProvider.GetBinary()
    {
        return IValueProvider.ThrowValueMismatchException<bool>();
    }

    public byte GetOrdinal()
    {
        return _value;
    }

    ulong IValueProvider.GetCount()
    {
        return IValueProvider.ThrowValueMismatchException<ulong>();
    }

    decimal IValueProvider.GetAmount()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    decimal IValueProvider.GetRatio()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    decimal IValueProvider.GetFrequency()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    public static Completeness Partial => new(PartialValue);

    public static Completeness Developing => new(DevelopingValue);

    public static Completeness Complete => new(CompleteValue);
}
