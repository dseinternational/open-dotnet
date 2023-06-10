// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;

namespace DSE.Open;

// Includes code adapted from https://github.com/dotnet/runtime/blob/6e34b5d4e9b16321f37c108fea3aa7e4e04b495a/src/libraries/System.Private.CoreLib/src/System/Char.cs
// Licensed by the .NET Foundation under under the MIT license.

public partial struct AsciiChar
{
    public static int CompareToCaseInsenstive(byte a, byte b) => (a | 0x20).CompareTo(b | 0x20);

    /// <summary>Indicates whether an ASCII character is within the specified inclusive range.</summary>
    /// <param name="b">The character to evaluate.</param>
    /// <param name="minInclusive">The lower bound, inclusive.</param>
    /// <param name="maxInclusive">The upper bound, inclusive.</param>
    /// <returns>true if <paramref name="b"/> is within the specified range; otherwise, false.</returns>
    /// <remarks>
    /// The method does not validate that <paramref name="maxInclusive"/> is greater than or equal
    /// to <paramref name="minInclusive"/>.  If <paramref name="maxInclusive"/> is less than
    /// <paramref name="minInclusive"/>, the behavior is undefined.
    /// </remarks>
    public static bool IsBetween(byte b, byte minInclusive, byte maxInclusive) =>
        (uint)(b - minInclusive) <= (uint)(maxInclusive - minInclusive);

    /// <summary>Indicates whether a character is categorized as an ASCII digit.</summary>
    /// <param name="b">The character to evaluate.</param>
    /// <returns>true if <paramref name="b"/> is an ASCII digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range '0' through '9', inclusive.
    /// </remarks>
    public static bool IsDigit(byte b) => IsBetween(b, (byte)'0', (byte)'9');

    /// <summary>Indicates whether a character is categorized as an ASCII letter.</summary>
    /// <param name="b">The character to evaluate.</param>
    /// <returns>true if <paramref name="b"/> is an ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// or 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetter(byte b) => (uint)((b | 0x20) - 'a') <= 'z' - 'a';

    /// <summary>Indicates whether a character is categorized as a lowercase ASCII letter.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterLower(byte c) => IsBetween(c, (byte)'a', (byte)'z');

    /// <summary>Indicates whether a character is categorized as an uppercase ASCII letter.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterUpper(byte c) => IsBetween(c, (byte)'A', (byte)'Z');

    /// <summary>Indicates whether a character is categorized as an ASCII letter or digit.</summary>
    /// <param name="b">The character to evaluate.</param>
    /// <returns>true if <paramref name="b"/> is an ASCII letter or digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// 'a' through 'z', inclusive, or '0' through '9', inclusive.
    /// </remarks>
    public static bool IsLetterOrDigit(byte b)
        => IsLetter(b) | IsBetween(b, (byte)'0', (byte)'9');

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="b"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(byte b) => b is >= 97 and <= 122;

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="b"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(uint b) => b is >= 97 and <= 122;

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="b"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(char b) => (uint)b is >= 97 and <= 122;

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="b"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(byte b) => b is >= 65 and <= 90;

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="b"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(uint b) => b is >= 65 and <= 90;

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="b"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(char b) => (uint)b is >= 65 and <= 90;

    // https://en.wikipedia.org/wiki/Whitespace_character
    // https://developer.mozilla.org/en-US/docs/Glossary/Whitespace
    // though: https://infra.spec.whatwg.org/#ascii-whitespace

    public static bool IsWhiteSpace(byte b) => b
        is 0x20 // space
        or 0x09 // tab
                // line feed, vertical tab, form feed, carriage return
        || IsBetween(b, 0x0A, 0x0D);

    // https://en.cppreference.com/w/cpp/string/byte/ispunct

    public static bool IsPunctuation(byte b) =>
        IsBetween(b, 0x21, 0x2F)
        || IsBetween(b, 0x3A, 0x40)
        || IsBetween(b, 0x5B, 0x60)
        || IsBetween(b, 0x7B, 0x7E);

    public static byte ToLower(byte b)
    {
        if (IsUpper(b))
        {
            b += 32;
        }

        return b;
    }

    public static char ToLower(char asciiChar)
    {
        if (IsUpper(asciiChar))
        {
            asciiChar += (char)32;
        }

        return asciiChar;
    }

    public static byte ToUpper(byte b)
    {
        if (IsLower(b))
        {
            b -= 32;
        }

        return b;
    }

    public static char ToUpper(char asciiChar)
    {
        if (IsLower(asciiChar))
        {
            asciiChar -= (char)32;
        }

        return asciiChar;
    }

    public static void ToLowerInPlace(Span<byte> span, out int bytesWritten)
    {
#if NET8_0_OR_GREATER
        System.Text.Ascii.ToLowerInPlace(span, out bytesWritten);
#endif
        for (var i = 0; i < span.Length; i++)
        {
            span[i] = ToLower(span[i]);
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
            span[i] = ToLower(span[i]);
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
            span[i] = ToUpper(span[i]);
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
            span[i] = ToUpper(span[i]);
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

    public static ReadOnlySpan<char> ToCharSpan(ReadOnlySpan<byte> source)
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
