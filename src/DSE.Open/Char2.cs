// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of two unicode characters.
/// </summary>
public readonly struct Char2
    : IEquatable<Char2>,
      ISpanFormattable,
      ISpanParsable<Char2>,
      ISpanFormattableCharCountProvider,
      IRepeatableHash64
{
    private const int CharCount = 2;

    private readonly InlineArray2<char> _chars;

    /// <summary>Initializes a new <see cref="Char2"/> from two characters.</summary>
    public Char2(char c0, char c1)
    {
        _chars[0] = c0;
        _chars[1] = c1;
    }

    /// <summary>Initializes a new <see cref="Char2"/> from a tuple of two characters.</summary>
    public Char2((char c0, char c1) value) : this(value.c0, value.c1)
    {
    }

    /// <summary>Initializes a new <see cref="Char2"/> from a span of exactly two characters.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="span"/> is not exactly 2 elements long.</exception>
    public Char2(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _chars = Unsafe.As<char, InlineArray2<char>>(ref MemoryMarshal.GetReference(span));
    }

    /// <summary>Deconstructs the value into its two component <see cref="char"/> values.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out char c0, out char c1)
    {
        c0 = _chars[0];
        c1 = _chars[1];
    }

    /// <inheritdoc/>
    public bool Equals(Char2 other)
    {
        return _chars[0] == other._chars[0] && _chars[1] == other._chars[1];
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Char2 other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_chars[0], _chars[1]);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return CharCount;
    }

    /// <inheritdoc/>
    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return CharCount;
    }

    /// <summary>Creates a <see cref="Char2"/> from a two-character string.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not exactly 2 characters long.</exception>
    public static Char2 FromString(string value)
    {
        return new(value.AsSpan());
    }

    /// <summary>Creates a <see cref="Char2"/> from a span of exactly two characters.</summary>
    public static Char2 FromSpan(ReadOnlySpan<char> span)
    {
        return new(span);
    }

    /// <summary>Determines whether two <see cref="Char2"/> values are equal.</summary>
    public static bool operator ==(Char2 left, Char2 right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Char2"/> values are not equal.</summary>
    public static bool operator !=(Char2 left, Char2 right)
    {
        return !left.Equals(right);
    }

    /// <summary>Returns the two-character string representation of the value.</summary>
    public static implicit operator string(Char2 value)
    {
        return value.ToString();
    }

    /// <summary>Parses a two-character string into a <see cref="Char2"/>.</summary>
    public static explicit operator Char2(string value)
    {
        return FromString(value);
    }

    /// <summary>Returns a new value with both characters uppercased using the current culture.</summary>
    public Char2 ToUpper()
    {
        return ToUpper(CultureInfo.CurrentCulture);
    }

    /// <summary>Returns a new value with both characters uppercased using the specified culture.</summary>
    public Char2 ToUpper(CultureInfo cultureInfo)
    {
        return new(char.ToUpper(_chars[0], cultureInfo), char.ToUpper(_chars[1], cultureInfo));
    }

    /// <summary>Returns a new value with both characters uppercased using the invariant culture.</summary>
    public Char2 ToUpperInvariant()
    {
        return ToUpper(CultureInfo.InvariantCulture);
    }

    /// <summary>Returns a new value with both characters lowercased using the current culture.</summary>
    public Char2 ToLower()
    {
        return ToLower(CultureInfo.CurrentCulture);
    }

    /// <summary>Returns a new value with both characters lowercased using the specified culture.</summary>
    public Char2 ToLower(CultureInfo cultureInfo)
    {
        return new(char.ToLower(_chars[0], cultureInfo), char.ToLower(_chars[1], cultureInfo));
    }

    /// <summary>Returns a new value with both characters lowercased using the invariant culture.</summary>
    public Char2 ToLowerInvariant()
    {
        return ToLower(CultureInfo.InvariantCulture);
    }

    // TODO: support format provider?

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= CharCount)
        {
            destination[0] = _chars[0];
            destination[1] = _chars[1];
            charsWritten = CharCount;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    // TODO: support format provider?
    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(CharCount, this, (span, value) =>
        {
            span[0] = value._chars[0];
            span[1] = value._chars[1];
        });
    }

    /// <inheritdoc/>
    public static Char2 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(Char2)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Char2 result)
    {
        if (s.Length == CharCount)
        {
            result = new(s);
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc/>
    public static Char2 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Char2 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    internal ReadOnlySpan<char> AsSpan()
    {
        return MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<InlineArray2<char>, char>(ref Unsafe.AsRef(in _chars)),
            CharCount);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(AsSpan());
    }
}
