// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A value used to identify a word.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<WordId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct WordId
    : IEquatableValue<WordId, ulong>,
      IUtf8SpanSerializable<WordId>
{
    public const ulong MinIdValue = 100000000001;
    public const ulong MaxIdValue = 999999999999;

    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public WordId(ulong value) : this(value, false)
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

    public static bool TryFromInt64(long value, out WordId id)
    {
        if (IsValidValue(value))
        {
            id = new WordId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    public static WordId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordId((ulong)value);
    }

    public static explicit operator WordId(long value)
    {
        return FromInt64(value);
    }

    public static long ToInt64(WordId value)
    {
        unchecked
        {
            return (long)value._value;
        }
    }

    public static implicit operator long(WordId value)
    {
        return ToInt64(value);
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static WordId GetRandomId()
    {
        return (WordId)(ulong)Random.Shared.NextInt64((long)MinIdValue, (long)MaxIdValue);
    }
#pragma warning restore CA5394 // Do not use insecure randomness
}
