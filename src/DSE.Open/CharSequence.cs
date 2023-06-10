// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
/// </summary>
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

    public CharSequence(ReadOnlySpan<char> value) : this((ReadOnlyMemory<char>)value.ToArray()) { }

    public CharSequence(ReadOnlyMemory<char> value)
    {
        _value = value;
    }

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public ReadOnlyMemory<char> AsMemory() => _value;

    public ReadOnlySpan<char> AsSpan() => _value.Span;

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
        result = new(s);
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

    public bool Equals(ReadOnlySpan<char> other) => _value.Span.SequenceEqual(other);

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(CharSequence other) => Equals(other._value);

    public override bool Equals(object? obj) => obj is CharSequence other && Equals(other);

    public override int GetHashCode() => string.GetHashCode(_value.Span, StringComparison.Ordinal);

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

    public static bool operator ==(CharSequence left, CharSequence right) => left.Equals(right);

    public static bool operator !=(CharSequence left, CharSequence right) => !(left == right);

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static implicit operator CharSequence(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(CharSequence left, CharSequence right) => left.CompareTo(right) < 0;

    public static bool operator <=(CharSequence left, CharSequence right) => left.CompareTo(right) <= 0;

    public static bool operator >(CharSequence left, CharSequence right) => left.CompareTo(right) > 0;

    public static bool operator >=(CharSequence left, CharSequence right) => left.CompareTo(right) >= 0;
}
