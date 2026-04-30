// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
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
      ISpanParsable<Utf8String>,
      IRepeatableHash64
{
    private readonly ReadOnlyMemory<byte> _utf8;

    /// <summary>
    /// Initialises a new <see cref="Utf8String"/> by UTF-8 encoding <paramref name="value"/>.
    /// </summary>
    public Utf8String(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _utf8 = Encoding.UTF8.GetBytes(value);
    }

    /// <summary>
    /// Initialises a new <see cref="Utf8String"/> wrapping the supplied UTF-8 bytes without copying.
    /// </summary>
    public Utf8String(ReadOnlyMemory<byte> utf8)
    {
        _utf8 = utf8;
    }

    /// <summary>Gets a value indicating whether this <see cref="Utf8String"/> contains no bytes.</summary>
    public bool IsEmpty => _utf8.IsEmpty;

    /// <summary>Gets the length, in UTF-8 bytes.</summary>
    public int Length => _utf8.Length;

    /// <summary>
    /// Returns the underlying UTF-8 bytes as a <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    public ReadOnlyMemory<byte> AsMemory()
    {
        return _utf8;
    }

    /// <summary>Gets a <see cref="ReadOnlySpan{T}"/> view over the underlying UTF-8 bytes.</summary>
    public ReadOnlySpan<byte> Span => _utf8.Span;

    /// <summary>
    /// Returns <see langword="true"/> if the UTF-8 byte sequences are equal.
    /// </summary>
    public bool Equals(Utf8String other)
    {
        return Span.SequenceEqual(other.Span);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Utf8String other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var h = new HashCode();
        h.AddBytes(Span);
        return h.ToHashCode();
    }

    /// <summary>
    /// Returns a copy of the underlying UTF-8 bytes as a new array.
    /// </summary>
    public byte[] ToByteArray()
    {
        return _utf8.ToArray();
    }

    /// <summary>
    /// Parses <paramref name="s"/> by UTF-8 encoding it into a new <see cref="Utf8String"/>.
    /// </summary>
    public static Utf8String Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static Utf8String Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException();
        return default; // unreachable
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Utf8String result)
    {
        var byteCount = Encoding.UTF8.GetByteCount(s);
        var bytes = new byte[byteCount];
        var bytesWritten = Encoding.UTF8.GetBytes(s, bytes);
        result = new(bytes.AsMemory()[..bytesWritten]);
        return true;
    }

    /// <summary>
    /// Parses <paramref name="s"/> by UTF-8 encoding it into a new <see cref="Utf8String"/>.
    /// </summary>
    public static Utf8String Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static Utf8String Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return new(s);
    }

    /// <summary>
    /// Tries to parse <paramref name="s"/> by UTF-8 encoding it into a new <see cref="Utf8String"/>.
    /// </summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out Utf8String result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return Encoding.UTF8.GetString(Span);
    }

    /// <summary>
    /// Returns a new <see cref="Utf8String"/> by UTF-8 encoding <paramref name="value"/>.
    /// </summary>
    public static Utf8String FromString(string value)
    {
        return Parse(value, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a new <see cref="Utf8String"/> by UTF-8 encoding <paramref name="value"/>.
    /// </summary>
    public static explicit operator Utf8String(string value)
    {
        return FromString(value);
    }

    /// <summary>
    /// Returns the decoded string representation of <paramref name="value"/>.
    /// </summary>
    public static explicit operator string(Utf8String value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Returns a new <see cref="Utf8String"/> by UTF-8 encoding <paramref name="value"/>.
    /// </summary>
    public static Utf8String FromCharSequence(CharSequence value)
    {
        return Parse(value.Span, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a new <see cref="Utf8String"/> by UTF-8 encoding <paramref name="value"/>.
    /// </summary>
    public static explicit operator Utf8String(CharSequence value)
    {
        return FromCharSequence(value);
    }

    /// <summary>
    /// Returns a new <see cref="Utf8String"/> over a copy of the bytes of <paramref name="value"/>.
    /// </summary>
    public static Utf8String FromAsciiString(AsciiString value)
    {
        return new((ReadOnlyMemory<byte>)value.ToByteArray());
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(this);
    }

    /// <summary>
    /// Returns a new <see cref="Utf8String"/> over a copy of the bytes of <paramref name="value"/>.
    /// </summary>
    public static explicit operator Utf8String(AsciiString value)
    {
        return FromAsciiString(value);
    }

    /// <summary>Returns <see langword="true"/> if the operands are equal.</summary>
    public static bool operator ==(Utf8String left, Utf8String right)
    {
        return left.Equals(right);
    }

    /// <summary>Returns <see langword="true"/> if the operands are not equal.</summary>
    public static bool operator !=(Utf8String left, Utf8String right)
    {
        return !(left == right);
    }
}
