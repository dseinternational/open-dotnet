// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;
using DSE.Open.Hashing;
using DSE.Open.Runtime.InteropServices;

namespace DSE.Open;

public static partial class MemoryExtensions
{
    internal static class SearchBytes
    {
        internal static readonly SearchValues<byte> s_asciiDigits =
            SearchValues.Create("0123456789"u8);

        internal static readonly SearchValues<byte> s_asciiLetters =
            SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"u8);

        internal static readonly SearchValues<byte> s_asciiLettersAndDigits =
            SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"u8);

        internal static readonly SearchValues<byte> s_asciiUpperLettersAndDigits =
            SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"u8);

        internal static readonly SearchValues<byte> s_asciiLettersLower =
            SearchValues.Create("abcdefghijklmnopqrstuvwxyz"u8);

        internal static readonly SearchValues<byte> s_asciiLettersUpper =
            SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ"u8);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an ASCII digit (0-9).
    /// </summary>
    public static bool ContainsOnlyAsciiDigits(this Span<byte> value)
    {
        return ContainsOnlyAsciiDigits((ReadOnlySpan<byte>)value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an ASCII letter (A-Z or a-z).
    /// </summary>
    public static bool ContainsOnlyAsciiLetters(this Span<byte> value)
    {
        return ContainsOnlyAsciiLetters((ReadOnlySpan<byte>)value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an ASCII letter or digit.
    /// </summary>
    public static bool ContainsOnlyAsciiLettersOrDigits(this Span<byte> value)
    {
        return ContainsOnlyAsciiLettersOrDigits((ReadOnlySpan<byte>)value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an upper-case ASCII letter or digit.
    /// </summary>
    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this Span<byte> value)
    {
        return ContainsOnlyAsciiUpperLettersOrDigits((ReadOnlySpan<byte>)value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is a lower-case ASCII letter (a-z).
    /// </summary>
    public static bool ContainsOnlyAsciiLettersLower(this Span<byte> value)
    {
        return ContainsOnlyAsciiLettersLower((ReadOnlySpan<byte>)value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an upper-case ASCII letter (A-Z).
    /// </summary>
    public static bool ContainsOnlyAsciiLettersUpper(this Span<byte> value)
    {
        return ContainsOnlyAsciiLettersUpper((ReadOnlySpan<byte>)value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element is an ASCII digit (0-9).
    /// </summary>
    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiDigits();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an ASCII digit (0-9).
    /// </summary>
    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiDigits, AsciiChar.IsDigit);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an ASCII letter (A-Z or a-z).
    /// </summary>
    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLetters, AsciiChar.IsLetter);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an ASCII letter or digit.
    /// </summary>
    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLettersAndDigits, AsciiChar.IsLetterOrDigit);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an upper-case ASCII letter or digit.
    /// </summary>
    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiUpperLettersAndDigits, Filter);

        static bool Filter(byte value)
        {
            return AsciiChar.IsLetterUpper(value) || AsciiChar.IsDigit(value);
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is a lower-case ASCII letter (a-z).
    /// </summary>
    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLettersLower, AsciiChar.IsLetterLower);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every byte is an upper-case ASCII letter (A-Z).
    /// </summary>
    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLettersUpper, AsciiChar.IsLetterUpper);
    }

    /// <summary>
    /// Returns a deterministic, platform-stable 64-bit hash code for <paramref name="value"/>.
    /// </summary>
    public static ulong GetRepeatableHashCode(this ReadOnlySpan<byte> value)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
    }

    /// <summary>
    /// Copies non-whitespace bytes from <paramref name="span"/> into <paramref name="buffer"/>.
    /// Returns <see langword="false"/> if <paramref name="buffer"/> is too small.
    /// </summary>
    public static bool TryCopyWhereNotWhitespace(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhereNotWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    /// <summary>
    /// Copies bytes from <paramref name="span"/> into <paramref name="buffer"/>, excluding ASCII punctuation.
    /// Returns <see langword="false"/> if <paramref name="buffer"/> is too small.
    /// </summary>
    public static bool TryCopyWhereNotPunctuation(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhereNotPunctuation((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    /// <summary>
    /// Copies bytes from <paramref name="span"/> into <paramref name="buffer"/>, excluding ASCII
    /// punctuation and whitespace. Returns <see langword="false"/> if <paramref name="buffer"/> is too small.
    /// </summary>
    public static bool TryCopyWhereNotPunctuationOrWhitespace(
        this Span<byte> span,
        Span<byte> buffer,
        out int bytesWritten)
    {
        return TryCopyWhereNotPunctuationOrWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    /// <summary>
    /// Copies non-whitespace bytes from <paramref name="span"/> into <paramref name="buffer"/>.
    /// Returns <see langword="false"/> if <paramref name="buffer"/> is too small.
    /// </summary>
    public static bool TryCopyWhereNotWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhere(span, buffer, v => !AsciiChar.IsWhiteSpace(v), out bytesWritten);
    }

    /// <summary>
    /// Copies bytes from <paramref name="span"/> into <paramref name="buffer"/>, excluding ASCII punctuation.
    /// Returns <see langword="false"/> if <paramref name="buffer"/> is too small.
    /// </summary>
    public static bool TryCopyWhereNotPunctuation(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhere(span, buffer, v => !AsciiChar.IsPunctuation(v), out bytesWritten);
    }

    /// <summary>
    /// Copies bytes from <paramref name="span"/> into <paramref name="buffer"/>, excluding ASCII
    /// punctuation and whitespace. Returns <see langword="false"/> if <paramref name="buffer"/> is too small.
    /// </summary>
    public static bool TryCopyWhereNotPunctuationOrWhitespace(
        this ReadOnlySpan<byte> span,
        Span<byte> buffer,
        out int bytesWritten)
    {
        return TryCopyWhere(span,
            buffer,
            v => !(AsciiChar.IsPunctuation(v) || AsciiChar.IsWhiteSpace(v)),
            out bytesWritten);
    }

    /// <summary>
    /// Copies bytes from <paramref name="span"/> into <paramref name="buffer"/>, excluding ASCII punctuation.
    /// Returns the number of bytes copied.
    /// </summary>
    public static int CopyExcludingPunctuation(this ReadOnlySpan<byte> span, Span<byte> buffer)
    {
        return CopyWhere(span, buffer, v => !AsciiChar.IsPunctuation(v));
    }

    /// <summary>
    /// Copies non-whitespace bytes from <paramref name="span"/> into <paramref name="buffer"/>.
    /// Returns the number of bytes copied.
    /// </summary>
    public static int CopyExcludingWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer)
    {
        return CopyWhere(span, buffer, v => !AsciiChar.IsWhiteSpace(v));
    }

    /// <summary>
    /// Copies bytes from <paramref name="span"/> into <paramref name="buffer"/>, excluding ASCII
    /// punctuation and whitespace. Returns the number of bytes copied.
    /// </summary>
    public static int CopyExcludingPunctuationAndWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer)
    {
        return CopyWhere(span, buffer, v => !(AsciiChar.IsWhiteSpace(v) || AsciiChar.IsPunctuation(v)));
    }

    /// <summary>
    /// Removes ASCII whitespace bytes from <paramref name="span"/> in place and returns a span over the result.
    /// </summary>
    public static Span<byte> RemoveWhitespace(this Span<byte> span)
    {
        return Remove(span, AsciiChar.IsWhiteSpace);
    }

    /// <summary>
    /// Removes ASCII punctuation bytes from <paramref name="span"/> in place and returns a span over the result.
    /// </summary>
    public static Span<byte> RemovePunctuation(this Span<byte> span)
    {
        return Remove(span, AsciiChar.IsPunctuation);
    }

    /// <summary>
    /// Removes ASCII punctuation and whitespace bytes from <paramref name="span"/> in place and returns
    /// a span over the result.
    /// </summary>
    public static Span<byte> RemovePunctuationAndWhitespace(this Span<byte> span)
    {
        return Remove(span, v => AsciiChar.IsWhiteSpace(v) || AsciiChar.IsPunctuation(v));
    }

    /// <summary>
    /// Removes any byte that is not an ASCII letter or digit from <paramref name="span"/> in place and
    /// returns a span over the result.
    /// </summary>
    public static Span<byte> RemoveNonLetterOrDigit(this Span<byte> span)
    {
        return Remove(span, v => !AsciiChar.IsLetterOrDigit(v));
    }

    /// <summary>
    /// Removes any byte that is not an ASCII letter or digit from <paramref name="span"/> in place and
    /// returns a span over the result.
    /// </summary>
    public static Span<byte> RemoveNonAsciiLetterOrDigit(this Span<byte> span)
    {
        return Remove(span, v => !AsciiChar.IsLetterOrDigit(v));
    }

    /// <summary>
    /// Checks if the span contains only valid ASCII bytes.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAscii(this ReadOnlySpan<byte> value)
    {
        return Ascii.IsValid(value);
    }
}
