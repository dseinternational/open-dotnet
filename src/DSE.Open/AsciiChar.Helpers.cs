// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;

namespace DSE.Open;

// Includes code adapted from https://github.com/dotnet/runtime/blob/6e34b5d4e9b16321f37c108fea3aa7e4e04b495a/src/libraries/System.Private.CoreLib/src/System/Char.cs
// Licensed by the .NET Foundation under under the MIT license.

public partial struct AsciiChar
{
    public static bool EqualsCaseInsensitive(byte a, byte b) => (a | 0x20) == (b | 0x20);

    public static int CompareToCaseInsensitive(byte a, byte b) => (a | 0x20).CompareTo(b | 0x20);

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

    public static bool IsDigit(AsciiChar b) => IsDigit(b._c);

    /// <summary>Indicates whether a character is categorized as an ASCII letter.</summary>
    /// <param name="b">The character to evaluate.</param>
    /// <returns>true if <paramref name="b"/> is an ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// or 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetter(byte b) => (uint)((b | 0x20) - 'a') <= 'z' - 'a';

    public static bool IsLetter(AsciiChar b) => IsLetter(b._c);

    /// <summary>Indicates whether a character is categorized as a lowercase ASCII letter.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterLower(byte c) => IsBetween(c, (byte)'a', (byte)'z');

    public static bool IsLetterLower(AsciiChar b) => IsLetterLower(b._c);

    /// <summary>Indicates whether a character is categorized as an uppercase ASCII letter.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterUpper(byte c) => IsBetween(c, (byte)'A', (byte)'Z');

    public static bool IsLetterUpper(AsciiChar b) => IsLetterUpper(b._c);

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
}
