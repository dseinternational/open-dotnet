// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;
using DSE.Open.Values;
using System.IO.Hashing;
using System.Text;
using DSE.Open.Hashing;

namespace DSE.Open.Observations;

/// <summary>
/// A value used to identify a Measure.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<MeasureId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct MeasureId
    : IEquatableValue<MeasureId, ulong>,
      IUtf8SpanSerializable<MeasureId>,
      IRepeatableHash64
{
    public const ulong MinIdValue = 100000000001;
    public const ulong MaxIdValue = 999999999999;
    private const ulong MaxRange = MaxIdValue - MinIdValue;

    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public MeasureId(ulong value) : this(value, false)
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

    public static bool TryFromInt64(long value, out MeasureId id)
    {
        if (IsValidValue(value))
        {
            id = new MeasureId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    public static MeasureId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new MeasureId((ulong)value);
    }

    public static explicit operator MeasureId(long value)
    {
        return FromInt64(value);
    }

    public long ToInt64()
    {
        unchecked
        {
            return (long)_value;
        }
    }

    public ulong ToUInt64()
    {
        return _value;
    }

    public static implicit operator long(MeasureId value)
    {
        return (long)value._value;
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static MeasureId GetRandomId()
    {
        return (MeasureId)(ulong)Random.Shared.NextInt64((long)MinIdValue, (long)MaxIdValue);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

    public static MeasureId FromUri(Uri urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        return FromUri(urn.ToString());
    }

    public static MeasureId FromUri(ReadOnlySpan<char> urn)
    {
        var c = Encoding.UTF8.GetByteCount(urn);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(urn, b);
        return (MeasureId)(MinIdValue + (ulong)(XxHash3.HashToUInt64(b) / (decimal)ulong.MaxValue * MaxRange));
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
