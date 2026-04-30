// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Hashing;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
/// </summary>
[JsonConverter(typeof(JsonStringCharSequenceConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly struct CharSequence
    : IEquatable<CharSequence>,
      IEquatable<ReadOnlyMemory<char>>,
      IEqualityOperators<CharSequence, CharSequence, bool>,
      IComparable<CharSequence>,
      ISpanFormattable,
      ISpanParsable<CharSequence>,
      ISpanFormattableCharCountProvider,
      IRepeatableHash64
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

    /// <summary>Gets the character at the specified zero-based position.</summary>
    public char this[int i] => _value.Span[i];

    /// <summary>Returns a sub-section of the current sequence as a new <see cref="CharSequence"/>.</summary>
    public CharSequence Slice(int start, int length)
    {
        return new(_value.Slice(start, length));
    }

    /// <summary>Gets a value indicating whether the sequence is empty.</summary>
    public bool IsEmpty => _value.IsEmpty;

    /// <summary>Gets the number of characters in the sequence.</summary>
    public int Length => _value.Length;

    /// <summary>Returns the underlying <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> values.</summary>
    public ReadOnlyMemory<char> AsMemory()
    {
        return _value;
    }

    /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> of the underlying characters.</summary>
    public ReadOnlySpan<char> Span => _value.Span;

    /// <inheritdoc/>
    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.Length;
    }

    /// <inheritdoc/>
    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _value.Length;
    }

    /// <summary>Parses the specified character span as a <see cref="CharSequence"/>.</summary>
    public static CharSequence Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static CharSequence Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        Expect.Unreachable();
        return default; // unreachable
    }

    /// <summary>Parses the specified string as a <see cref="CharSequence"/>.</summary>
    public static CharSequence Parse(string s)
    {
        return Parse(s, default);
    }

    /// <summary>Parses the specified string as a <see cref="CharSequence"/> using the invariant culture.</summary>
    public static CharSequence ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static CharSequence Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        Expect.Unreachable();
        return default; // unreachable
    }

    /// <summary>Attempts to parse the specified character span as a <see cref="CharSequence"/>.</summary>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        out CharSequence result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out CharSequence result)
    {
        result = new(s.ToArray());
        return true;
    }

    /// <summary>Attempts to parse the specified string as a <see cref="CharSequence"/>.</summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out CharSequence result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public int CompareTo(CharSequence other)
    {
        return _value.Span.SequenceCompareTo(other._value.Span);
    }

    /// <summary>Determines whether the sequence ends with the specified value using ordinal comparison.</summary>
    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return EndsWith(value, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence ends with the specified value using the specified comparison.</summary>
    public bool EndsWith(ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return _value.Span.EndsWith(value, comparisonType);
    }

    /// <summary>Determines whether the sequence ends with the specified character.</summary>
    public bool EndsWith(char value)
    {
        return _value.Span.EndsWith(value);
    }

    /// <summary>Determines whether the sequence ends with the specified value using ordinal comparison.</summary>
    public bool EndsWith(CharSequence value)
    {
        return EndsWith(value.Span, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence ends with the specified value using the specified comparison.</summary>
    public bool EndsWith(CharSequence value, StringComparison comparisonType)
    {
        return _value.Span.EndsWith(value.Span, comparisonType);
    }

    /// <summary>Determines whether the sequence starts with the specified value using ordinal comparison.</summary>
    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return StartsWith(value, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence starts with the specified value using the specified comparison.</summary>
    public bool StartsWith(ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return _value.Span.StartsWith(value, comparisonType);
    }

    /// <summary>Determines whether the sequence starts with the specified value using ordinal comparison.</summary>
    public bool StartsWith(CharSequence value)
    {
        return StartsWith(value.Span, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence starts with the specified value using the specified comparison.</summary>
    public bool StartsWith(CharSequence value, StringComparison comparisonType)
    {
        return _value.Span.StartsWith(value.Span, comparisonType);
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

    /// <summary>Determines whether the sequence equals the specified string using ordinal comparison.</summary>
    public bool Equals(string other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence equals the specified string using the specified comparison.</summary>
    public bool Equals(string other, StringComparison comparisonType)
    {
        return Equals(other.AsSpan(), comparisonType);
    }

    /// <summary>Determines whether the sequence equals the specified character span using ordinal comparison.</summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence equals the specified character span using the specified comparison.</summary>
    public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType)
    {
        return _value.Span.Equals(other, comparisonType);
    }

    /// <inheritdoc/>
    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>Determines whether the sequence equals the specified character memory using the specified comparison.</summary>
    public bool Equals(ReadOnlyMemory<char> other, StringComparison comparisonType)
    {
        return Equals(other.Span, comparisonType);
    }

    /// <inheritdoc/>
    public bool Equals(CharSequence other)
    {
        return Equals(other, StringComparison.Ordinal);
    }

    /// <summary>Determines whether two <see cref="CharSequence"/> values are equal using the specified comparison.</summary>
    public bool Equals(CharSequence other, StringComparison comparisonType)
    {
        return Equals(other._value, comparisonType);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is CharSequence other && Equals(other, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return string.GetHashCode(_value.Span, StringComparison.Ordinal);
    }

    /// <summary>Returns the zero-based index of the first occurrence of the specified character, or -1 if not found.</summary>
    public int IndexOf(char c)
    {
        return _value.Span.IndexOf(c);
    }

    /// <summary>Returns the zero-based index of the last occurrence of the specified character, or -1 if not found.</summary>
    public int LastIndexOf(char c)
    {
        return _value.Span.LastIndexOf(c);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(default, default);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return new(_value.Span);
    }

    /// <inheritdoc/>
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

    /// <summary>Determines whether two <see cref="CharSequence"/> values are equal using ordinal comparison.</summary>
    public static bool operator ==(CharSequence left, CharSequence right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    /// <summary>Determines whether two <see cref="CharSequence"/> values are not equal using ordinal comparison.</summary>
    public static bool operator !=(CharSequence left, CharSequence right)
    {
        return !(left == right);
    }

    /// <summary>Creates a <see cref="CharSequence"/> from the specified string.</summary>
    public static CharSequence FromString(string value)
    {
        return Parse(value, null);
    }

    /// <summary>Creates a <see cref="CharSequence"/> from the specified string.</summary>
    public static implicit operator CharSequence(string value)
    {
        return Parse(value, null);
    }

    /// <summary>Determines whether one <see cref="CharSequence"/> precedes another in ordinal order.</summary>
    public static bool operator <(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one <see cref="CharSequence"/> precedes or equals another in ordinal order.</summary>
    public static bool operator <=(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one <see cref="CharSequence"/> follows another in ordinal order.</summary>
    public static bool operator >(CharSequence left, CharSequence right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one <see cref="CharSequence"/> follows or equals another in ordinal order.</summary>
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

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(this);
    }
}
