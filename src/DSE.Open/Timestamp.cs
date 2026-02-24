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
    public static readonly Timestamp Empty;

    public const int Size = 8;

    public const int Base64Length = 12;

    private readonly InlineArray8<byte> _bytes;

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

    public byte[] GetBytes() => AsSpan().ToArray();

    public ReadOnlySpan<byte> AsSpan()
        => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<InlineArray8<byte>, byte>(ref Unsafe.AsRef(in _bytes)), Size);

    public int CompareTo(Timestamp other) => AsSpan().SequenceCompareTo(other.AsSpan());

    // InlineArray types throw NotSupportedException from their generated Equals,
    // so we must provide explicit equality and hashing over the span.

    public bool Equals(Timestamp other) => AsSpan().SequenceEqual(other.AsSpan());

    public override int GetHashCode() => MemoryMarshal.Read<long>(AsSpan()).GetHashCode();

    public override string ToString()
    {
        return ToString(null, null);
    }

    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[Base64Length];
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

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

    [SkipLocalsInit]
    public string ToBase64String()
    {
        Span<char> buffer = stackalloc char[Base64Length];
        _ = TryFormat(buffer, out var charsWritten, null, null);
        return buffer[..charsWritten].ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator byte[](Timestamp timestamp)
    {
        return timestamp.GetBytes();
    }

    public static explicit operator Timestamp(byte[] data)
    {
        return new(data);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static Timestamp Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Timestamp result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static Timestamp Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<Timestamp>("Could not parse timestamp");
    }

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

    public static bool operator <(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Timestamp left, Timestamp right)
    {
        return left.CompareTo(right) >= 0;
    }
}
