// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;

namespace DSE.Open;

public static partial class MemoryExtensions
{
    internal static class SearchBytes
    {
        internal static readonly SearchValues<byte> s_asciiLetters = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"u8);

        internal static readonly SearchValues<byte> s_asciiLettersAndDigits =
            SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"u8);

        internal static readonly SearchValues<byte> s_asciiUpperLettersAndDigits = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"u8);
    }

    public static bool ContainsOnlyAsciiDigits(this Span<byte> value)
        => ContainsOnlyAsciiDigits((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiLetters(this Span<byte> value)
        => ContainsOnlyAsciiLetters((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiLettersOrDigits(this Span<byte> value)
        => ContainsOnlyAsciiLettersOrDigits((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this Span<byte> value)
        => ContainsOnlyAsciiUpperLettersOrDigits((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiLettersLower(this Span<byte> value)
        => ContainsOnlyAsciiLettersLower((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiLettersUpper(this Span<byte> value)
        => ContainsOnlyAsciiLettersUpper((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<byte> value)
        => value.IndexOfAnyExceptInRange((byte)'0', (byte)'9') == -1;

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<byte> value)
        => value.IndexOfAnyExcept(SearchBytes.s_asciiLetters) == -1;

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<byte> value)
        => value.IndexOfAnyExcept(SearchBytes.s_asciiLettersAndDigits) == -1;

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<byte> value)
        => value.IndexOfAnyExcept(SearchBytes.s_asciiUpperLettersAndDigits) == -1;

    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<byte> value)
        => value.IndexOfAnyExceptInRange((byte)'a', (byte)'z') == -1;

    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<byte> value)
        => value.IndexOfAnyExceptInRange((byte)'A', (byte)'Z') == -1;


    public static bool TryCopyWhereNotWhitespace(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
        => TryCopyWhereNotWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);

    public static bool TryCopyWhereNotPunctuation(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
        => TryCopyWhereNotPunctuation((ReadOnlySpan<byte>)span, buffer, out bytesWritten);

    public static bool TryCopyWhereNotPunctuationOrWhitespace(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
        => TryCopyWhereNotPunctuationOrWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);

    public static bool TryCopyWhereNotWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
        => TryCopyWhere(span, buffer, v => !AsciiChar.IsWhiteSpace(v), out bytesWritten);

    public static bool TryCopyWhereNotPunctuation(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
        => TryCopyWhere(span, buffer, v => !AsciiChar.IsPunctuation(v), out bytesWritten);

    public static bool TryCopyWhereNotPunctuationOrWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
        => TryCopyWhere(span, buffer, v => !(AsciiChar.IsPunctuation(v) || AsciiChar.IsWhiteSpace(v)), out bytesWritten);

    public static int CopyExcludingPunctuation(this ReadOnlySpan<byte> span, Span<byte> buffer)
        => CopyWhere(span, buffer, v => !AsciiChar.IsPunctuation(v));

    public static int CopyExcludingWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer)
        => CopyWhere(span, buffer, v => !AsciiChar.IsWhiteSpace(v));

    public static int CopyExcludingPunctuationAndWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer)
        => CopyWhere(span, buffer, v => !(AsciiChar.IsWhiteSpace(v) || AsciiChar.IsPunctuation(v)));

    public static Span<byte> RemoveWhitespace(this Span<byte> span)
        => Remove(span, AsciiChar.IsWhiteSpace);

    public static Span<byte> RemovePunctuation(this Span<byte> span)
        => Remove(span, AsciiChar.IsPunctuation);

    public static Span<byte> RemovePunctuationAndWhitespace(this Span<byte> span)
        => Remove(span, v => AsciiChar.IsWhiteSpace(v) || AsciiChar.IsPunctuation(v));

    public static Span<byte> RemoveNonLetterOrDigit(this Span<byte> span)
        => Remove(span, v => !AsciiChar.IsLetterOrDigit(v));

    public static Span<byte> RemoveNonAsciiLetterOrDigit(this Span<byte> span)
        => Remove(span, v => !AsciiChar.IsLetterOrDigit(v));

    [Obsolete("Use `Ascii.ToLowerInPlace`")]
    public static void ToLower(this Span<byte> span)
    {
        if (span.IsEmpty)
        {
            return;
        }

        for (var i = 0; i < span.Length; i++)
        {
            span[i] = AsciiChar.ToLower(span[i]);
        }
    }

    [Obsolete("Use `Ascii.ToUpperInPlace`")]
    public static void ToUpper(this Span<byte> span)
    {
        if (span.IsEmpty)
        {
            return;
        }

        for (var i = 0; i < span.Length; i++)
        {
            span[i] = AsciiChar.ToUpper(span[i]);
        }
    }

    /// <summary>
    /// Checks if the span contains only ASCII letters.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ContainsOnlyLetters(this ReadOnlySpan<AsciiChar> value)
        => ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLetters();

    /// <summary>
    /// Checks if the span contains only valid ASCII bytes.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAscii(this ReadOnlySpan<byte> value) => Ascii.IsValid(value);


    /// <summary>
    /// Determines whether the specified spans of ASCII are equal, ignoring case.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool SequenceEqualsCaseInsensitive(this ReadOnlySpan<AsciiChar> a, ReadOnlySpan<AsciiChar> b)
        => Ascii.EqualsIgnoreCase(ValuesMarshal.AsBytes(a), ValuesMarshal.AsBytes(b));
}
