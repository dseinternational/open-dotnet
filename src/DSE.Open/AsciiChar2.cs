// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of two ASCII bytes.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct AsciiChar2
    : IComparable<AsciiChar2>,
      IEquatable<AsciiChar2>,
      IEqualityOperators<AsciiChar2, AsciiChar2, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiChar2>,
      IConvertibleTo<AsciiChar2, string>,
      ITryConvertibleFrom<AsciiChar2, string>
{
    private const int CharCount = 2;

    // internal for AsciiChar2Comparer
    internal readonly AsciiChar _c0;
    internal readonly AsciiChar _c1;

    public AsciiChar2(AsciiChar c0, AsciiChar c1)
    {
        _c0 = c0;
        _c1 = c1;
    }

    public AsciiChar2(byte c0, byte c1)
    {
        _c0 = (AsciiChar)c0;
        _c1 = (AsciiChar)c1;
    }

    public AsciiChar2(char c0, char c1)
    {
        _c0 = (AsciiChar)c0;
        _c1 = (AsciiChar)c1;
    }

    public AsciiChar2((AsciiChar c0, AsciiChar c1) value) : this(value.c0, value.c1)
    {
    }

    public AsciiChar2((byte c0, byte c1) value) : this(value.c0, value.c1)
    {
    }

    public AsciiChar2((char c0, char c1) value) : this(value.c0, value.c1)
    {
    }

    public AsciiChar2(ReadOnlySpan<AsciiChar> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = span[0];
        _c1 = span[1];
    }

    public AsciiChar2(ReadOnlySpan<byte> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = (AsciiChar)span[0];
        _c1 = (AsciiChar)span[1];
    }

    public AsciiChar2(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = (AsciiChar)span[0];
        _c1 = (AsciiChar)span[1];
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out AsciiChar c0, out AsciiChar c1)
    {
        c0 = _c0;
        c1 = _c1;
    }

    public int CompareTo(AsciiChar2 other)
    {
        var c = _c0.CompareTo(other._c0);
        return c != 0 ? c : _c1.CompareTo(other._c1);
    }

    public bool Equals(AsciiChar2 other) => _c0 == other._c0 && _c1 == other._c1;

    public bool EqualsCaseInsensitive(AsciiChar2 other)
        => AsciiChar.EqualsCaseInsensitive(_c0, other._c0) && AsciiChar.EqualsCaseInsensitive(_c1, other._c1);

    public int CompareToCaseInsensitive(AsciiChar2 other)
    {
        var c = AsciiChar.CompareToCaseInsensitive(_c0, other._c0);

        return c != 0 ? c : AsciiChar.CompareToCaseInsensitive(_c1, other._c1);
    }

    public bool Equals(string other) => Equals(other.AsSpan());

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => other.Length == CharCount && other[0] == _c0 && other[1] == _c1;

    public override bool Equals(object? obj) => obj is AsciiChar2 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_c0, _c1);

    public override string ToString() => ToString(null, null);

    public Char2 ToChar2() => new((char)_c0, (char)_c1);

    public char[] ToCharArray() => new[] { (char)_c0, _c1 };

    public static AsciiChar2 FromString(string value) => new(value.AsSpan());

    public static AsciiChar2 FromSpan(ReadOnlySpan<AsciiChar> span) => new(span);

    public static AsciiChar2 FromByteSpan(ReadOnlySpan<byte> span) => new(span);

    public static AsciiChar2 FromCharSpan(ReadOnlySpan<char> span) => new(span);

    public static bool operator ==(AsciiChar2 left, AsciiChar2 right) => left.Equals(right);

    public static bool operator !=(AsciiChar2 left, AsciiChar2 right) => !left.Equals(right);

    public static implicit operator string(AsciiChar2 value) => value.ToString();

    public static implicit operator Char2(AsciiChar2 value) => value.ToChar2();

    public static explicit operator AsciiChar2(string value) => FromString(value);

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AsciiChar2(ReadOnlySpan<byte> value) => FromByteSpan(value);

    public static explicit operator AsciiChar2(ReadOnlySpan<char> value) => FromCharSpan(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public AsciiChar2 ToUpper() => new(_c0.ToUpper(), _c1.ToUpper());

    public AsciiChar2 ToLower() => new(_c0.ToLower(), _c1.ToLower());

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= CharCount)
        {
            destination[0] = (char)_c0;
            destination[1] = (char)_c1;
            charsWritten = CharCount;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
        => string.Create(CharCount, this, (span, char2) =>
        {
            span[0] = char2._c0;
            span[1] = char2._c1;
        });

    public string ToStringLower(string? format, IFormatProvider? formatProvider)
        => string.Create(CharCount, this, (span, char2) =>
        {
            span[0] = char2._c0.ToLower();
            span[1] = char2._c1.ToLower();
        });

    public string ToStringUpper(string? format, IFormatProvider? formatProvider)
        => string.Create(CharCount, this, (span, char2) =>
        {
            span[0] = char2._c0.ToUpper();
            span[1] = char2._c1.ToUpper();
        });

    public static AsciiChar2 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(AsciiChar2)}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out AsciiChar2 result)
    {
        if (s.Length >= CharCount && AsciiChar.IsAscii(s[0])
            && AsciiChar.IsAscii(s[1]))
        {
            result = new AsciiChar2(s);
            return true;
        }

        result = default;
        return false;
    }

    public static AsciiChar2 Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out AsciiChar2 result)
        => TryParse(s.AsSpan(), provider, out result);

    static string IConvertibleTo<AsciiChar2, string>.ConvertTo(AsciiChar2 value) => value.ToString();

    static bool ITryConvertibleFrom<AsciiChar2, string>.TryFromValue(string value, out AsciiChar2 result)
        => TryParse(value, null, out result);

    public static bool operator <(AsciiChar2 left, AsciiChar2 right) => left.CompareTo(right) < 0;

    public static bool operator <=(AsciiChar2 left, AsciiChar2 right) => left.CompareTo(right) <= 0;

    public static bool operator >(AsciiChar2 left, AsciiChar2 right) => left.CompareTo(right) > 0;

    public static bool operator >=(AsciiChar2 left, AsciiChar2 right) => left.CompareTo(right) >= 0;
}
