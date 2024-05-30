// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
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
      ISpanParsable<CharSequence>,
      ISpanFormatableCharCountProvider
{
    private readonly ReadOnlyMemory<char> _value;

    /// <summary>
    /// Initialises a new <see cref="CharSequence"/> value, referencing the characters
    /// in the specified string.
    /// </summary>
    /// <param name="value"></param>
    /// <remarks>The characters are not copied, as we trust <see cref="string"/> to
    /// preserve immutability.</remarks>
    public CharSequence(string value) : this(value.AsMemory(), false)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="CharSequence"/> value, using the characters
    /// in the specified region of memory.
    /// </summary>
    /// <param name="value"></param>
    /// <remarks>The characters are copied, to ensure immutability. To avoid this,
    /// use <see cref="CreateUnsafe(ReadOnlyMemory{char})"/>.</remarks>
    public CharSequence(ReadOnlyMemory<char> value) : this(value, true)
    {
    }

    private CharSequence(ReadOnlyMemory<char> value, bool copy)
    {
        _value = copy ? (ReadOnlyMemory<char>)value.ToArray() : value;
    }

    /// <summary>
    /// Returns a <see cref="CharSequence"/> that points to the same memory
    /// as <paramref name="value"/>. The caller is responsible for ensuring that the
    /// memory is not modified while the <see cref="CharSequence"/> is in use.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static CharSequence CreateUnsafe(ReadOnlyMemory<char> value)
    {
        return new(value, false);
    }

    public char this[int i] => _value.Span[i];

    public CharSequence Slice(int start, int length)
    {
        return new(_value.Slice(start, length));
    }

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public ReadOnlyMemory<char> AsMemory()
    {
        return _value;
    }

    public ReadOnlySpan<char> AsSpan()
    {
        return _value.Span;
    }

    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.Length;
    }

    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _value.Length;
    }

    public static CharSequence Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static CharSequence Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        Expect.Unreachable();
        return default; // unreachable
    }

    public static CharSequence Parse(string s)
    {
        return Parse(s, default);
    }

    public static CharSequence ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static CharSequence Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);

        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        Expect.Unreachable();
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out CharSequence result)
    {
        return TryParse(s, default, out result);
    }

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
    {
        return TryParse(s, default, out result);
    }

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

        result = new(s);
        return true;
    }

    public int CompareTo(CharSequence other)
    {
        return _value.Span.SequenceCompareTo(other._value.Span);
    }

    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return EndsWith(value, StringComparison.Ordinal);
    }

    public bool EndsWith(ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return _value.Span.EndsWith(value, comparisonType);
    }

    public bool EndsWith(CharSequence value)
    {
        return EndsWith(value.AsSpan(), StringComparison.Ordinal);
    }

    public bool EndsWith(CharSequence value, StringComparison comparisonType)
    {
        return _value.Span.EndsWith(value.AsSpan(), comparisonType);
    }

    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return StartsWith(value, StringComparison.Ordinal);
    }

    public bool StartsWith(ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return _value.Span.StartsWith(value, comparisonType);
    }

    public bool StartsWith(CharSequence value)
    {
        return StartsWith(value.AsSpan(), StringComparison.Ordinal);
    }

    public bool StartsWith(CharSequence value, StringComparison comparisonType)
    {
        return _value.Span.StartsWith(value.AsSpan(), comparisonType);
    }

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="CharSequence"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(char value)
    {
        return _value.Span.Contains(value);
    }

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="CharSequence"/>
    /// using <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value)
    {
        return Contains(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="CharSequence"/>
    /// using the specified <paramref name="comparisonType"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return _value.Span.Contains(value, comparisonType);
    }

    public bool Equals(string other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(string other, StringComparison comparisonType)
    {
        return Equals(other.AsSpan(), comparisonType);
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType)
    {
        return _value.Span.Equals(other, comparisonType);
    }

    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(ReadOnlyMemory<char> other, StringComparison comparisonType)
    {
        return Equals(other.Span, comparisonType);
    }

    public bool Equals(CharSequence other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(CharSequence other, StringComparison comparisonType)
    {
        return Equals(other._value, comparisonType);
    }

    public override bool Equals(object? obj)
    {
        return obj is CharSequence other && Equals(other, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(_value.Span, StringComparison.Ordinal);
    }

    public int IndexOf(char c)
    {
        return _value.Span.IndexOf(c);
    }

    public int LastIndexOf(char c)
    {
        return _value.Span.LastIndexOf(c);
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return new(_value.Span);
    }

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

    public static bool operator ==(CharSequence left, CharSequence right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    public static bool operator !=(CharSequence left, CharSequence right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static implicit operator CharSequence(string value)
    {
        return Parse(value, null);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Converts the <paramref name="asciiString"/> to a <see cref="CharSequence"/>.
    /// </summary>
    /// <param name="asciiString"></param>
    /// <returns>A new <see cref="CharSequence"/> instance.</returns>
    public static CharSequence FromAsciiString(AsciiString asciiString)
    {
        return new(asciiString.ToCharArray());
    }
}
