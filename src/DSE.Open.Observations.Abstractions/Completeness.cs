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
      IObservationValue
{
    private const byte PartialValue = 10;
    private const byte DevelopingValue = 50;
    private const byte CompleteValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public ObservationValueType ValueType => ObservationValueType.Ordinal;

    public static bool IsValidValue(byte value)
    {
        return value is PartialValue or DevelopingValue or CompleteValue;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IObservationValue.GetBinary()
    {
        return IObservationValue.ThrowValueMismatchException<bool>();
    }

    public byte GetOrdinal()
    {
        return _value;
    }

    ulong IObservationValue.GetCount()
    {
        return IObservationValue.ThrowValueMismatchException<ulong>();
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

    public static Completeness Partial => new(PartialValue);

    public static Completeness Developing => new(DevelopingValue);

    public static Completeness Complete => new(CompleteValue);
}
