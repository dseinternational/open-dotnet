// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of three unicode characters.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct Char3
    : IEquatable<Char3>,
      ISpanFormattable,
      ISpanParsable<Char3>
{
    private const int CharCount = 3;

    private readonly char _c0;
    private readonly char _c1;
    private readonly char _c2;

    public Char3(char c0, char c1, char c2)
    {
        _c0 = c0;
        _c1 = c1;
        _c2 = c2;
    }

    public Char3((char c0, char c1, char c2) value)
        : this(value.c0, value.c1, value.c2)
    {
    }

    public Char3(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _c0 = span[0];
        _c1 = span[1];
        _c2 = span[2];
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out char c0, out char c1, out char c2)
    {
        c0 = _c0;
        c1 = _c1;
        c2 = _c2;
    }

    public bool Equals(Char3 other)
        => _c0 == other._c0
            && _c1 == other._c1
            && _c2 == other._c2;

    public override bool Equals(object? obj) => obj is Char3 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_c0, _c1, _c2);

    public override string ToString() => ToString(null, null);

    public static Char3 FromString(string value) => new(value.AsSpan());

    public static Char3 FromSpan(ReadOnlySpan<char> span) => new(span);

    public static bool operator ==(Char3 left, Char3 right) => left.Equals(right);

    public static bool operator !=(Char3 left, Char3 right) => !left.Equals(right);

    public static implicit operator string(Char3 value) => value.ToString();

    public static explicit operator Char3(string value) => FromString(value);

    public Char3 ToUpper()
        => new(
            char.ToUpper(_c0),
            char.ToUpper(_c1),
            char.ToUpper(_c2));

    public Char3 ToUpper(CultureInfo cultureInfo)
        => new(
            char.ToUpper(_c0, cultureInfo),
            char.ToUpper(_c1, cultureInfo),
            char.ToUpper(_c2, cultureInfo));

    public Char3 ToUpperInvariant() => ToUpper(CultureInfo.InvariantCulture);

    public Char3 ToLower()
        => new(
            char.ToLower(_c0),
            char.ToLower(_c1),
            char.ToLower(_c2));

    public Char3 ToLower(CultureInfo cultureInfo)
        => new(
            char.ToLower(_c0, cultureInfo),
            char.ToLower(_c1, cultureInfo),
            char.ToLower(_c2, cultureInfo));

    public Char3 ToLowerInvariant() => ToLower(CultureInfo.InvariantCulture);

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
            destination[2] = _c2;
            charsWritten = CharCount;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    // TODO: support format provider?

    public string ToString(string? format, IFormatProvider? formatProvider)
        => string.Create(CharCount, this, (span, value) =>
        {
            span[0] = value._c0;
            span[1] = value._c1;
            span[2] = value._c2;
        });

    public static Char3 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(Char3)}");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Char3 result)
    {
        if (s.Length == CharCount)
        {
            result = new Char3(s);
            return true;
        }

        result = default;
        return false;
    }

    public static Char3 Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Char3 result)
        => TryParse(s.AsSpan(), provider, out result);
}
