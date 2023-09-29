// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
/// </summary>
[JsonConverter(typeof(JsonStringCharSequenceConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly struct CharSequence
    : IEquatable<CharSequence>,
      IEquatable<ReadOnlyMemory<char>>,
      IEqualityOperators<CharSequence, CharSequence, bool>,
      IComparable<CharSequence>,
      ISpanFormattable,
      ISpanParsable<CharSequence>
{
    private readonly ReadOnlyMemory<char> _value;

    public CharSequence(string value) : this(value.AsMemory()) { }

    public CharSequence(ReadOnlyMemory<char> value)
    {
        _value = value;
    }

    public char this[int i] => _value.Span[i];

    public CharSequence Slice(int start, int length) => new(_value.Slice(start, length));

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public ReadOnlyMemory<char> AsMemory() => _value;

    public ReadOnlySpan<char> AsSpan() => _value.Span;

    public ReadOnlySpan<char> Span => _value.Span;

    public static CharSequence Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static CharSequence Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowInvalidOperationException(); // this should not be possible
        return default; // unreachable
    }

    public static CharSequence Parse(string s)
        => Parse(s, default);

    public static CharSequence Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out CharSequence result)
        => TryParse(s, default, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out CharSequence result)
    {
        result = new(s.ToArray());
        return true;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out CharSequence result)
        => TryParse(s, default, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out CharSequence result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public int CompareTo(CharSequence other) => _value.Span.SequenceCompareTo(other._value.Span);

    public bool EndsWith(ReadOnlySpan<char> value) => EndsWith(value, StringComparison.Ordinal);

    public bool EndsWith(ReadOnlySpan<char> value, StringComparison comparisonType) => _value.Span.EndsWith(value, comparisonType);

    public bool EndsWith(CharSequence value) => EndsWith(value.Span, StringComparison.Ordinal);

    public bool EndsWith(CharSequence value, StringComparison comparisonType) => _value.Span.EndsWith(value.Span, comparisonType);

    public bool StartsWith(ReadOnlySpan<char> value) => StartsWith(value, StringComparison.Ordinal);

    public bool StartsWith(ReadOnlySpan<char> value, StringComparison comparisonType) => _value.Span.StartsWith(value, comparisonType);

    public bool StartsWith(CharSequence value) => StartsWith(value.Span, StringComparison.Ordinal);

    public bool StartsWith(CharSequence value, StringComparison comparisonType) => _value.Span.StartsWith(value.Span, comparisonType);

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="CharSequence"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(char value) => _value.Span.Contains(value);

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="CharSequence"/> using <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value) => Contains(value, StringComparison.Ordinal);

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="CharSequence"/> using the specified <paramref name="comparisonType"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value, StringComparison comparisonType) => _value.Span.Contains(value, comparisonType);

    public bool Equals(string other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(string other, StringComparison comparisonType) => Equals(other.AsSpan(), comparisonType);

    public bool Equals(ReadOnlySpan<char> other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType) => _value.Span.Equals(other, comparisonType);

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(ReadOnlyMemory<char> other, StringComparison comparisonType) => Equals(other.Span, comparisonType);

    public bool Equals(CharSequence other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(CharSequence other, StringComparison comparisonType) => Equals(other._value, comparisonType);

    public override bool Equals(object? obj) => obj is CharSequence other && Equals(other, StringComparison.Ordinal);

    public override int GetHashCode() => string.GetHashCode(_value.Span, StringComparison.Ordinal);

    public int IndexOf(char c) => _value.Span.IndexOf(c);

    public int LastIndexOf(char c) => _value.Span.LastIndexOf(c);

    public override string ToString() => ToString(default, default);

    public string ToString(string? format, IFormatProvider? formatProvider) => new(_value.Span);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= _value.Length)
        {
            _value.Span.CopyTo(destination);
            charsWritten = _value.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static bool operator ==(CharSequence left, CharSequence right) => left.Equals(right, StringComparison.Ordinal);

    public static bool operator !=(CharSequence left, CharSequence right) => !(left == right);

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static implicit operator CharSequence(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(CharSequence left, CharSequence right) => left.CompareTo(right) < 0;

    public static bool operator <=(CharSequence left, CharSequence right) => left.CompareTo(right) <= 0;

    public static bool operator >(CharSequence left, CharSequence right) => left.CompareTo(right) > 0;

    public static bool operator >=(CharSequence left, CharSequence right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Converts the <paramref name="asciiString"/> to a <see cref="CharSequence"/>.
    /// </summary>
    /// <param name="asciiString"></param>
    /// <returns>A new <see cref="CharSequence"/> instance.</returns>
    public static CharSequence FromAsciiString(AsciiString asciiString) => new(asciiString.ToCharArray());
}
