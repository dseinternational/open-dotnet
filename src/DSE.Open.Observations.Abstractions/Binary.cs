// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A binary observation value with the value <c>0</c> (false) or <c>1</c> (true).
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Binary, byte>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Binary
    : IEquatableValue<Binary, byte>,
      IUtf8SpanSerializable<Binary>,
      IRepeatableHash64,
      IObservationValue
{
    /// <inheritdoc/>
    public static int MaxSerializedCharLength => 1;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => 1;

    /// <inheritdoc/>
    public MeasurementValueType ValueType => MeasurementValueType.Binary;

    /// <inheritdoc/>
    public static bool IsValidValue(byte value)
    {
        return value < 2;
    }

    /// <summary>
    /// A <see cref="Binary"/> value representing <see langword="true"/>.
    /// </summary>
    public static readonly Binary True = new(1, true);

    /// <summary>
    /// A <see cref="Binary"/> value representing <see langword="false"/>.
    /// </summary>
    public static readonly Binary False = new(0, true);

    /// <summary>
    /// Returns the <see cref="bool"/> equivalent of this <see cref="Binary"/> value.
    /// </summary>
    /// <returns><see langword="true"/> if the value is <see cref="True"/>; otherwise, <see langword="false"/>.</returns>
    public bool ToBoolean()
    {
        return _value == 1;
    }

    /// <summary>
    /// Returns a <see cref="Binary"/> value equivalent to the specified <see cref="bool"/>.
    /// </summary>
    /// <param name="value">The <see cref="bool"/> value to convert.</param>
    /// <returns><see cref="True"/> if <paramref name="value"/> is <see langword="true"/>; otherwise, <see cref="False"/>.</returns>
    public static Binary FromBoolean(bool value)
    {
        return value ? True : False;
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
    /// Implicitly converts a <see cref="Binary"/> value to its <see cref="bool"/> equivalent.
    /// </summary>
    /// <param name="value">The <see cref="Binary"/> value to convert.</param>
    public static implicit operator bool(Binary value)
    {
        return value.ToBoolean();
    }

    /// <summary>
    /// Implicitly converts a <see cref="bool"/> value to its <see cref="Binary"/> equivalent.
    /// </summary>
    /// <param name="value">The <see cref="bool"/> value to convert.</param>
    public static implicit operator Binary(bool value)
    {
        return FromBoolean(value);
    }
}
