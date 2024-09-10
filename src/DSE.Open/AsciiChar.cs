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
        ISpanFormatableCharCountProvider
{
    private readonly byte _asciiByte;

    public static int MaxSerializedByteLength => 1;

    public AsciiChar(byte asciiByte) : this(asciiByte, false)
    {
    }

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

    public int CompareTo(AsciiChar other)
    {
        return _asciiByte.CompareTo(other._asciiByte);
    }

    public int CompareToIgnoreCase(AsciiChar other)
    {
        return CompareToIgnoreCase(_asciiByte, other._asciiByte);
    }

    public bool Equals(AsciiChar other)
    {
        return _asciiByte == other._asciiByte;
    }

    public bool Equals(byte other)
    {
        return _asciiByte == other;
    }

    public override bool Equals(object? obj)
    {
        return obj is AsciiChar other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_asciiByte);
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

    public static bool operator ==(AsciiChar left, AsciiChar right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AsciiChar left, AsciiChar right)
    {
        return !left.Equals(right);
    }

    public byte ToByte()
    {
        return _asciiByte;
    }

    public char ToChar()
    {
        return (char)_asciiByte;
    }

    public int ToInt32()
    {
        return _asciiByte;
    }

    public static AsciiChar FromByte(byte asciiByte)
    {
        return new(asciiByte);
    }

    public static AsciiChar FromChar(char asciiUtf16Char)
    {
        return new(asciiUtf16Char);
    }

    public static explicit operator string(AsciiChar value)
    {
        return value.ToString();
    }

    public static explicit operator AsciiChar(byte asciiByte)
    {
        return FromByte(asciiByte);
    }

    public static explicit operator AsciiChar(char asciiUtf16Char)
    {
        return FromChar(asciiUtf16Char);
    }

    public static implicit operator byte(AsciiChar asciiChar)
    {
        return asciiChar.ToByte();
    }

    public static implicit operator char(AsciiChar asciiChar)
    {
        return asciiChar.ToChar();
    }

    public static implicit operator int(AsciiChar asciiChar)
    {
        return asciiChar.ToInt32();
    }

    public AsciiChar ToUpper()
    {
        return new(ToUpper(_asciiByte));
    }

    public AsciiChar ToLower()
    {
        return new(ToLower(_asciiByte));
    }

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

    public static AsciiChar Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{s}' as a {nameof(AsciiChar)}");
        return default; // unreachable
    }

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

    public static AsciiChar Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiChar result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static AsciiChar Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{utf8Text.ToArray()}' as a {nameof(AsciiChar)}");
        return default; // unreachable
    }

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

    public static bool operator <(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(AsciiChar left, AsciiChar right)
    {
        return left.CompareTo(right) >= 0;
    }
}
