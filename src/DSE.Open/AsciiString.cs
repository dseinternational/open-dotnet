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
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="AsciiChar"/>.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AsciiString
    : IEquatable<AsciiString>,
      IEquatable<ReadOnlyMemory<AsciiChar>>,
      IComparable<AsciiString>,
      IEqualityOperators<AsciiString, AsciiString, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiString>
{
    private readonly ReadOnlyMemory<AsciiChar> _value;

    public AsciiString(ReadOnlySpan<AsciiChar> value) : this(value.ToArray())
    {
    }

    public AsciiString(AsciiChar[] value)
    {
        _value = value;
    }

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    /// <summary>
    /// Searches for the specified value and returns the index of its first occurrence. If not found,
    /// returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IndexOf(AsciiChar value) => _value.Span.IndexOf(value);

    public ReadOnlyMemory<AsciiChar> AsMemory() => _value;

    public ReadOnlySpan<AsciiChar> AsSpan() => _value.Span;

    public AsciiChar[] ToArray() => _value.ToArray();

    public byte[] ToByteArray()
    {
        if (_value.IsEmpty)
        {
            return Array.Empty<byte>();
        }

        var result = new byte[_value.Length];

        for (var i = 0; i < _value.Length; i++)
        {
            result[i] = (byte)_value.Span[i];
        }

        return result;
    }

    public char[] ToCharArray()
    {
        if (_value.IsEmpty)
        {
            return Array.Empty<char>();
        }

        var result = new char[_value.Length];

        for (var i = 0; i < _value.Length; i++)
        {
            result[i] = (char)_value.Span[i];
        }

        return result;
    }

    public static AsciiString Parse(ReadOnlySpan<char> s) => Parse(s, default);

    public static AsciiString Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    public static AsciiString Parse(string s) => Parse(s, default);

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
        AsciiChar[]? rented = null;

        try
        {
            Span<AsciiChar> buffer = s.Length <= 256
                ? stackalloc AsciiChar[s.Length]
                : (rented = ArrayPool<AsciiChar>.Shared.Rent(256));

            for (var i = 0; i < s.Length; i++)
            {
                var b = (byte)s[i];
                if (!AsciiChar.IsAscii(b))
                {
                    result = default;
                    return false;
                }

                buffer[i] = (AsciiChar)s[i];
            }

            result = new(buffer[..s.Length]);
            return true;
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<AsciiChar>.Shared.Return(rented);
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

    public int CompareToCaseInsensitive(ReadOnlySpan<AsciiChar> asciiBytes)
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

    public bool Equals(ReadOnlySpan<AsciiChar> other) => _value.Span.SequenceEqual(other);

    public bool Equals(ReadOnlyMemory<AsciiChar> other) => Equals(other.Span);

    public bool Equals(AsciiString other) => Equals(other._value);

    public bool EqualsCaseInsensitive(AsciiString other) => SequenceEqualsCaseInsenstive(_value.Span, other._value.Span);

    public override bool Equals(object? obj) => obj is AsciiString other && Equals(other);

    public override int GetHashCode()
    {
        var c = new HashCode();
        c.AddBytes(MemoryMarshal.Cast<AsciiChar, byte>(_value.Span));
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