// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Security;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A value used to identify an <see cref="IObservation"/>.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<ObservationId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct ObservationId
    : IEquatableValue<ObservationId, ulong>,
      IUtf8SpanSerializable<ObservationId>,
      IRepeatableHash64
{
    /// <summary>
    /// The minimum value of an <see cref="ObservationId"/>.
    /// </summary>
    public const ulong MinIdValue = 1;

    /// <summary>
    /// The maximum value of an <see cref="ObservationId"/>.
    /// </summary>
    public const ulong MaxIdValue = NumberHelper.MaxJsonSafeInteger;

    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of an <see cref="ObservationId"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of an <see cref="ObservationId"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="ObservationId"/> from the specified value.
    /// </summary>
    /// <param name="value">The underlying identifier value.</param>
    public ObservationId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="ObservationId"/> value.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= MaxIdValue and >= MinIdValue;
    }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="ObservationId"/> value.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)MaxIdValue) and >= ((long)MinIdValue);
    }

    /// <summary>
    /// Attempts to create an <see cref="ObservationId"/> from the specified <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="id">When this method returns, contains the resulting <see cref="ObservationId"/>
    /// if conversion succeeded, or the default value otherwise.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromInt64(long value, out ObservationId id)
    {
        if (IsValidValue(value))
        {
            id = new ObservationId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Creates an <see cref="ObservationId"/> from the specified <see cref="long"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid value.</exception>
    public static ObservationId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new ObservationId((ulong)value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> value to an <see cref="ObservationId"/>.
    /// </summary>
    public static explicit operator ObservationId(long value)
    {
        return FromInt64(value);
    }

    /// <summary>
    /// Returns the underlying value of the specified <see cref="ObservationId"/> as a <see cref="long"/>.
    /// </summary>
    public static long ToInt64(ObservationId value)
    {
        unchecked
        {
            return (long)value._value;
        }
    }

    /// <summary>
    /// Converts an <see cref="ObservationId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(ObservationId value)
    {
        return ToInt64(value);
    }

    /// <summary>
    /// Returns a randomly generated <see cref="ObservationId"/> in the inclusive range
    /// <see cref="MinIdValue"/> to <see cref="MaxIdValue"/>.
    /// </summary>
    public static ObservationId GetRandomId()
    {
        return (ObservationId)RandomValueGenerator.GetUInt64Value(MinIdValue, MaxIdValue + 1);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
