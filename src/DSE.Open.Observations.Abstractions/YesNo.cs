// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Represents the selection of a choice between "Yes" and "No".
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<YesNo, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct YesNo
    : IEquatableValue<YesNo, AsciiString>,
      IUtf8SpanSerializable<YesNo>,
      IRepeatableHash64,
      IObservationValue
{
    public static int MaxSerializedCharLength => 3;

    public static int MaxSerializedByteLength => 3;

    public MeasurementValueType ValueType => MeasurementValueType.Binary;

    public static bool IsValidValue(AsciiString value)
    {
        return value == Yes._value || value == No._value;
    }

    public static readonly YesNo Yes = new((AsciiString)"yes", true);

    public static readonly YesNo No = new((AsciiString)"no", true);

    public bool ToBoolean()
    {
        return this == Yes;
    }

    public static YesNo FromBoolean(bool value)
    {
        return value ? Yes : No;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    public bool GetBinary()
    {
        return ToBoolean();
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

    decimal IObservationValue.GetRatio()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    decimal IObservationValue.GetFrequency()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    public static implicit operator bool(YesNo value)
    {
        return value.ToBoolean();
    }

    public static implicit operator YesNo(bool value)
    {
        return FromBoolean(value);
    }
}
