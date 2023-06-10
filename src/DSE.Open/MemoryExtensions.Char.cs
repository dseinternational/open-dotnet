// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static partial class MemoryExtensions
{
    public static bool ContainsOnlyAsciiDigits(this Span<char> value)
        => ContainsOnlyAsciiDigits((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiLetters(this Span<char> value)
        => ContainsOnlyAsciiLetters((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiLettersOrDigits(this Span<char> value)
        => ContainsOnlyAsciiLettersOrDigits((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this Span<char> value)
        => ContainsOnlyAsciiUpperLettersOrDigits((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<char> value)
        => ContainsOnly(value, char.IsAsciiDigit);

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<char> value)
        => ContainsOnly(value, char.IsAsciiLetter);

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<char> value)
        => ContainsOnly(value, char.IsAsciiLetterOrDigit);

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<char> value)
        => ContainsOnly(value, v => char.IsAsciiLetterUpper(v) || char.IsAsciiDigit(v));

    public static bool TryCopyWhereNotWhitespace(this Span<char> span, Span<char> buffer, out int charsWritten)
        => TryCopyWhereNotWhitespace((ReadOnlySpan<char>)span, buffer, out charsWritten);

    public static bool TryCopyWhereNotPunctuation(this Span<char> span, Span<char> buffer, out int charsWritten)
        => TryCopyWhereNotPunctuation((ReadOnlySpan<char>)span, buffer, out charsWritten);

    public static bool TryCopyWhereNotPunctuationOrWhitespace(this Span<char> span, Span<char> buffer, out int charsWritten)
        => TryCopyWhereNotPunctuationOrWhitespace((ReadOnlySpan<char>)span, buffer, out charsWritten);

    public static bool TryCopyWhereNotWhitespace(this ReadOnlySpan<char> span, Span<char> buffer, out int charsWritten)
        => TryCopyWhere(span, buffer, v => !char.IsWhiteSpace(v), out charsWritten);

    public static bool TryCopyWhereNotPunctuation(this ReadOnlySpan<char> span, Span<char> buffer, out int charsWritten)
        => TryCopyWhere(span, buffer, v => !char.IsPunctuation(v), out charsWritten);

    public static bool TryCopyWhereNotPunctuationOrWhitespace(this ReadOnlySpan<char> span, Span<char> buffer, out int charsWritten)
        => TryCopyWhere(span, buffer, v => !(char.IsPunctuation(v) || char.IsWhiteSpace(v)), out charsWritten);

    public static int CopyExcludingPunctuation(this ReadOnlySpan<char> span, Span<char> buffer)
        => CopyWhere(span, buffer, v => !char.IsPunctuation(v));

    public static int CopyExcludingWhitespace(this ReadOnlySpan<char> span, Span<char> buffer)
        => CopyWhere(span, buffer, v => !char.IsWhiteSpace(v));

    public static int CopyExcludingPunctuationAndWhitespace(this ReadOnlySpan<char> span, Span<char> buffer)
        => CopyWhere(span, buffer, v => !(char.IsWhiteSpace(v) || char.IsPunctuation(v)));

    public static Span<char> RemoveWhitespace(this Span<char> span)
        => Remove(span, char.IsWhiteSpace);

    public static Span<char> RemovePunctuation(this Span<char> span)
        => Remove(span, char.IsPunctuation);

    public static Span<char> RemovePunctuationAndWhitespace(this Span<char> span)
        => Remove(span, v => char.IsWhiteSpace(v) || char.IsPunctuation(v));

    public static Span<char> RemoveNonLetterOrDigit(this Span<char> span)
        => Remove(span, v => !char.IsLetterOrDigit(v));

    public static Span<char> RemoveNonAsciiLetterOrDigit(this Span<char> span)
        => Remove(span, v => !char.IsAsciiLetterOrDigit(v));

    public static void ToLower(this Span<char> span)
    {
        if (span.IsEmpty)
        {
            return;
        }

        for (var i = 0; i < span.Length; i++)
        {
            span[i] = char.ToLower(span[i]);
        }
    }

    public static void ToLowerInvariant(this Span<char> span)
    {
        if (span.IsEmpty)
        {
            return;
        }

        for (var i = 0; i < span.Length; i++)
        {
            span[i] = char.ToLowerInvariant(span[i]);
        }
    }

    public static void ToUpper(this Span<char> span)
    {
        if (span.IsEmpty)
        {
            return;
        }

        for (var i = 0; i < span.Length; i++)
        {
            span[i] = char.ToUpper(span[i]);
        }
    }

    public static void ToUpperInvariant(this Span<char> span)
    {
        if (span.IsEmpty)
        {
            return;
        }

        for (var i = 0; i < span.Length; i++)
        {
            span[i] = char.ToUpperInvariant(span[i]);
        }
    }
}
