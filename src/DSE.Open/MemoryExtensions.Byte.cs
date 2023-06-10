// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static partial class MemoryExtensions
{
    public static bool ContainsOnlyAsciiDigits(this Span<byte> value)
        => ContainsOnlyAsciiDigits((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiLetters(this Span<byte> value)
        => ContainsOnlyAsciiLetters((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiLettersOrDigits(this Span<byte> value)
        => ContainsOnlyAsciiLettersOrDigits((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this Span<byte> value)
        => ContainsOnlyAsciiUpperLettersOrDigits((ReadOnlySpan<byte>)value);

    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<byte> value)
        => ContainsOnly(value, AsciiChar.IsDigit);

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<byte> value)
        => ContainsOnly(value, AsciiChar.IsLetter);

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<byte> value)
        => ContainsOnly(value, AsciiChar.IsLetterOrDigit);

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<byte> value)
        => ContainsOnly(value, v => AsciiChar.IsLetterUpper(v) || AsciiChar.IsDigit(v));

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
}
