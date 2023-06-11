// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open;

/// <summary>
/// An ASCII character, stored as a byte.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AsciiChar
    : IComparable<AsciiChar>,
      IEquatable<AsciiChar>,
      IEqualityOperators<AsciiChar, AsciiChar, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiChar>
{
    private readonly byte _asciiByte;

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

    private static void EnsureIsValidAsciiChar(
        byte value,
        [CallerArgumentExpression("value")] string? name = null)
    {
        if (!IsAscii(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }
    }

    private static void EnsureIsValidAsciiChar(
        char value,
        [CallerArgumentExpression("value")] string? name = null)
    {
        if (!IsAscii(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }
    }

    public int CompareTo(AsciiChar other) => _asciiByte.CompareTo(other._asciiByte);

    public int CompareToCaseInsensitive(AsciiChar other) => CompareToCaseInsensitive(_asciiByte, other._asciiByte);

    public bool Equals(AsciiChar other) => _asciiByte == other._asciiByte;

    public override bool Equals(object? obj) => obj is AsciiChar other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_asciiByte);

    public override string ToString() => new(new[] { (char)_asciiByte });

    public static bool operator ==(AsciiChar left, AsciiChar right) => left.Equals(right);

    public static bool operator !=(AsciiChar left, AsciiChar right) => !left.Equals(right);

    public byte ToByte() => _asciiByte;

    public char ToChar() => (char)_asciiByte;

    public int ToInt32() => _asciiByte;

    public static AsciiChar FromByte(byte b) => new(b);

    public static AsciiChar FromChar(char c) => new(c);

    public static explicit operator string(AsciiChar value) => value.ToString();

    public static explicit operator AsciiChar(byte b) => FromByte(b);

    public static implicit operator byte(AsciiChar c) => c.ToByte();

    public static implicit operator char(AsciiChar c) => c.ToChar();

    public static implicit operator int(AsciiChar c) => c.ToInt32();

    public static explicit operator AsciiChar(char c) => FromChar(c);

    public AsciiChar ToUpper() => new(ToUpper(_asciiByte));

    public AsciiChar ToLower() => new(ToLower(_asciiByte));

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= 1)
        {
            destination[0] = (char)_asciiByte;
            charsWritten = 1;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

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
        [MaybeNullWhen(false)] out AsciiChar result)
    {
        if (s.Length >= 1 && IsAscii(s[0]))
        {
            result = new AsciiChar(s[0], true);
            return true;
        }

        result = default;
        return false;
    }

    public static AsciiChar Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out AsciiChar result)
        => TryParse(s.AsSpan(), provider, out result);

    public static bool operator <(AsciiChar left, AsciiChar right) => left.CompareTo(right) < 0;

    public static bool operator <=(AsciiChar left, AsciiChar right) => left.CompareTo(right) <= 0;

    public static bool operator >(AsciiChar left, AsciiChar right) => left.CompareTo(right) > 0;

    public static bool operator >=(AsciiChar left, AsciiChar right) => left.CompareTo(right) >= 0;
}
