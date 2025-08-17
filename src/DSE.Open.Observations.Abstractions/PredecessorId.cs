// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Identifies a directed link between two measures.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<PredecessorId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct PredecessorId
    : IEquatableValue<PredecessorId, ulong>,
      IUtf8SpanSerializable<PredecessorId>,
      IRepeatableHash64
{
    public const ulong MinIdValue = 100000000001;
    public const ulong MaxIdValue = 999999999999;
    private const ulong MaxRange = MaxIdValue - MinIdValue;

    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public PredecessorId(ulong value) : this(value, false)
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

    public static bool TryFromInt64(long value, out PredecessorId id)
    {
        if (IsValidValue(value))
        {
            id = new PredecessorId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    public static PredecessorId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new PredecessorId((ulong)value);
    }

    public static explicit operator PredecessorId(long value)
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

    public static implicit operator long(PredecessorId value)
    {
        return (long)value._value;
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static PredecessorId GetRandomId()
    {
        return (PredecessorId)(ulong)Random.Shared.NextInt64((long)MinIdValue, (long)MaxIdValue);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

    public static PredecessorId FromUri(Uri urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        return FromUri(urn.ToString());
    }

    public static PredecessorId FromUri(ReadOnlySpan<char> urn)
    {
        var c = Encoding.UTF8.GetByteCount(urn);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(urn, b);
        return (PredecessorId)(MinIdValue + (ulong)(XxHash3.HashToUInt64(b) / (decimal)ulong.MaxValue * MaxRange));
    }

    public static PredecessorId FromMeasures(Uri head, Uri tail)
    {
        return FromMeasures(MeasureId.FromUri(head), MeasureId.FromUri(tail));
    }

    public static PredecessorId FromMeasures(MeasureId head, MeasureId tail)
    {
        Span<byte> b = stackalloc byte[16];
        _ = BitConverter.TryWriteBytes(b, head.ToUInt64());
        _ = BitConverter.TryWriteBytes(b[8..], tail.ToUInt64());
        return (PredecessorId)(MinIdValue + (ulong)(XxHash3.HashToUInt64(b) / (decimal)ulong.MaxValue * MaxRange));
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
