// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="byte"/>.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct AsciiCharSequence
    : IEquatable<AsciiCharSequence>,
      IEquatable<ReadOnlyMemory<byte>>,
      IComparable<AsciiCharSequence>,
      IEqualityOperators<AsciiCharSequence, AsciiCharSequence, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiCharSequence>
{
    private readonly ReadOnlyMemory<byte> _value;

    public AsciiCharSequence(ReadOnlyMemory<char> value) : this(AsciiChar.ToByteSpan(value.Span)) { }

    public AsciiCharSequence(ReadOnlySpan<char> value) : this(AsciiChar.ToByteSpan(value)) { }

    public AsciiCharSequence(ReadOnlySpan<byte> value) : this((ReadOnlyMemory<byte>)value.ToArray()) { }

    public AsciiCharSequence(ReadOnlyMemory<byte> value)
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
    public int IndexOf(byte value) => _value.Span.IndexOf(value);

    public ReadOnlyMemory<byte> AsMemory() => _value;

    public ReadOnlySpan<byte> AsSpan() => _value.Span;

    public ReadOnlySpan<char> ToCharSpan() => AsciiChar.ToCharSpan(_value.Span);

    public static AsciiCharSequence Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static AsciiCharSequence Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid {nameof(AsciiCharSequence)} value.");
        return default; // unreachable
    }

    public static AsciiCharSequence Parse(string s)
        => Parse(s, default);

    public static AsciiCharSequence Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AsciiCharSequence result)
        => TryParse(s, default, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiCharSequence result)
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

            result = new(buffer[..s.Length]);
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
      out AsciiCharSequence result)
      => TryParse(s, default, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiCharSequence result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public int CompareTo(AsciiCharSequence other) => _value.Span.SequenceCompareTo(other._value.Span);

    public int CompareToCaseInsensitive(AsciiCharSequence other)
    {
        var length = Math.Min(_value.Length, other._value.Length);

        for (var i = 0; i < length; i++)
        {
            var c = AsciiChar.CompareToCaseInsenstive(_value.Span[i], other._value.Span[i]);

            if (c != 0)
            {
                return c;
            }
        }

        return _value.Length - other._value.Length;
    }

    public bool Equals(ReadOnlySpan<byte> other) => _value.Span.SequenceEqual(other);

    public bool Equals(ReadOnlyMemory<byte> other) => Equals(other.Span);

    public bool Equals(AsciiCharSequence other) => Equals(other._value);

    public override bool Equals(object? obj) => obj is AsciiCharSequence other && Equals(other);

    public override int GetHashCode() => base.GetHashCode(); // TODO

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

    public static bool operator ==(AsciiCharSequence left, AsciiCharSequence right) => left.Equals(right);

    public static bool operator !=(AsciiCharSequence left, AsciiCharSequence right) => !(left == right);

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static explicit operator AsciiCharSequence(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(AsciiCharSequence left, AsciiCharSequence right) => left.CompareTo(right) < 0;

    public static bool operator <=(AsciiCharSequence left, AsciiCharSequence right) => left.CompareTo(right) <= 0;

    public static bool operator >(AsciiCharSequence left, AsciiCharSequence right) => left.CompareTo(right) > 0;

    public static bool operator >=(AsciiCharSequence left, AsciiCharSequence right) => left.CompareTo(right) >= 0;

}
