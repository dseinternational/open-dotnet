// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of ASCII bytes.
/// </summary>
/// <remarks>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="byte"/>.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AsciiString
    : IEquatable<AsciiString>,
      IEquatable<ReadOnlyMemory<byte>>,
      IComparable<AsciiString>,
      IEqualityOperators<AsciiString, AsciiString, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiString>
{
    private readonly ReadOnlyMemory<byte> _value;

    public AsciiString(ReadOnlyMemory<char> value) : this(ToByteSpan(value.Span)) { }

    public AsciiString(ReadOnlySpan<char> value) : this(ToByteSpan(value)) { }

    public AsciiString(ReadOnlySpan<byte> value) : this((ReadOnlyMemory<byte>)value.ToArray()) { }

    public AsciiString(ReadOnlyMemory<byte> value) : this(value, false)
    {
    }

    private AsciiString(ReadOnlySpan<byte> value, bool skipValidation)
        : this((ReadOnlyMemory<byte>)value.ToArray(), skipValidation)
    {
    }

    private AsciiString(ReadOnlyMemory<byte> value, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureIsAscii(value.Span);
        }
        _value = value;
    }

    public static bool IsAscii(ReadOnlySpan<byte> value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsAscii(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static void EnsureIsAscii(ReadOnlySpan<byte> value)
    {
        if (!IsAscii(value))
        {
            ThrowHelper.ThrowArgumentException("Value must be ASCII.");
        }
    }

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    /// <summary>
    /// Searches for the specified value and returns the index of its first occurrence. If not found,
    /// returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IndexOf(byte value) => _value.Span.IndexOf(value);

    public ReadOnlyMemory<byte> AsMemory() => _value;

    public ReadOnlySpan<byte> AsSpan() => _value.Span;

    public byte[] ToByteArray() => _value.ToArray();

    public char[] ToCharArray() => AsciiString.ToCharArray(_value.Span);

    public static AsciiString Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static AsciiString Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    public static AsciiString Parse(string s)
        => Parse(s, default);

    public static AsciiString Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AsciiString result)
        => TryParse(s, default, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        byte[]? rented = null;

        try
        {
            Span<byte> buffer = s.Length <= 256
                ? stackalloc byte[s.Length]
                : (rented = ArrayPool<byte>.Shared.Rent(256));

            for (var i = 0; i < s.Length; i++)
            {
                var b = (byte)s[i];
                if (!AsciiChar.IsAscii(b))
                {
                    result = default;
                    return false;
                }
                buffer[i] = (byte)s[i];
            }

            result = new(buffer[..s.Length], true);
            return true;
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }

    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      out AsciiString result)
      => TryParse(s, default, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public int CompareTo(AsciiString other) => _value.Span.SequenceCompareTo(other._value.Span);

    public int CompareToCaseInsensitive(AsciiString other)
        => CompareToCaseInsensitive(other._value.Span);

    public int CompareToCaseInsensitive(ReadOnlySpan<byte> asciiBytes)
    {
        var length = Math.Min(_value.Length, asciiBytes.Length);

        for (var i = 0; i < length; i++)
        {
            var c = AsciiChar.CompareToCaseInsensitive(_value.Span[i], asciiBytes[i]);

            if (c != 0)
            {
                return c;
            }
        }

        return _value.Length - asciiBytes.Length;
    }

    public bool Equals(ReadOnlySpan<byte> other) => _value.Span.SequenceEqual(other);

    public bool Equals(ReadOnlyMemory<byte> other) => Equals(other.Span);

    public bool Equals(AsciiString other) => Equals(other._value);

    public bool EqualsCaseInsensitive(AsciiString other) => SequenceEqualsCaseInsenstive(_value.Span, other._value.Span);

    public override bool Equals(object? obj) => obj is AsciiString other && Equals(other);

    public override int GetHashCode()
    {
        var c = new HashCode();
        c.AddBytes(_value.Span);
        return c.ToHashCode();
    }

    public override string ToString() => ToString(default, default);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        char[]? rented = null;

        try
        {
            Span<char> buffer = _value.Length <= 128
                ? stackalloc char[_value.Length]
                : (rented = ArrayPool<char>.Shared.Rent(128));

            _ = TryFormat(buffer, out var charsWritten, format, formatProvider);

            return new string(buffer[..charsWritten]);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= _value.Length)
        {
            for (var i = 0; i < _value.Length; i++)
            {
                destination[i] = (char)_value.Span[i];
            }

            charsWritten = _value.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static bool operator ==(AsciiString left, AsciiString right) => left.Equals(right);

    public static bool operator !=(AsciiString left, AsciiString right) => !(left == right);

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static explicit operator AsciiString(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(AsciiString left, AsciiString right) => left.CompareTo(right) < 0;

    public static bool operator <=(AsciiString left, AsciiString right) => left.CompareTo(right) <= 0;

    public static bool operator >(AsciiString left, AsciiString right) => left.CompareTo(right) > 0;

    public static bool operator >=(AsciiString left, AsciiString right) => left.CompareTo(right) >= 0;

}
