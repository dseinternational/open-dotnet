// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Security;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A value used to identify a word.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<ObservationId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct ObservationId
    : IEquatableValue<ObservationId, ulong>,
      IUtf8SpanSerializable<ObservationId>
{
    public const ulong MinIdValue = 1;
    public const ulong MaxIdValue = NumberHelper.MaxJsonSafeInteger;

    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public ObservationId(ulong value) : this(value, false)
    {
    }

    public static bool IsValidValue(ulong value)
    {
        return value is <= MaxIdValue and >= MinIdValue;
    }

    public static bool IsValidValue(long value)
    {
        return value is <= ((long)MaxIdValue) and >= ((long)MinIdValue);
    }

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

    public static ObservationId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new ObservationId((ulong)value);
    }

    public static explicit operator ObservationId(long value)
    {
        return FromInt64(value);
    }

    public static long ToInt64(ObservationId value)
    {
        unchecked
        {
            return (long)value._value;
        }
    }

    public static implicit operator long(ObservationId value)
    {
        return ToInt64(value);
    }

    public static ObservationId GetRandomId()
    {
        return (ObservationId)RandomValueGenerator.GetUInt64Value(MinIdValue, MaxIdValue);
    }
}
