// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="byte"/> where the data is UTF8 encoded text.
/// </summary>
[JsonConverter(typeof(JsonStringUtf8StringConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Utf8String
    : IEquatable<Utf8String>,
      ISpanFormattable,
      ISpanParsable<Utf8String>
{
    private readonly ReadOnlyMemory<byte> _utf8;

    public Utf8String(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _utf8 = Encoding.UTF8.GetBytes(value);
    }

    public Utf8String(ReadOnlyMemory<byte> utf8)
    {
        _utf8 = utf8;
    }

    public bool IsEmpty => _utf8.IsEmpty;

    public int Length => _utf8.Length;

    public ReadOnlyMemory<byte> AsMemory()
    {
        return _utf8;
    }

    public ReadOnlySpan<byte> Span => _utf8.Span;

    public bool Equals(Utf8String other)
    {
        return Span.SequenceEqual(other.Span);
    }

    public override bool Equals(object? obj)
    {
        return obj is Utf8String other && Equals(other);
    }

    public override int GetHashCode()
    {
        var h = new HashCode();
        h.AddBytes(Span);
        return h.ToHashCode();
    }

    public byte[] ToByteArray()
    {
        return _utf8.ToArray();
    }

    public static Utf8String Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static Utf8String Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException();
        return default; // unreachable
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Utf8String result)
    {
        var byteCount = Encoding.UTF8.GetByteCount(s);
        var bytes = new byte[byteCount];
        var bytesWritten = Encoding.UTF8.GetBytes(s, bytes);
        result = new(bytes.AsMemory()[..bytesWritten]);
        return true;
    }

    public static Utf8String Parse(string s)
    {
        return Parse(s, null);
    }

    public static Utf8String Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return new(s);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out Utf8String result)
    {
        return TryParse(s, null, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Utf8String result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var charCount = Encoding.UTF8.GetCharCount(Span);

        if (destination.Length < charCount)
        {
            charsWritten = 0;
            return false;
        }

        charsWritten = Encoding.UTF8.GetChars(Span, destination);
        return true;
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return Encoding.UTF8.GetString(Span);
    }

    public static Utf8String FromString(string value)
    {
        return Parse(value, CultureInfo.CurrentCulture);
    }

    public static explicit operator Utf8String(string value)
    {
        return FromString(value);
    }

    public static explicit operator string(Utf8String value)
    {
        return value.ToString();
    }

    public static Utf8String FromCharSequence(CharSequence value)
    {
        return Parse(value.AsSpan(), CultureInfo.CurrentCulture);
    }

    public static explicit operator Utf8String(CharSequence value)
    {
        return FromCharSequence(value);
    }

    public static Utf8String FromAsciiString(AsciiString value)
    {
        return new((ReadOnlyMemory<byte>)value.ToByteArray());
    }

    public static explicit operator Utf8String(AsciiString value)
    {
        return FromAsciiString(value);
    }

    public static bool operator ==(Utf8String left, Utf8String right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Utf8String left, Utf8String right)
    {
        return !(left == right);
    }
}
