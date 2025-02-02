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

    public static bool ContainsOnlyAsciiDigits(this Span<byte> value)
    {
        return ContainsOnlyAsciiDigits((ReadOnlySpan<byte>)value);
    }

    public static bool ContainsOnlyAsciiLetters(this Span<byte> value)
    {
        return ContainsOnlyAsciiLetters((ReadOnlySpan<byte>)value);
    }

    public static bool ContainsOnlyAsciiLettersOrDigits(this Span<byte> value)
    {
        return ContainsOnlyAsciiLettersOrDigits((ReadOnlySpan<byte>)value);
    }

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this Span<byte> value)
    {
        return ContainsOnlyAsciiUpperLettersOrDigits((ReadOnlySpan<byte>)value);
    }

    public static bool ContainsOnlyAsciiLettersLower(this Span<byte> value)
    {
        return ContainsOnlyAsciiLettersLower((ReadOnlySpan<byte>)value);
    }

    public static bool ContainsOnlyAsciiLettersUpper(this Span<byte> value)
    {
        return ContainsOnlyAsciiLettersUpper((ReadOnlySpan<byte>)value);
    }

    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiDigits();
    }

    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLetters, AsciiChar.IsDigit);
    }

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLetters, AsciiChar.IsLetter);
    }

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLettersAndDigits, AsciiChar.IsLetterOrDigit);
    }

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiUpperLettersAndDigits, Filter);

        static bool Filter(byte value)
        {
            return AsciiChar.IsLetterUpper(value) || AsciiChar.IsDigit(value);
        }
    }

    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLettersLower, AsciiChar.IsLetterLower);
    }

    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<byte> value)
    {
        return ContainsOnlyCore(value, SearchBytes.s_asciiLettersUpper, AsciiChar.IsLetterUpper);
    }

    public static ulong GetRepeatableHashCode(this ReadOnlySpan<byte> value)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
    }

    public static bool TryCopyWhereNotWhitespace(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhereNotWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    public static bool TryCopyWhereNotPunctuation(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhereNotPunctuation((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    public static bool TryCopyWhereNotPunctuationOrWhitespace(
        this Span<byte> span,
        Span<byte> buffer,
        out int bytesWritten)
    {
        return TryCopyWhereNotPunctuationOrWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    public static bool TryCopyWhereNotWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhere(span, buffer, v => !AsciiChar.IsWhiteSpace(v), out bytesWritten);
    }

    public static bool TryCopyWhereNotPunctuation(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhere(span, buffer, v => !AsciiChar.IsPunctuation(v), out bytesWritten);
    }

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

    public static int CopyExcludingPunctuation(this ReadOnlySpan<byte> span, Span<byte> buffer)
    {
        return CopyWhere(span, buffer, v => !AsciiChar.IsPunctuation(v));
    }

    public static int CopyExcludingWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer)
    {
        return CopyWhere(span, buffer, v => !AsciiChar.IsWhiteSpace(v));
    }

    public static int CopyExcludingPunctuationAndWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer)
    {
        return CopyWhere(span, buffer, v => !(AsciiChar.IsWhiteSpace(v) || AsciiChar.IsPunctuation(v)));
    }

    public static Span<byte> RemoveWhitespace(this Span<byte> span)
    {
        return Remove(span, AsciiChar.IsWhiteSpace);
    }

    public static Span<byte> RemovePunctuation(this Span<byte> span)
    {
        return Remove(span, AsciiChar.IsPunctuation);
    }

    public static Span<byte> RemovePunctuationAndWhitespace(this Span<byte> span)
    {
        return Remove(span, v => AsciiChar.IsWhiteSpace(v) || AsciiChar.IsPunctuation(v));
    }

    public static Span<byte> RemoveNonLetterOrDigit(this Span<byte> span)
    {
        return Remove(span, v => !AsciiChar.IsLetterOrDigit(v));
    }

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
