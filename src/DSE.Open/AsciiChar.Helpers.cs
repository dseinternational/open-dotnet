// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

// Includes code adapted from https://github.com/dotnet/runtime/blob/6e34b5d4e9b16321f37c108fea3aa7e4e04b495a/src/libraries/System.Private.CoreLib/src/System/Char.cs
// Licensed by the .NET Foundation under under the MIT license.

public partial struct AsciiChar
{
    public static bool EqualsCaseInsensitive(byte asciiByte1, byte asciiByte2) => (asciiByte1 | 0x20) == (asciiByte2 | 0x20);

    public static int CompareToCaseInsensitive(byte asciiByte1, byte asciiByte2) => (asciiByte1 | 0x20).CompareTo(asciiByte2 | 0x20);

    /// <summary>Indicates whether an ASCII character is within the specified inclusive range.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <param name="minInclusive">The lower bound, inclusive.</param>
    /// <param name="maxInclusive">The upper bound, inclusive.</param>
    /// <returns>true if <paramref name="asciiByte"/> is within the specified range; otherwise, false.</returns>
    /// <remarks>
    /// The method does not validate that <paramref name="maxInclusive"/> is greater than or equal
    /// to <paramref name="minInclusive"/>.  If <paramref name="maxInclusive"/> is less than
    /// <paramref name="minInclusive"/>, the behavior is undefined.
    /// </remarks>
    public static bool IsBetween(byte asciiByte, byte minInclusive, byte maxInclusive) =>
        (uint)(asciiByte - minInclusive) <= (uint)(maxInclusive - minInclusive);

    /// <summary>Indicates whether a character is categorized as an ASCII digit.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is an ASCII digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range '0' through '9', inclusive.
    /// </remarks>
    public static bool IsDigit(byte asciiByte) => IsBetween(asciiByte, (byte)'0', (byte)'9');

    public static bool IsDigit(AsciiChar asciiChar) => IsDigit(asciiChar._asciiByte);

    /// <summary>Indicates whether a character is categorized as an ASCII letter.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is an ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// or 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetter(byte asciiByte) => (uint)((asciiByte | 0x20) - 'a') <= 'z' - 'a';

    public static bool IsLetter(AsciiChar asciiChar) => IsLetter(asciiChar._asciiByte);

    /// <summary>Indicates whether a character is categorized as a lowercase ASCII letter.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterLower(byte asciiByte) => IsBetween(asciiByte, (byte)'a', (byte)'z');

    public static bool IsLetterLower(AsciiChar c) => IsLetterLower(c._asciiByte);

    /// <summary>Indicates whether a character is categorized as an uppercase ASCII letter.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterUpper(byte asciiByte) => IsBetween(asciiByte, (byte)'A', (byte)'Z');

    public static bool IsLetterUpper(AsciiChar asciiChar) => IsLetterUpper(asciiChar._asciiByte);

    /// <summary>Indicates whether a character is categorized as an ASCII letter or digit.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is an ASCII letter or digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// 'a' through 'z', inclusive, or '0' through '9', inclusive.
    /// </remarks>
    public static bool IsLetterOrDigit(byte asciiByte)
        => IsLetter(asciiByte) | IsBetween(asciiByte, (byte)'0', (byte)'9');

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(byte asciiByte) => asciiByte is >= 97 and <= 122;

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(uint asciiByte) => asciiByte is >= 97 and <= 122;

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(char asciiByte) => (uint)asciiByte is >= 97 and <= 122;

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(byte asciiByte) => asciiByte is >= 65 and <= 90;

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(uint asciiByte) => asciiByte is >= 65 and <= 90;

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(char asciiByte) => (uint)asciiByte is >= 65 and <= 90;

    // https://en.wikipedia.org/wiki/Whitespace_character
    // https://developer.mozilla.org/en-US/docs/Glossary/Whitespace
    // though: https://infra.spec.whatwg.org/#ascii-whitespace

    public static bool IsWhiteSpace(byte asciiByte) => asciiByte
        is 0x20 // space
        or 0x09 // tab
                // line feed, vertical tab, form feed, carriage return
        || IsBetween(asciiByte, 0x0A, 0x0D);

    // https://en.cppreference.com/w/cpp/string/byte/ispunct

    public static bool IsPunctuation(byte asciiByte) =>
        IsBetween(asciiByte, 0x21, 0x2F)
        || IsBetween(asciiByte, 0x3A, 0x40)
        || IsBetween(asciiByte, 0x5B, 0x60)
        || IsBetween(asciiByte, 0x7B, 0x7E);

    public static byte ToLower(byte asciiByte)
    {
        if (IsUpper(asciiByte))
        {
            asciiByte += 32;
        }

        return asciiByte;
    }

    public static char ToLower(char asciiChar)
    {
        if (IsUpper(asciiChar))
        {
            asciiChar += (char)32;
        }

        return asciiChar;
    }

    public static byte ToUpper(byte asciiByte)
    {
        if (IsLower(asciiByte))
        {
            asciiByte -= 32;
        }

        return asciiByte;
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
