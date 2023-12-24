// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.Intrinsics;

namespace DSE.Open;

public static partial class MemoryExtensions
{
    internal static class SearchChars
    {
        internal static readonly SearchValues<char> s_asciiLetters = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");

        internal static readonly SearchValues<char> s_asciiLettersAndDigits = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        internal static readonly SearchValues<char> s_asciiUpperLettersAndDigits = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
    }

    public static bool ContainsOnlyAsciiDigits(this Span<char> value)
        => ContainsOnlyAsciiDigits((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiLetters(this Span<char> value)
        => ContainsOnlyAsciiLetters((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiLettersOrDigits(this Span<char> value)
        => ContainsOnlyAsciiLettersOrDigits((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this Span<char> value)
        => ContainsOnlyAsciiUpperLettersOrDigits((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiLettersLower(this Span<char> value)
        => ContainsOnlyAsciiLettersLower((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiLettersUpper(this Span<char> value)
        => ContainsOnlyAsciiLettersUpper((ReadOnlySpan<char>)value);

    public static bool ContainsOnlyAsciiDigits(this ReadOnlySpan<char> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<ushort>.Count)
        {
            return value.IndexOfAnyExceptInRange('0', '9') == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<char> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<ushort>.Count)
        {
            return value.IndexOfAnyExcept(SearchChars.s_asciiLetters) == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiLetter(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<char> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<ushort>.Count)
        {
            return value.IndexOfAnyExcept(SearchChars.s_asciiLettersAndDigits) == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiLetterOrDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<char> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<ushort>.Count)
        {
            return value.IndexOfAnyExcept(SearchChars.s_asciiUpperLettersAndDigits) == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiLetterUpper(value[i]) && !char.IsAsciiDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<char> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<ushort>.Count)
        {
            return value.IndexOfAnyExceptInRange('a', 'z') == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiLetterLower(value[i]))
            {
                return false;
            }
        }

        return true;

    }

    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<char> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<ushort>.Count)
        {
            return value.IndexOfAnyExceptInRange('A', 'Z') == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsAsciiLetterUpper(value[i]))
            {
                return false;
            }
        }

        return true;
    }

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
            span[i] = char.ToLower(span[i], CultureInfo.CurrentCulture);
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
            span[i] = char.ToUpper(span[i], CultureInfo.CurrentCulture);
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
