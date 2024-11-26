// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<YesNo, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Binary
    : IEquatableValue<Binary, byte>,
      IUtf8SpanSerializable<Binary>,
      IRepeatableHash64,
      IValueProvider
{
    public static int MaxSerializedCharLength => 1;

    public static int MaxSerializedByteLength => 1;

    public ValueType ValueType => ValueType.Binary;

    public static bool IsValidValue(byte value)
    {
        return value < 2;
    }

    public static readonly Binary True = new(1, true);

    public static readonly Binary False = new(0, true);

    public bool ToBoolean()
    {
        return _value == 1;
    }

    public static Binary FromBoolean(bool value)
    {
        return value ? True : False;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    public bool GetBinary()
    {
        return ToBoolean();
    }

    byte IValueProvider.GetOrdinal()
    {
        return IValueProvider.ThrowValueMismatchException<byte>();
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

    public static implicit operator bool(Binary value)
    {
        return value.ToBoolean();
    }

    public static implicit operator Binary(bool value)
    {
        return FromBoolean(value);
    }
}
