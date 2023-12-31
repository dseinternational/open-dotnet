// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.Intrinsics;
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
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<byte>.Count)
        {
            return value.IndexOfAnyExceptInRange((byte)'0', (byte)'9') == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLetters();
    }

    public static bool ContainsOnlyAsciiLetters(this ReadOnlySpan<byte> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<byte>.Count)
        {
            return value.IndexOfAnyExcept(SearchBytes.s_asciiLetters) == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetter(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersOrDigits();
    }

    public static bool ContainsOnlyAsciiLettersOrDigits(this ReadOnlySpan<byte> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<byte>.Count)
        {
            return value.IndexOfAnyExcept(SearchBytes.s_asciiLettersAndDigits) == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetterOrDigit(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiUpperLettersOrDigits();
    }

    public static bool ContainsOnlyAsciiUpperLettersOrDigits(this ReadOnlySpan<byte> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<byte>.Count)
        {
            return value.IndexOfAnyExcept(SearchBytes.s_asciiUpperLettersAndDigits) == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!(AsciiChar.IsLetterUpper(value[i]) || AsciiChar.IsDigit(value[i])))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersLower();
    }

    public static bool ContainsOnlyAsciiLettersLower(this ReadOnlySpan<byte> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length >= Vector128<byte>.Count)
        {
            return value.IndexOfAnyExceptInRange((byte)'a', (byte)'z') == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetterLower(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<AsciiChar> value)
    {
        return ValuesMarshal.AsBytes(value).ContainsOnlyAsciiLettersUpper();
    }

    public static bool ContainsOnlyAsciiLettersUpper(this ReadOnlySpan<byte> value)
    {
        if (Vector128.IsHardwareAccelerated && value.Length > Vector128<byte>.Count)
        {
            return value.IndexOfAnyExceptInRange((byte)'A', (byte)'Z') == -1;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetterUpper(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool TryCopyWhereNotWhitespace(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhereNotWhitespace((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    public static bool TryCopyWhereNotPunctuation(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhereNotPunctuation((ReadOnlySpan<byte>)span, buffer, out bytesWritten);
    }

    public static bool TryCopyWhereNotPunctuationOrWhitespace(this Span<byte> span, Span<byte> buffer, out int bytesWritten)
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

    public static bool TryCopyWhereNotPunctuationOrWhitespace(this ReadOnlySpan<byte> span, Span<byte> buffer, out int bytesWritten)
    {
        return TryCopyWhere(span, buffer, v => !(AsciiChar.IsPunctuation(v) || AsciiChar.IsWhiteSpace(v)), out bytesWritten);
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

    /// <summary>
    /// Determines whether the specified spans of ASCII are equal, ignoring case.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool SequenceEqualsCaseInsensitive(this ReadOnlySpan<AsciiChar> a, ReadOnlySpan<AsciiChar> b)
    {
        return Ascii.EqualsIgnoreCase(ValuesMarshal.AsBytes(a), ValuesMarshal.AsBytes(b));
    }
}
