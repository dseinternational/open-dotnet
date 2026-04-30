// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of three unicode characters.
/// </summary>
public readonly struct Char3
    : IEquatable<Char3>,
      ISpanFormattable,
      ISpanParsable<Char3>,
      ISpanFormattableCharCountProvider,
      ISpanSerializable<Char3>,
      IRepeatableHash64
{
    private const int CharCount = 3;

    /// <summary>Gets the maximum number of characters used by the serialized form of a <see cref="Char3"/>.</summary>
    public static int MaxSerializedCharLength => CharCount;

    private readonly InlineArray3<char> _chars;

    /// <summary>Initializes a new <see cref="Char3"/> from three characters.</summary>
    public Char3(char c0, char c1, char c2)
    {
        _chars[0] = c0;
        _chars[1] = c1;
        _chars[2] = c2;
    }

    /// <summary>Initializes a new <see cref="Char3"/> from a tuple of three characters.</summary>
    public Char3((char c0, char c1, char c2) value)
        : this(value.c0, value.c1, value.c2)
    {
    }

    /// <summary>Initializes a new <see cref="Char3"/> from a span of exactly three characters.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="span"/> is not exactly 3 elements long.</exception>
    public Char3(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _chars = Unsafe.As<char, InlineArray3<char>>(ref MemoryMarshal.GetReference(span));
    }

    /// <summary>Deconstructs the value into its three component <see cref="char"/> values.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out char c0, out char c1, out char c2)
    {
        c0 = _chars[0];
        c1 = _chars[1];
        c2 = _chars[2];
    }

    /// <inheritdoc/>
    public bool Equals(Char3 other)
    {
        return _chars[0] == other._chars[0]
               && _chars[1] == other._chars[1]
               && _chars[2] == other._chars[2];
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Char3 other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_chars[0], _chars[1], _chars[2]);
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

    /// <summary>Creates a <see cref="Char3"/> from a three-character string.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not exactly 3 characters long.</exception>
    public static Char3 FromString(string value)
    {
        return new(value.AsSpan());
    }

    /// <summary>Creates a <see cref="Char3"/> from a span of exactly three characters.</summary>
    public static Char3 FromSpan(ReadOnlySpan<char> span)
    {
        return new(span);
    }

    /// <summary>Determines whether two <see cref="Char3"/> values are equal.</summary>
    public static bool operator ==(Char3 left, Char3 right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Char3"/> values are not equal.</summary>
    public static bool operator !=(Char3 left, Char3 right)
    {
        return !left.Equals(right);
    }

    /// <summary>Returns the three-character string representation of the value.</summary>
    public static implicit operator string(Char3 value)
    {
        return value.ToString();
    }

    /// <summary>Parses a three-character string into a <see cref="Char3"/>.</summary>
    public static explicit operator Char3(string value)
    {
        return FromString(value);
    }

    /// <summary>Returns a new value with all three characters uppercased using the current culture.</summary>
    public Char3 ToUpper()
    {
        return ToUpper(CultureInfo.CurrentCulture);
    }

    /// <summary>Returns a new value with all three characters uppercased using the specified culture.</summary>
    public Char3 ToUpper(CultureInfo cultureInfo)
    {
        return new(
            char.ToUpper(_chars[0], cultureInfo),
            char.ToUpper(_chars[1], cultureInfo),
            char.ToUpper(_chars[2], cultureInfo));
    }

    /// <summary>Returns a new value with all three characters uppercased using the invariant culture.</summary>
    public Char3 ToUpperInvariant()
    {
        return ToUpper(CultureInfo.InvariantCulture);
    }

    /// <summary>Returns a new value with all three characters lowercased using the current culture.</summary>
    public Char3 ToLower()
    {
        return ToLower(CultureInfo.CurrentCulture);
    }

    /// <summary>Returns a new value with all three characters lowercased using the specified culture.</summary>
    public Char3 ToLower(CultureInfo cultureInfo)
    {
        return new(
            char.ToLower(_chars[0], cultureInfo),
            char.ToLower(_chars[1], cultureInfo),
            char.ToLower(_chars[2], cultureInfo));
    }

    /// <summary>Returns a new value with all three characters lowercased using the invariant culture.</summary>
    public Char3 ToLowerInvariant()
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
            destination[2] = _chars[2];
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
            span[2] = value._chars[2];
        });
    }

    /// <inheritdoc/>
    public static Char3 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(Char3)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Char3 result)
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
    public static Char3 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Char3 result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    internal ReadOnlySpan<char> AsSpan()
    {
        return MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<InlineArray3<char>, char>(ref Unsafe.AsRef(in _chars)),
            CharCount);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(AsSpan());
    }
}
