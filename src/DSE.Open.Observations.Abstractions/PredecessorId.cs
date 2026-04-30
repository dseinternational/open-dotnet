// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Runtime.Helpers;
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
    /// <summary>
    /// The minimum value of a <see cref="PredecessorId"/>.
    /// </summary>
    public const ulong MinIdValue = 100000000001;

    /// <summary>
    /// The maximum value of a <see cref="PredecessorId"/>.
    /// </summary>
    public const ulong MaxIdValue = 999999999999;
    private const ulong MaxRange = MaxIdValue - MinIdValue;

    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of a <see cref="PredecessorId"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of a <see cref="PredecessorId"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="PredecessorId"/> from the specified value.
    /// </summary>
    /// <param name="value">The underlying identifier value.</param>
    public PredecessorId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="PredecessorId"/> value.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= MaxIdValue and >= MinIdValue;
    }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="PredecessorId"/> value.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)MaxIdValue) and >= ((long)MinIdValue);
    }

    /// <summary>
    /// Attempts to create a <see cref="PredecessorId"/> from the specified <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="id">When this method returns, contains the resulting <see cref="PredecessorId"/>
    /// if conversion succeeded, or the default value otherwise.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Creates a <see cref="PredecessorId"/> from the specified <see cref="long"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid value.</exception>
    public static PredecessorId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new PredecessorId((ulong)value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> value to a <see cref="PredecessorId"/>.
    /// </summary>
    public static explicit operator PredecessorId(long value)
    {
        return FromInt64(value);
    }

    /// <summary>
    /// Returns the underlying value as a <see cref="long"/>.
    /// </summary>
    public long ToInt64()
    {
        unchecked
        {
            return (long)_value;
        }
    }

    /// <summary>
    /// Returns the underlying value as a <see cref="ulong"/>.
    /// </summary>
    public ulong ToUInt64()
    {
        return _value;
    }

    /// <summary>
    /// Converts a <see cref="PredecessorId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(PredecessorId value)
    {
        return (long)value._value;
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    /// <summary>
    /// Returns a randomly generated <see cref="PredecessorId"/> in the inclusive range
    /// <see cref="MinIdValue"/> to <see cref="MaxIdValue"/>.
    /// </summary>
    public static PredecessorId GetRandomId()
    {
        return (PredecessorId)(ulong)Random.Shared.NextInt64((long)MinIdValue, (long)MaxIdValue + 1);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

    /// <summary>
    /// Derives a <see cref="PredecessorId"/> from the specified URI.
    /// </summary>
    /// <param name="urn">The URI from which to derive the identifier.</param>
    public static PredecessorId FromUri(Uri urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        return FromUri(urn.ToString());
    }

    /// <summary>
    /// Derives a <see cref="PredecessorId"/> from the specified URI.
    /// </summary>
    /// <param name="urn">The URI from which to derive the identifier.</param>
    public static PredecessorId FromUri(ReadOnlySpan<char> urn)
    {
        var c = Encoding.UTF8.GetByteCount(urn);

        byte[]? rented = null;
        Span<byte> b = MemoryThresholds.CanStackalloc<byte>(c)
            ? stackalloc byte[c]
            : (rented = ArrayPool<byte>.Shared.Rent(c));

        try
        {
            var written = Encoding.UTF8.GetBytes(urn, b);
            return (PredecessorId)(MinIdValue + (ulong)(XxHash3.HashToUInt64(b[..written]) / (decimal)ulong.MaxValue * MaxRange));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }

    /// <summary>
    /// Derives a <see cref="PredecessorId"/> for a directed link from the measure identified by
    /// <paramref name="head"/> to the measure identified by <paramref name="tail"/>.
    /// </summary>
    public static PredecessorId FromMeasures(Uri head, Uri tail)
    {
        return FromMeasures(MeasureId.FromUri(head), MeasureId.FromUri(tail));
    }

    /// <summary>
    /// Derives a <see cref="PredecessorId"/> for a directed link from the measure identified by
    /// <paramref name="head"/> to the measure identified by <paramref name="tail"/>.
    /// </summary>
    public static PredecessorId FromMeasures(MeasureId head, MeasureId tail)
    {
        Span<byte> b = stackalloc byte[16];
        _ = BitConverter.TryWriteBytes(b, head.ToUInt64());
        _ = BitConverter.TryWriteBytes(b[8..], tail.ToUInt64());
        return (PredecessorId)(MinIdValue + (ulong)(XxHash3.HashToUInt64(b) / (decimal)ulong.MaxValue * MaxRange));
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
