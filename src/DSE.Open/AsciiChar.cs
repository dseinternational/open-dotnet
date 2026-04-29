// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An ASCII character, stored as a byte.
/// </summary>
[JsonConverter(typeof(JsonStringAsciiCharNConverter<AsciiChar>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct AsciiChar
    : IComparable<AsciiChar>,
      IEquatable<AsciiChar>,
      IEqualityOperators<AsciiChar, AsciiChar, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiChar>,
      IUtf8SpanSerializable<AsciiChar>,
      ISpanFormattableCharCountProvider
{
    private readonly byte _asciiByte;

    /// <summary>
    /// Gets the maximum number of bytes used by the UTF-8 serialized form of an <see cref="AsciiChar"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 1;

    /// <summary>
    /// Initializes a new <see cref="AsciiChar"/> from the specified ASCII byte value.
    /// </summary>
    /// <param name="asciiByte">A byte in the ASCII range (0-127).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="asciiByte"/> is not an ASCII character.</exception>
    public AsciiChar(byte asciiByte) : this(asciiByte, false)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="AsciiChar"/> from the specified ASCII character.
    /// </summary>
    /// <param name="asciiChar">A character in the ASCII range (0-127).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="asciiChar"/> is not an ASCII character.</exception>
    public AsciiChar(char asciiChar) : this(asciiChar, false)
    {
    }

    private AsciiChar(byte asciiByte, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureIsValidAsciiChar(asciiByte);
        }

        _asciiByte = asciiByte;
    }

    private AsciiChar(char asciiChar, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureIsValidAsciiChar(asciiChar);
        }

        _asciiByte = (byte)asciiChar;
    }

    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not an ASCII character.</exception>
    private static void EnsureIsValidAsciiChar(
        byte value,
        [CallerArgumentExpression(nameof(value))]
        string? name = null)
    {
        EnsureIsValidAsciiChar((char)value, name);
    }

    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not an ASCII character.</exception>
    private static void EnsureIsValidAsciiChar(
        char value,
        [CallerArgumentExpression(nameof(value))]
        string? name = null)
    {
        if (!IsAscii(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(name);
        }
    }

    /// <inheritdoc/>
    public int CompareTo(AsciiChar other)
    {
        return _asciiByte.CompareTo(other._asciiByte);
    }

    /// <summary>
    /// Compares this instance to another <see cref="AsciiChar"/> ignoring ASCII case.
    /// </summary>
    /// <param name="other">The other <see cref="AsciiChar"/> to compare with.</param>
    /// <returns>A signed integer indicating relative order, ignoring case.</returns>
    public int CompareToIgnoreCase(AsciiChar other)
    {
        return CompareToIgnoreCase(_asciiByte, other._asciiByte);
    }

    /// <inheritdoc/>
    public bool Equals(AsciiChar other)
    {
        return _asciiByte == other._asciiByte;
    }

    /// <summary>
    /// Determines whether the underlying ASCII byte equals the specified value.
    /// </summary>
    /// <param name="other">The byte to compare against.</param>
    /// <returns><see langword="true"/> if the underlying byte equals <paramref name="other"/>; otherwise <see langword="false"/>.</returns>
    public bool Equals(byte other)
    {
        return _asciiByte == other;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is AsciiChar other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_asciiByte);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
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

    /// <summary>Determines whether two <see cref="AsciiChar"/> values are equal.</summary>
    public static bool operator ==(AsciiChar left, AsciiChar right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="AsciiChar"/> values are not equal.</summary>
    public static bool operator !=(AsciiChar left, AsciiChar right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Returns the underlying ASCII byte value.
    /// </summary>
    public byte ToByte()
    {
        return _asciiByte;
    }

    /// <summary>
    /// Returns the underlying ASCII byte value as a UTF-16 <see cref="char"/>.
    /// </summary>
    public char ToChar()
    {
        return (char)_asciiByte;
    }

    /// <summary>
    /// Returns the underlying ASCII byte value as a 32-bit signed integer.
    /// </summary>
    public int ToInt32()
    {
        return _asciiByte;
    }

    /// <summary>
    /// Creates an <see cref="AsciiChar"/> from the specified ASCII byte value.
    /// </summary>
    /// <param name="asciiByte">A byte in the ASCII range (0-127).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="asciiByte"/> is not an ASCII character.</exception>
    public static AsciiChar FromByte(byte asciiByte)
    {
        return new(asciiByte);
    }

    /// <summary>
    /// Creates an <see cref="AsciiChar"/> from the specified UTF-16 character.
    /// </summary>
    /// <param name="asciiUtf16Char">A character in the ASCII range (0-127).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="asciiUtf16Char"/> is not an ASCII character.</exception>
    public static AsciiChar FromChar(char asciiUtf16Char)
    {
        return new(asciiUtf16Char);
    }

    /// <summary>Returns the single-character string representation of the value.</summary>
    public static explicit operator string(AsciiChar value)
    {
        return value.ToString();
    }

    /// <summary>Converts a byte to an <see cref="AsciiChar"/>, validating that it is in the ASCII range.</summary>
    public static explicit operator AsciiChar(byte asciiByte)
    {
        return FromByte(asciiByte);
    }

    /// <summary>Converts a UTF-16 character to an <see cref="AsciiChar"/>, validating that it is in the ASCII range.</summary>
    public static explicit operator AsciiChar(char asciiUtf16Char)
    {
        return FromChar(asciiUtf16Char);
    }

    /// <summary>Returns the underlying ASCII byte value.</summary>
    public static implicit operator byte(AsciiChar asciiChar)
    {
        return asciiChar.ToByte();
    }

    /// <summary>Returns the underlying ASCII value as a UTF-16 <see cref="char"/>.</summary>
    public static implicit operator char(AsciiChar asciiChar)
    {
        return asciiChar.ToChar();
    }

    /// <summary>Returns the underlying ASCII value as a 32-bit signed integer.</summary>
    public static implicit operator int(AsciiChar asciiChar)
    {
        return asciiChar.ToInt32();
    }

    /// <summary>
    /// Returns the uppercase equivalent of this character. Non-letters are returned unchanged.
    /// </summary>
    public AsciiChar ToUpper()
    {
        return new(ToUpper(_asciiByte));
    }

    /// <summary>
    /// Returns the lowercase equivalent of this character. Non-letters are returned unchanged.
    /// </summary>
    public AsciiChar ToLower()
    {
        return new(ToLower(_asciiByte));
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= 1)
        {
            switch (format)
            {
                case "":
                    destination[0] = (char)_asciiByte;
                    break;
                case "U" or "u":
                    destination[0] = (char)ToUpper(_asciiByte);
                    break;
                case "L" or "l":
                    destination[0] = (char)ToLower(_asciiByte);
                    break;
                default:
                    ThrowHelper.ThrowFormatException($"The format '{format}' is not supported for {nameof(AsciiChar)}");
                    break;
            }

            charsWritten = 1;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (utf8Destination.Length >= 1)
        {
            switch (format)
            {
                case "":
                    utf8Destination[0] = _asciiByte;
                    break;
                case "U" or "u":
                    utf8Destination[0] = ToUpper(_asciiByte);
                    break;
                case "L" or "l":
                    utf8Destination[0] = ToLower(_asciiByte);
                    break;
                default:
                    ThrowHelper.ThrowFormatException($"The format '{format}' is not supported for {nameof(AsciiChar)}");
                    break;
            }

            bytesWritten = 1;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(1, (this, format, formatProvider), (span, state) =>
        {
            var (asciiChar, format, formatProvider) = state;
            var result = asciiChar.TryFormat(span, out var charsWritten, format, formatProvider);
            Debug.Assert(result);
            Debug.Assert(charsWritten == 1);
        });
    }

    /// <inheritdoc/>
    public static AsciiChar Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(AsciiChar)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiChar result)
    {
        if (s.Length >= 1 && IsAscii(s[0]))
        {
            result = new(s[0], true);
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc/>
    public static AsciiChar Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiChar result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public static AsciiChar Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{utf8Text.ToArray()}' as a {nameof(AsciiChar)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out AsciiChar result)
    {
        if (!utf8Text.IsEmpty && IsAscii(utf8Text[0]))
        {
            result = new(utf8Text[0], true);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Determines whether one <see cref="AsciiChar"/> precedes another.</summary>
    public static bool operator <(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one <see cref="AsciiChar"/> precedes or equals another.</summary>
    public static bool operator <=(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one <see cref="AsciiChar"/> follows another.</summary>
    public static bool operator >(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one <see cref="AsciiChar"/> follows or equals another.</summary>
    public static bool operator >=(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) >= 0;
    }
}
