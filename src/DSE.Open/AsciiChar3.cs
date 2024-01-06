// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of three ASCII bytes.
/// </summary>
[JsonConverter(typeof(JsonStringAsciiCharNConverter<AsciiChar3>))]
[StructLayout(LayoutKind.Auto)]
public readonly struct AsciiChar3
    : IComparable<AsciiChar3>,
      IEquatable<AsciiChar3>,
      IEqualityOperators<AsciiChar3, AsciiChar3, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiChar3>,
      IConvertibleTo<AsciiChar3, string>,
      ITryConvertibleFrom<AsciiChar3, string>,
      IUtf8SpanSerializable<AsciiChar3>,
      ISpanFormatableCharCountProvider
{
    private const int CharCount = 3;
    public static int MaxSerializedByteLength => 3;

    // internal for AsciiChar3Comparer
    internal readonly AsciiChar _c0;
    internal readonly AsciiChar _c1;
    internal readonly AsciiChar _c2;

    public AsciiChar3(AsciiChar c0, AsciiChar c1, AsciiChar c2)
    {
        _c0 = c0;
        _c1 = c1;
        _c2 = c2;
    }

    public AsciiChar3(char c0, char c1, char c2)
    {
        _c0 = (AsciiChar)c0;
        _c1 = (AsciiChar)c1;
        _c2 = (AsciiChar)c2;
    }

    public AsciiChar3((AsciiChar c0, AsciiChar c1, AsciiChar c2) value) : this(value.c0, value.c1, value.c2)
    {
    }

    public AsciiChar3((char c0, char c1, char c2) value) : this(value.c0, value.c1, value.c2)
    {
    }

    public AsciiChar3(ReadOnlySpan<AsciiChar> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = span[0];
        _c1 = span[1];
        _c2 = span[2];
    }

    public AsciiChar3(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = (AsciiChar)span[0];
        _c1 = (AsciiChar)span[1];
        _c2 = (AsciiChar)span[2];
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out AsciiChar c0, out AsciiChar c1, out AsciiChar c2)
    {
        c0 = _c0;
        c1 = _c1;
        c2 = _c2;
    }

    public bool Equals(AsciiChar3 other)
    {
        return _c0 == other._c0 && _c1 == other._c1 && _c2 == other._c2;
    }

    public bool Equals(string other)
    {
        return Equals(other.AsSpan());
    }

    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other.Span);
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return other.Length == CharCount && other[0] == _c0 && other[1] == _c1 && other[2] == _c2;
    }

    public override bool Equals(object? obj)
    {
        return obj is AsciiChar3 other && Equals(other);
    }

    public bool EqualsCaseInsensitive(AsciiChar3 other)
    {
        return AsciiChar.EqualsCaseInsensitive(_c0, other._c0)
               && AsciiChar.EqualsCaseInsensitive(_c1, other._c1)
               && AsciiChar.EqualsCaseInsensitive(_c2, other._c2);
    }

    public int CompareToCaseInsensitive(AsciiChar3 other)
    {
        var c = AsciiChar.CompareToCaseInsensitive(_c0, other._c0);

        if (c != 0)
        {
            return c;
        }

        c = AsciiChar.CompareToCaseInsensitive(_c1, other._c1);

        return c != 0 ? c : AsciiChar.CompareToCaseInsensitive(_c2, other._c2);
    }

    public int CompareTo(AsciiChar3 other)
    {
        var c = _c0.CompareTo(other._c0);

        if (c != 0)
        {
            return c;
        }

        c = _c1.CompareTo(other._c1);

        return c != 0 ? c : _c2.CompareTo(other._c2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_c0, _c1, _c2);
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return MaxSerializedByteLength;
    }

    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return MaxSerializedByteLength;
    }

    public Char3 ToChar3()
    {
        return new Char3((char)_c0, (char)_c1, (char)_c2);
    }

    public char[] ToCharArray()
    {
        return [(char)_c0, _c1, _c2];
    }

    public static AsciiChar3 FromString(string value)
    {
        return new AsciiChar3(value.AsSpan());
    }

    public static AsciiChar3 FromSpan(ReadOnlySpan<AsciiChar> span)
    {
        return new AsciiChar3(span);
    }

    public static bool operator ==(AsciiChar3 left, AsciiChar3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AsciiChar3 left, AsciiChar3 right)
    {
        return !left.Equals(right);
    }

    public static implicit operator string(AsciiChar3 value)
    {
        return value.ToString();
    }

    public static implicit operator Char3(AsciiChar3 value)
    {
        return value.ToChar3();
    }

    public static explicit operator AsciiChar3(string value)
    {
        return FromString(value);
    }

    public AsciiChar3 ToUpper()
    {
        return new AsciiChar3(_c0.ToUpper(), _c1.ToUpper(), _c2.ToUpper());
    }

    public AsciiChar3 ToLower()
    {
        return new AsciiChar3(_c0.ToLower(), _c1.ToLower(), _c2.ToLower());
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (destination.Length >= CharCount)
        {
            _c0.TryFormat(destination, out _, format, provider);
            _c1.TryFormat(destination[1..], out _, format, provider);
            _c2.TryFormat(destination[2..], out _, format, provider);
            charsWritten = CharCount;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(CharCount, (this, format, formatProvider), (span, state) =>
        {
            var (value, format, formatProvider) = state;
            value.TryFormat(span, out _, format, formatProvider);
        });
    }

    public string ToStringLower()
    {
        return ToString("L", null);
    }

    public string ToStringUpper()
    {
        return ToString("U", null);
    }

    public static AsciiChar3 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(AsciiChar3)}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiChar3 result)
    {
        if (s.Length == CharCount && AsciiChar.IsAscii(s[0])
                                  && AsciiChar.IsAscii(s[1]) && AsciiChar.IsAscii(s[2]))
        {
            result = new AsciiChar3(s);
            return true;
        }

        result = default;
        return false;
    }

    public static AsciiChar3 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out AsciiChar3 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    static string IConvertibleTo<AsciiChar3, string>.ConvertTo(AsciiChar3 value)
    {
        return value.ToString();
    }

    static bool ITryConvertibleFrom<AsciiChar3, string>.TryFromValue(string value, out AsciiChar3 result)
    {
        return TryParse(value, null, out result);
    }

    public static AsciiChar3 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{utf8Text.ToArray()}' as a {nameof(AsciiChar3)}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out AsciiChar3 result)
    {
        if (utf8Text.Length != CharCount
            || !AsciiChar.IsAscii(utf8Text[0])
            || !AsciiChar.IsAscii(utf8Text[1])
            || !AsciiChar.IsAscii(utf8Text[2]))
        {
            result = default;
            return false;
        }

        result = new AsciiChar3((AsciiChar)utf8Text[0], (AsciiChar)utf8Text[1], (AsciiChar)utf8Text[2]);
        return true;
    }

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (utf8Destination.Length >= CharCount)
        {
            _c0.TryFormat(utf8Destination, out _, format, provider);
            _c1.TryFormat(utf8Destination[1..], out _, format, provider);
            _c2.TryFormat(utf8Destination[2..], out _, format, provider);

            bytesWritten = CharCount;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    public static bool operator <(AsciiChar3 left, AsciiChar3 right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(AsciiChar3 left, AsciiChar3 right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(AsciiChar3 left, AsciiChar3 right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(AsciiChar3 left, AsciiChar3 right)
    {
        return left.CompareTo(right) >= 0;
    }
}
