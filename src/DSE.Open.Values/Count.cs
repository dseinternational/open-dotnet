// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A (non-negative) count.
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonInt64ValueConverter<Count>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Count : IDivisibleValue<Count, long>, IUtf8SpanSerializable<Count>
{
    public const long MaxValue = NumberHelper.MaxJsonSafeInteger;

    public static int MaxSerializedCharLength => 10;

    public static int MaxSerializedByteLength => 10;

    public static Count Zero { get; } = new(0);

    public Count(long value) : this(value, false) { }

    public Count(ulong value) : this(value, false) { }

    public Count(int value) : this(value, false) { }

    public Count(uint value) : this(value, true) { }

    public Count(short value) : this(value, false) { }

    public Count(ushort value) : this(value, true) { }

    private Count(ulong value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = (long)value;
    }

    public static bool IsValidValue(long value)
    {
        return value >= Zero._value && value <= MaxValue;
    }

    public static bool IsValidValue(ulong value)
    {
        return value >= (ulong)Zero._value && value <= MaxValue;
    }

    private static void EnsureIsValidValue(ulong value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(Count)} value");
        }
    }

    public static bool TryFromValue(ulong value, out Count result)
    {
        if (IsValidValue(value))
        {
            result = new Count(value, true);
            return true;
        }

        result = default;
        return false;
    }

    public static Count FromValue(ulong value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static bool TryFromValue(int value, out Count result)
    {
        if (IsValidValue(value))
        {
            result = new Count(value, true);
            return true;
        }

        result = default;
        return false;
    }

    public static Count FromValue(int value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static Count FromValue(uint value)
    {
        return new(value, true);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Count(ulong value)
    {
        return FromValue(value);
    }

    public static implicit operator ulong(Count value)
    {
        return (ulong)value._value;
    }

    public static explicit operator Count(int value)
    {
        return FromValue(value);
    }

    public static explicit operator int(Count value)
    {
        return (int)value._value;
    }

    public static implicit operator Count(uint value)
    {
        return FromValue(value);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
