// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
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
public readonly struct AsciiChar2
    : IComparable<AsciiChar2>,
      IEquatable<AsciiChar2>,
      IEqualityOperators<AsciiChar2, AsciiChar2, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiChar2>,
      IConvertibleTo<AsciiChar2, string>,
      ITryConvertibleFrom<AsciiChar2, string>,
      IUtf8SpanSerializable<AsciiChar2>,
      ISpanFormattableCharCountProvider,
      IRepeatableHash64
{
    private const int CharCount = 2;

    /// <summary>Gets the number of bytes used by the UTF-8 serialized form of an <see cref="AsciiChar2"/>.</summary>
    public static int MaxSerializedByteLength => 2;

    // internal for AsciiChar2Comparer
    internal readonly InlineArray2<AsciiChar> _chars;

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from two ASCII characters.</summary>
    public AsciiChar2(AsciiChar c0, AsciiChar c1)
    {
        _chars[0] = c0;
        _chars[1] = c1;
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from two ASCII bytes.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if any value is not an ASCII character.</exception>
    public AsciiChar2(byte c0, byte c1) : this((AsciiChar)c0, (AsciiChar)c1)
    {
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from two characters in the ASCII range.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if any value is not an ASCII character.</exception>
    public AsciiChar2(char c0, char c1) : this((AsciiChar)c0, (AsciiChar)c1)
    {
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from a tuple of two ASCII characters.</summary>
    public AsciiChar2((AsciiChar c0, AsciiChar c1) value) : this(value.c0, value.c1)
    {
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from a tuple of two ASCII bytes.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if any value is not an ASCII character.</exception>
    public AsciiChar2((byte c0, byte c1) value) : this(value.c0, value.c1)
    {
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from a tuple of two characters in the ASCII range.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if any value is not an ASCII character.</exception>
    public AsciiChar2((char c0, char c1) value) : this(value.c0, value.c1)
    {
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from a span of exactly two ASCII characters.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="span"/> is not exactly 2 elements long.</exception>
    public AsciiChar2(ReadOnlySpan<AsciiChar> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        _chars = Unsafe.As<AsciiChar, InlineArray2<AsciiChar>>(ref MemoryMarshal.GetReference(span));
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from a span of exactly two ASCII bytes.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="span"/> is not exactly 2 ASCII bytes.</exception>
    public AsciiChar2(ReadOnlySpan<byte> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        if (!Ascii.IsValid(span))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        // AsciiChar is a single-byte struct with identical layout to byte,
        // and all bytes have been validated as ASCII above.
        _chars = Unsafe.As<byte, InlineArray2<AsciiChar>>(ref MemoryMarshal.GetReference(span));
    }

    /// <summary>Initializes a new <see cref="AsciiChar2"/> from a span of exactly two characters in the ASCII range.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="span"/> is not exactly 2 elements or contains non-ASCII characters.</exception>
    public AsciiChar2(ReadOnlySpan<char> span)
    {
        if (span.Length != CharCount)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(span));
        }

        // chars require per-element casting (narrowing), so delegate to the element constructor.
        this = new((AsciiChar)span[0], (AsciiChar)span[1]);
    }

    /// <summary>Deconstructs the value into its two component <see cref="AsciiChar"/> values.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out AsciiChar c0, out AsciiChar c1)
    {
        c0 = _chars[0];
        c1 = _chars[1];
    }

    /// <inheritdoc/>
    public int CompareTo(AsciiChar2 other)
    {
        var c = _chars[0].CompareTo(other._chars[0]);
        return c != 0 ? c : _chars[1].CompareTo(other._chars[1]);
    }

    /// <summary>Compares this instance to another <see cref="AsciiChar2"/> ignoring ASCII case.</summary>
    public int CompareToIgnoreCase(AsciiChar2 other)
    {
        var c = AsciiChar.CompareToIgnoreCase(_chars[0], other._chars[0]);

        return (c != 0) switch
        {
            true => c,
            _ => AsciiChar.CompareToIgnoreCase(_chars[1], other._chars[1])
        };
    }

    /// <inheritdoc/>
    public bool Equals(AsciiChar2 other)
    {
        return _chars[0] == other._chars[0] && _chars[1] == other._chars[1];
    }

    /// <summary>Determines whether two <see cref="AsciiChar2"/> values are equal ignoring ASCII case.</summary>
    public bool EqualsIgnoreCase(AsciiChar2 other)
    {
        return AsciiChar.EqualsIgnoreCase(_chars[0], other._chars[0]) && AsciiChar.EqualsIgnoreCase(_chars[1], other._chars[1]);
    }

    /// <summary>Determines whether the value equals the specified string (compared as ASCII).</summary>
    public bool Equals(string other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Equals(other.AsSpan());
    }

    /// <summary>Determines whether the value equals the specified string ignoring ASCII case.</summary>
    public bool EqualsIgnoreCase(string other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return EqualsIgnoreCase(other.AsSpan());
    }

    /// <summary>Determines whether the value equals the specified character memory.</summary>
    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other.Span);
    }

    /// <summary>Determines whether the value equals the specified character span.</summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return other.Length == CharCount && other[0] == _chars[0] && other[1] == _chars[1];
    }

    /// <summary>Determines whether the value equals the specified character span ignoring ASCII case.</summary>
    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return other.Length == CharCount
            && Ascii.EqualsIgnoreCase([_chars[0].ToChar(), _chars[1].ToChar()], other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is AsciiChar2 other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_chars[0], _chars[1]);
    }

    /// <inheritdoc/>
    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return MaxSerializedByteLength;
    }

    /// <inheritdoc/>
    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return MaxSerializedByteLength;
    }

    /// <summary>Converts this value to a <see cref="Char2"/>.</summary>
    public Char2 ToChar2()
    {
        return new(_chars[0], _chars[1]);
    }

    /// <summary>Returns a new array containing the two characters as UTF-16 <see cref="char"/> values.</summary>
    public char[] ToCharArray()
    {
        return [_chars[0], _chars[1]];
    }

    /// <summary>Creates an <see cref="AsciiChar2"/> from a two-character string.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not exactly 2 ASCII characters.</exception>
    public static AsciiChar2 FromString(string value)
    {
        return new(value.AsSpan());
    }

    /// <summary>Creates an <see cref="AsciiChar2"/> from a span of exactly two ASCII characters.</summary>
    public static AsciiChar2 FromSpan(ReadOnlySpan<AsciiChar> span)
    {
        return new(span);
    }

    /// <summary>Creates an <see cref="AsciiChar2"/> from a span of exactly two ASCII bytes.</summary>
    public static AsciiChar2 FromByteSpan(ReadOnlySpan<byte> span)
    {
        return new(span);
    }

    /// <summary>Creates an <see cref="AsciiChar2"/> from a span of exactly two characters in the ASCII range.</summary>
    public static AsciiChar2 FromCharSpan(ReadOnlySpan<char> span)
    {
        return new(span);
    }

    /// <summary>Determines whether two <see cref="AsciiChar2"/> values are equal.</summary>
    public static bool operator ==(AsciiChar2 left, AsciiChar2 right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="AsciiChar2"/> values are not equal.</summary>
    public static bool operator !=(AsciiChar2 left, AsciiChar2 right)
    {
        return !left.Equals(right);
    }

    /// <summary>Returns the two-character string representation of the value.</summary>
    public static implicit operator string(AsciiChar2 value)
    {
        return value.ToString();
    }

    /// <summary>Converts an <see cref="AsciiChar2"/> to a <see cref="Char2"/>.</summary>
    public static implicit operator Char2(AsciiChar2 value)
    {
        return value.ToChar2();
    }

    /// <summary>Parses a two-character string into an <see cref="AsciiChar2"/>.</summary>
    public static explicit operator AsciiChar2(string value)
    {
        return FromString(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>Creates an <see cref="AsciiChar2"/> from a span of exactly two ASCII bytes.</summary>
    public static explicit operator AsciiChar2(ReadOnlySpan<byte> value)
    {
        return FromByteSpan(value);
    }

    /// <summary>Creates an <see cref="AsciiChar2"/> from a span of exactly two characters in the ASCII range.</summary>
    public static explicit operator AsciiChar2(ReadOnlySpan<char> value)
    {
        return FromCharSpan(value);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>Returns a new value with both characters uppercased. Non-letters are unchanged.</summary>
    public AsciiChar2 ToUpper()
    {
        return new(_chars[0].ToUpper(), _chars[1].ToUpper());
    }

    /// <summary>Returns a new value with both characters lowercased. Non-letters are unchanged.</summary>
    public AsciiChar2 ToLower()
    {
        return new(_chars[0].ToLower(), _chars[1].ToLower());
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= 2)
        {
            _ = _chars[0].TryFormat(destination, out _, format, provider);
            _ = _chars[1].TryFormat(destination[1..], out _, format, provider);
            charsWritten = 2;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <inheritdoc/>
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

    /// <summary>Returns the string representation with all letters lowercased.</summary>
    public string ToStringLower()
    {
        return ToString("L", null);
    }

    /// <summary>Returns the string representation with all letters uppercased.</summary>
    public string ToStringUpper()
    {
        return ToString("U", null);
    }

    /// <inheritdoc/>
    public static AsciiChar2 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(AsciiChar2)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public static AsciiChar2 Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public static AsciiChar2 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse value as an {nameof(AsciiChar2)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (utf8Destination.Length >= MaxSerializedByteLength)
        {
            _ = _chars[0].TryFormat(utf8Destination, out _, format, provider);
            _ = _chars[1].TryFormat(utf8Destination[1..], out _, format, provider);
            bytesWritten = MaxSerializedByteLength;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    internal ReadOnlySpan<AsciiChar> AsSpan()
    {
        return MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<InlineArray2<AsciiChar>, AsciiChar>(ref Unsafe.AsRef(in _chars)),
            CharCount);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(AsSpan());
    }

    /// <summary>Determines whether one <see cref="AsciiChar2"/> precedes another.</summary>
    public static bool operator <(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one <see cref="AsciiChar2"/> precedes or equals another.</summary>
    public static bool operator <=(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one <see cref="AsciiChar2"/> follows another.</summary>
    public static bool operator >(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one <see cref="AsciiChar2"/> follows or equals another.</summary>
    public static bool operator >=(AsciiChar2 left, AsciiChar2 right)
    {
        return left.CompareTo(right) >= 0;
    }
}
