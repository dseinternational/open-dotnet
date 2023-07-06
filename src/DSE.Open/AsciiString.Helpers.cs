// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;

namespace DSE.Open;

public partial struct AsciiString
{
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

    public static void ToLowerInPlace(Span<byte> span, out int bytesWritten)
    {
#if NET8_0_OR_GREATER
        System.Text.Ascii.ToLowerInPlace(span, out bytesWritten);
#endif
        for (var i = 0; i < span.Length; i++)
        {
            span[i] = AsciiChar.ToLower(span[i]);
        }

        bytesWritten = span.Length;
    }

    public static void ToLowerInPlace(Span<char> span, out int charsWritten)
    {
#if NET8_0_OR_GREATER
        System.Text.Ascii.ToLowerInPlace(span, out charsWritten);
#endif
        for (var i = 0; i < span.Length; i++)
        {
            span[i] = AsciiChar.ToLower(span[i]);
        }

        charsWritten = span.Length;
    }

    public static void ToUpperInPlace(Span<byte> span, out int bytesWritten)
    {
#if NET8_0_OR_GREATER
        System.Text.Ascii.ToUpperInPlace(span, out bytesWritten);
#endif
        for (var i = 0; i < span.Length; i++)
        {
            span[i] = AsciiChar.ToUpper(span[i]);
        }

        bytesWritten = span.Length;
    }

    public static void ToUpperInPlace(Span<char> span, out int charsWritten)
    {
#if NET8_0_OR_GREATER
        System.Text.Ascii.ToUpperInPlace(span, out charsWritten);
#endif
        for (var i = 0; i < span.Length; i++)
        {
            span[i] = AsciiChar.ToUpper(span[i]);
        }

        charsWritten = span.Length;
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
    }
}
