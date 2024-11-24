// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of two ASCII bytes.
/// </summary>
[JsonConverter(typeof(JsonStringAsciiCharNConverter<AsciiChar2>))]
[StructLayout(LayoutKind.Sequential)]
public readonly struct AsciiChar2
    : IComparable<AsciiChar2>,
      IEquatable<AsciiChar2>,
      IEqualityOperators<AsciiChar2, AsciiChar2, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiChar2>,
      IConvertibleTo<AsciiChar2, string>,
      ITryConvertibleFrom<AsciiChar2, string>,
      IUtf8SpanSerializable<AsciiChar2>,
      ISpanFormatableCharCountProvider,
      IRepeatableHash64
{
    private const int CharCount = 2;

    public static int MaxSerializedByteLength => 2;

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

    public int CompareToIgnoreCase(AsciiChar2 other)
    {
        var c = AsciiChar.CompareToIgnoreCase(_c0, other._c0);

        return (c != 0) switch
        {
            true => c,
            _ => AsciiChar.CompareToIgnoreCase(_c1, other._c1)
        };
    }

    public bool Equals(AsciiChar2 other)
    {
        return _c0 == other._c0 && _c1 == other._c1;
    }

    public bool EqualsIgnoreCase(AsciiChar2 other)
    {
        return AsciiChar.EqualsIgnoreCase(_c0, other._c0) && AsciiChar.EqualsIgnoreCase(_c1, other._c1);
    }

    public bool Equals(string other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Equals(other.AsSpan());
    }

    public bool EqualsIgnoreCase(string other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return EqualsIgnoreCase(other.AsSpan());
    }

    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other.Span);
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return other.Length == CharCount && other[0] == _c0 && other[1] == _c1;
    }

    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return other.Length == CharCount
            && Ascii.EqualsIgnoreCase([_c0.ToChar(), _c1.ToChar()], other);
    }

    public override bool Equals(object? obj)
    {
        return obj is AsciiChar2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_c0, _c1);
    }

    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return MaxSerializedByteLength;
    }

    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return MaxSerializedByteLength;
    }

    public Char2 ToChar2()
    {
        return new(_c0, _c1);
    }

    public char[] ToCharArray()
    {
        return [_c0, _c1];
    }

    public static AsciiChar2 FromString(string value)
    {
        return new(value.AsSpan());
    }

    public static AsciiChar2 FromSpan(ReadOnlySpan<AsciiChar> span)
    {
        return new(span);
    }

    public static AsciiChar2 FromByteSpan(ReadOnlySpan<byte> span)
    {
        return new(span);
    }

    public static AsciiChar2 FromCharSpan(ReadOnlySpan<char> span)
    {
        return new(span);
    }

    public static bool operator ==(AsciiChar2 left, AsciiChar2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AsciiChar2 left, AsciiChar2 right)
    {
        return !left.Equals(right);
    }

    public static implicit operator string(AsciiChar2 value)
    {
        return value.ToString();
    }

    public static implicit operator Char2(AsciiChar2 value)
    {
        return value.ToChar2();
    }

    public static explicit operator AsciiChar2(string value)
    {
        return FromString(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AsciiChar2(ReadOnlySpan<byte> value)
    {
        return FromByteSpan(value);
    }

    public static explicit operator AsciiChar2(ReadOnlySpan<char> value)
    {
        return FromCharSpan(value);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public AsciiChar2 ToUpper()
    {
        return new(_c0.ToUpper(), _c1.ToUpper());
    }

    public AsciiChar2 ToLower()
    {
        return new(_c0.ToLower(), _c1.ToLower());
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= 2)
        {
            _ = _c0.TryFormat(destination, out _, format, provider);
            _ = _c1.TryFormat(destination[1..], out _, format, provider);
            charsWritten = 2;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    /// Returns a string representation of the value using the specified format and culture-specific format information.
    /// <remarks>
    /// The <paramref name="format"/> can be unspecified or either 'L' or 'U' to convert the value to lower or upper case respectively.
    /// </remarks>
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(CharCount, (this, format, formatProvider), (span, state) =>
        {
            var (value, format, formatProvider) = state;
            var result = value.TryFormat(span, out _, format, formatProvider);
            Debug.Assert(result);
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
        out AsciiChar2 result)
    {
        if (s.Length == CharCount && AsciiChar.IsAscii(s[0]) && AsciiChar.IsAscii(s[1]))
        {
            result = new(s[..2]);
            return true;
        }

        result = default;
        return false;
    }

    public static AsciiChar2 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiChar2 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    static string IConvertibleTo<AsciiChar2, string>.ConvertTo(AsciiChar2 value)
    {
        return value.ToString();
    }

    static bool ITryConvertibleFrom<AsciiChar2, string>.TryFromValue(string value, out AsciiChar2 result)
    {
        return TryParse(value, null, out result);
    }

    public static AsciiChar2 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse value as an {nameof(AsciiChar2)}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out AsciiChar2 result)
    {
        if (utf8Text.Length == MaxSerializedByteLength && AsciiChar.IsAscii(utf8Text[0]) && AsciiChar.IsAscii(utf8Text[1]))
        {
            result = new(new(utf8Text[0]), new(utf8Text[1]));
            return true;
        }

        result = default;
        return false;
    }

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (utf8Destination.Length >= MaxSerializedByteLength)
        {
            _ = _c0.TryFormat(utf8Destination, out _, format, provider);
            _ = _c1.TryFormat(utf8Destination[1..], out _, format, provider);
            bytesWritten = MaxSerializedByteLength;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode([_c0, _c1]);
    }

    public static bool operator <(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) >= 0;
    }
}
