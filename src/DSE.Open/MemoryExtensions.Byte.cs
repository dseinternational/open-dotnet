// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;

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

    public static bool ContainsOnlyLetters(ReadOnlySpan<AsciiChar> value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetter(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ContainsOnlyLetters(ReadOnlySpan<byte> value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsLetter(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsAscii(ReadOnlySpan<byte> value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (!AsciiChar.IsAscii(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool SequenceEqualsCaseInsensitive(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (var i = 0; i < a.Length; i++)
        {
            if (!AsciiChar.EqualsCaseInsensitive(a[i], b[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool SequenceEqualsCaseInsensitive(ReadOnlySpan<AsciiChar> a, ReadOnlySpan<AsciiChar> b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (var i = 0; i < a.Length; i++)
        {
            if (!AsciiChar.EqualsCaseInsensitive(a[i], b[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static void ConvertToChar(ReadOnlySpan<byte> source, Span<char> output, out int charsWritten)
    {
        if (source.Length > output.Length)
        {
            ThrowHelper.ThrowInvalidOperationException("Output buffer must be at least as large as source.");
            charsWritten = 0; // unreachable
            return;
        }

        for (var i = 0; i < source.Length; i++)
        {
            output[i] = (char)source[i];
        }

        charsWritten = source.Length;
    }

    public static char[] ToCharArray(ReadOnlySpan<byte> source)
    {
        char[]? rented = null;

        try
        {
            Span<char> chars = source.Length <= StackallocThresholds.MaxCharLength
                ? stackalloc char[StackallocThresholds.MaxCharLength]
                : (rented = ArrayPool<char>.Shared.Rent(source.Length));

            ConvertToChar(source, chars, out var charsWritten);
            return chars[..charsWritten].ToArray();
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    public static ReadOnlySpan<byte> ToByteSpan(ReadOnlySpan<char> source)
    {
        byte[]? rented = null;

        try
        {
            Span<byte> chars = source.Length <= StackallocThresholds.MaxByteLength
                ? stackalloc byte[StackallocThresholds.MaxByteLength]
                : (rented = ArrayPool<byte>.Shared.Rent(source.Length));

            for (var i = 0; i < source.Length; i++)
            {
                chars[i] = (byte)source[i];
            }

            return chars[..source.Length].ToArray();
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }zx
}
