// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Runtime.Helpers;
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
    /// <summary>
    /// The minimum value of a <see cref="MeasureId"/>.
    /// </summary>
    public const ulong MinIdValue = 100000000001;

    /// <summary>
    /// The maximum value of a <see cref="MeasureId"/>.
    /// </summary>
    public const ulong MaxIdValue = 999999999999;
    private const ulong MaxRange = MaxIdValue - MinIdValue;

    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of a <see cref="MeasureId"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of a <see cref="MeasureId"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="MeasureId"/> from the specified value.
    /// </summary>
    /// <param name="value">The underlying identifier value.</param>
    public MeasureId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="MeasureId"/> value.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= MaxIdValue and >= MinIdValue;
    }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="MeasureId"/> value.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)MaxIdValue) and >= ((long)MinIdValue);
    }

    /// <summary>
    /// Attempts to create a <see cref="MeasureId"/> from the specified <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="id">When this method returns, contains the resulting <see cref="MeasureId"/>
    /// if conversion succeeded, or the default value otherwise.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Creates a <see cref="MeasureId"/> from the specified <see cref="long"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not a valid value.</exception>
    public static MeasureId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new MeasureId((ulong)value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> value to a <see cref="MeasureId"/>.
    /// </summary>
    public static explicit operator MeasureId(long value)
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
    /// Converts a <see cref="MeasureId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(MeasureId value)
    {
        return (long)value._value;
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    /// <summary>
    /// Returns a randomly generated <see cref="MeasureId"/> in the inclusive range
    /// <see cref="MinIdValue"/> to <see cref="MaxIdValue"/>.
    /// </summary>
    public static MeasureId GetRandomId()
    {
        return (MeasureId)(ulong)Random.Shared.NextInt64((long)MinIdValue, (long)MaxIdValue + 1);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

    /// <summary>
    /// Derives a <see cref="MeasureId"/> from the specified URI.
    /// </summary>
    /// <param name="urn">The URI from which to derive the identifier.</param>
    public static MeasureId FromUri(Uri urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        return FromUri(urn.ToString());
    }

    /// <summary>
    /// Derives a <see cref="MeasureId"/> from the specified URI.
    /// </summary>
    /// <param name="urn">The URI from which to derive the identifier.</param>
    public static MeasureId FromUri(ReadOnlySpan<char> urn)
    {
        var c = Encoding.UTF8.GetByteCount(urn);

        byte[]? rented = null;
        Span<byte> b = MemoryThresholds.CanStackalloc<byte>(c)
            ? stackalloc byte[c]
            : (rented = ArrayPool<byte>.Shared.Rent(c));

        try
        {
            var written = Encoding.UTF8.GetBytes(urn, b);
            return (MeasureId)(MinIdValue + (ulong)(XxHash3.HashToUInt64(b[..written]) / (decimal)ulong.MaxValue * MaxRange));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
