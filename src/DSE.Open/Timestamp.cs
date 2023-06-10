// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An 8 byte version stamp.
/// </summary>
[JsonConverter(typeof(JsonStringTimestampConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct Timestamp : IComparable<Timestamp>, ISpanFormattable, ISpanParsable<Timestamp>
{
    public static readonly Timestamp Empty;

    public const int Size = 8;

    public const int Base64Length = 12;

    private readonly byte _b0, _b1, _b2, _b3, _b4, _b5, _b6, _b7;

    public Timestamp(ReadOnlySpan<byte> timestamp) : this(timestamp, true)
    {
    }

    private Timestamp(ReadOnlySpan<byte> timestamp, bool guard)
    {
        if (guard)
        {
            Guard.IsTrue(timestamp.Length == Size);
        }

        _b0 = timestamp[0];
        _b1 = timestamp[1];
        _b2 = timestamp[2];
        _b3 = timestamp[3];
        _b4 = timestamp[4];
        _b5 = timestamp[5];
        _b6 = timestamp[6];
        _b7 = timestamp[7];
    }

    public byte[] GetBytes() => new[] { _b0, _b1, _b2, _b3, _b4, _b5, _b6, _b7 };

    public int CompareTo(Timestamp other)
    {
        ReadOnlySpan<byte> v1 = stackalloc[] { _b0, _b1, _b2, _b3, _b4, _b5, _b6, _b7 };
        ReadOnlySpan<byte> v2 = stackalloc[]
        {
            other._b0,
            other._b1,
            other._b2,
            other._b3,
            other._b4,
            other._b5,
            other._b6,
            other._b7
        };
        return v1.SequenceCompareTo(v2);
    }

    public override int GetHashCode() => HashCode.Combine(_b0, _b1, _b2, _b3, _b4, _b5, _b6, _b7);

    public override string ToString() => ToString(null, null);

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

        ReadOnlySpan<byte> bytes = stackalloc[] { _b0, _b1, _b2, _b3, _b4, _b5, _b6, _b7 };
        return Convert.TryToBase64Chars(bytes, destination, out charsWritten);
    }

    public string ToBase64String()
    {
        Span<char> buffer = stackalloc char[Base64Length];
        _ = TryFormat(buffer, out var charsWritten, null, null);
        return buffer[..charsWritten].ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator byte[](Timestamp timestamp) => timestamp.GetBytes();

    public static explicit operator Timestamp(byte[] data) => new(data);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static Timestamp Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
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
                result = new Timestamp(bytes[..bytesWritten], guard: false);
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

        result = new Timestamp(data, guard: false);
        return true;
    }

    public static bool operator <(Timestamp left, Timestamp right) => left.CompareTo(right) < 0;

    public static bool operator <=(Timestamp left, Timestamp right) => left.CompareTo(right) <= 0;

    public static bool operator >(Timestamp left, Timestamp right) => left.CompareTo(right) > 0;

    public static bool operator >=(Timestamp left, Timestamp right) => left.CompareTo(right) >= 0;
}
