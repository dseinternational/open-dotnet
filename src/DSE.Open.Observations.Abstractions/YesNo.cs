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
    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of a <see cref="YesNo"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 3;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of a <see cref="YesNo"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 3;

    /// <inheritdoc/>
    public MeasurementValueType ValueType => MeasurementValueType.Binary;

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="YesNo"/> value.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return value == Yes._value || value == No._value;
    }

    /// <summary>
    /// Represents the "Yes" choice.
    /// </summary>
    public static readonly YesNo Yes = new((AsciiString)"yes", true);

    /// <summary>
    /// Represents the "No" choice.
    /// </summary>
    public static readonly YesNo No = new((AsciiString)"no", true);

    /// <summary>
    /// Returns the <see cref="bool"/> equivalent of this value, where <see cref="Yes"/> is
    /// <see langword="true"/> and <see cref="No"/> is <see langword="false"/>.
    /// </summary>
    public bool ToBoolean()
    {
        return this == Yes;
    }

    /// <summary>
    /// Returns the <see cref="YesNo"/> equivalent of the specified <see cref="bool"/> value.
    /// </summary>
    public static YesNo FromBoolean(bool value)
    {
        return value ? Yes : No;
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Converts a <see cref="YesNo"/> to a <see cref="bool"/>.
    /// </summary>
    public static implicit operator bool(YesNo value)
    {
        return value.ToBoolean();
    }

    /// <summary>
    /// Converts a <see cref="bool"/> to a <see cref="YesNo"/>.
    /// </summary>
    public static implicit operator YesNo(bool value)
    {
        return FromBoolean(value);
    }
}
