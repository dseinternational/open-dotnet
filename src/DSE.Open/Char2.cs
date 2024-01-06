// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of two unicode characters.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct Char2
    : IEquatable<Char2>,
      ISpanFormattable,
      ISpanParsable<Char2>,
      ISpanFormatableCharCountProvider
{
    private const int CharCount = 2;

    private readonly char _c0;
    private readonly char _c1;

    public Char2(char c0, char c1)
    {
        _c0 = c0;
        _c1 = c1;
    }

    public Char2((char c0, char c1) value) : this(value.c0, value.c1)
    {
    }

    public Char2(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = span[0];
        _c1 = span[1];
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out char c0, out char c1)
    {
        c0 = _c0;
        c1 = _c1;
    }

    public bool Equals(Char2 other)
    {
        return _c0 == other._c0 && _c1 == other._c1;
    }

    public override bool Equals(object? obj)
    {
        return obj is Char2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_c0, _c1);
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

    public static Char2 FromString(string value)
    {
        return new Char2(value.AsSpan());
    }

    public static Char2 FromSpan(ReadOnlySpan<char> span)
    {
        return new Char2(span);
    }

    public static bool operator ==(Char2 left, Char2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Char2 left, Char2 right)
    {
        return !left.Equals(right);
    }

    public static implicit operator string(Char2 value)
    {
        return value.ToString();
    }

    public static explicit operator Char2(string value)
    {
        return FromString(value);
    }

    public Char2 ToUpper()
    {
        return ToUpper(CultureInfo.CurrentCulture);
    }

    public Char2 ToUpper(CultureInfo cultureInfo)
    {
        return new Char2(char.ToUpper(_c0, cultureInfo), char.ToUpper(_c1, cultureInfo));
    }

    public Char2 ToUpperInvariant()
    {
        return ToUpper(CultureInfo.InvariantCulture);
    }

    public Char2 ToLower()
    {
        return ToLower(CultureInfo.CurrentCulture);
    }

    public Char2 ToLower(CultureInfo cultureInfo)
    {
        return new Char2(char.ToLower(_c0, cultureInfo), char.ToLower(_c1, cultureInfo));
    }

    public Char2 ToLowerInvariant()
    {
        return ToLower(CultureInfo.InvariantCulture);
    }

    // TODO: support format provider?

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= CharCount)
        {
            destination[0] = _c0;
            destination[1] = _c1;
            charsWritten = CharCount;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    // TODO: support format provider?
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(CharCount, this, (span, value) =>
        {
            span[0] = value._c0;
            span[1] = value._c1;
        });
    }

    public static Char2 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(Char2)}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Char2 result)
    {
        if (s.Length == CharCount)
        {
            result = new Char2(s);
            return true;
        }

        result = default;
        return false;
    }

    public static Char2 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Char2 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }
}
