// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An 8 byte version stamp.
/// </summary>
[JsonConverter(typeof(JsonStringTimestampConverter))]
public readonly record struct Timestamp : IComparable<Timestamp>, ISpanFormattable, ISpanParsable<Timestamp>
{
    /// <summary>
    /// An empty timestamp (all zero bytes).
    /// </summary>
    public static readonly Timestamp Empty;

    /// <summary>
    /// The size of a <see cref="Timestamp"/> in bytes.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// The length, in characters, of the base 64 encoded representation of a <see cref="Timestamp"/>.
    /// </summary>
    public const int Base64Length = 12;

    private readonly InlineArray8<byte> _bytes;

    /// <summary>
    /// Initialises a new <see cref="Timestamp"/> from a span of <see cref="Size"/> bytes.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="timestamp"/> is not <see cref="Size"/> bytes long.</exception>
    public Timestamp(ReadOnlySpan<byte> timestamp) : this(timestamp, true)
    {
    }

    private Timestamp(ReadOnlySpan<byte> timestamp, bool guard)
    {
        if (guard)
        {
            Guard.IsTrue(timestamp.Length == Size);
        }

        _bytes = Unsafe.As<byte, InlineArray8<byte>>(ref MemoryMarshal.GetReference(timestamp));
    }

    /// <summary>
    /// Returns a copy of the underlying bytes as a new array.
    /// </summary>
    public byte[] GetBytes() => AsSpan().ToArray();

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> view over the underlying bytes.
    /// </summary>
    public ReadOnlySpan<byte> AsSpan()
        => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<InlineArray8<byte>, byte>(ref Unsafe.AsRef(in _bytes)), Size);

    /// <inheritdoc/>
    public int CompareTo(Timestamp other) => AsSpan().SequenceCompareTo(other.AsSpan());

    // InlineArray types throw NotSupportedException from their generated Equals,
    // so we must provide explicit equality and hashing over the span.

    /// <summary>
    /// Returns <see langword="true"/> if the byte sequences are equal.
    /// </summary>
    public bool Equals(Timestamp other) => AsSpan().SequenceEqual(other.AsSpan());

    /// <inheritdoc/>
    public override int GetHashCode() => MemoryMarshal.Read<long>(AsSpan()).GetHashCode();

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[Base64Length];
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length < Base64Length)
        {
            charsWritten = 0;
            return this == Empty;
        }

        return Convert.TryToBase64Chars(AsSpan(), destination, out charsWritten);
    }

    /// <summary>
    /// Returns a base 64 encoded string representation of this timestamp.
    /// </summary>
    [SkipLocalsInit]
    public string ToBase64String()
    {
        Span<char> buffer = stackalloc char[Base64Length];
        _ = TryFormat(buffer, out var charsWritten, null, null);
        return buffer[..charsWritten].ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Returns a copy of the underlying bytes as a new array.
    /// </summary>
    public static explicit operator byte[](Timestamp timestamp)
    {
        return timestamp.GetBytes();
    }

    /// <summary>
    /// Creates a new <see cref="Timestamp"/> from <paramref name="data"/>.
    /// </summary>
    public static explicit operator Timestamp(byte[] data)
    {
        return new(data);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <inheritdoc/>
    public static Timestamp Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Timestamp result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public static Timestamp Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<Timestamp>("Could not parse timestamp");
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Timestamp result)
    {
        if (s.IsEmpty)
        {
            result = Empty;
            return true;
        }

        if (s.Length == Base64Length)
        {
            Span<byte> bytes = stackalloc byte[Size];

            if (Convert.TryFromBase64Chars(s, bytes, out var bytesWritten))
            {
                result = new(bytes[..bytesWritten], guard: false);
                return true;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Tries to create a <see cref="Timestamp"/> from <paramref name="data"/>. Empty input produces
    /// <see cref="Empty"/>; otherwise returns <see langword="false"/> if the length is not <see cref="Size"/>.
    /// </summary>
    public static bool TryCreate(ReadOnlySpan<byte> data, out Timestamp result)
    {
        if (data.IsEmpty)
        {
            result = default;
            return true;
        }

        if (data.Length != Size)
        {
            result = default;
            return false;
        }

        result = new(data, guard: false);
        return true;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>.</summary>
    public static bool operator <(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>.</summary>
    public static bool operator <=(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>.</summary>
    public static bool operator >(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>.</summary>
    public static bool operator >=(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) >= 0;
    }
}
