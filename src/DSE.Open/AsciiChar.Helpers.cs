// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

// Includes code adapted from https://github.com/dotnet/runtime/blob/6e34b5d4e9b16321f37c108fea3aa7e4e04b495a/src/libraries/System.Private.CoreLib/src/System/Char.cs
// Licensed by the .NET Foundation under under the MIT license.

public partial struct AsciiChar
{
    /// <summary>Indicates whether the specified byte represents an ASCII character.</summary>
    public static bool IsAscii(byte b)
    {
        return b <= 127;
    }

    /// <summary>Indicates whether the specified signed byte represents an ASCII character.</summary>
    public static bool IsAscii(sbyte b)
    {
        return b is >= 0;
    }

    /// <summary>Indicates whether the specified value is a valid ASCII code point (0-127).</summary>
    public static bool IsAscii(int b)
    {
        return b is >= 0 and <= 127;
    }

    /// <summary>Indicates whether the specified value is a valid ASCII code point (0-127).</summary>
    public static bool IsAscii(uint b)
    {
        return b <= 127;
    }

    /// <summary>Indicates whether the specified character is in the ASCII range.</summary>
    public static bool IsAscii(char c)
    {
        return c <= 127;
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is a lowercase ASCII letter.</summary>
    public static bool IsLower(AsciiChar asciiChar)
    {
        return IsLower(asciiChar._asciiByte);
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is an uppercase ASCII letter.</summary>
    public static bool IsUpper(AsciiChar asciiChar)
    {
        return IsUpper(asciiChar._asciiByte);
    }

    /// <summary>
    /// Determines whether two ASCII bytes are equal ignoring case (using bitwise OR with 0x20).
    /// </summary>
    public static bool EqualsIgnoreCase(byte asciiByte1, byte asciiByte2)
    {
        return (asciiByte1 | 0x20) == (asciiByte2 | 0x20);
    }

    /// <summary>
    /// Compares two ASCII bytes ignoring case (using bitwise OR with 0x20).
    /// </summary>
    /// <returns>A signed integer indicating the relative order of the two values, ignoring case.</returns>
    public static int CompareToIgnoreCase(byte asciiByte1, byte asciiByte2)
    {
        return (asciiByte1 | 0x20).CompareTo(asciiByte2 | 0x20);
    }

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
    public static bool IsBetween(byte asciiByte, byte minInclusive, byte maxInclusive)
    {
        return (uint)(asciiByte - minInclusive) <= (uint)(maxInclusive - minInclusive);
    }

    /// <summary>Indicates whether a character is categorized as an ASCII digit.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is an ASCII digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range '0' through '9', inclusive.
    /// </remarks>
    public static bool IsDigit(byte asciiByte)
    {
        return IsBetween(asciiByte, (byte)'0', (byte)'9');
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is an ASCII digit ('0'-'9').</summary>
    public static bool IsDigit(AsciiChar asciiChar)
    {
        return IsDigit(asciiChar._asciiByte);
    }

    /// <summary>Indicates whether a character is categorized as an ASCII letter.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is an ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// or 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetter(byte asciiByte)
    {
        return (uint)((asciiByte | 0x20) - 'a') <= 'z' - 'a';
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is an ASCII letter ('A'-'Z' or 'a'-'z').</summary>
    public static bool IsLetter(AsciiChar asciiChar)
    {
        return IsLetter(asciiChar._asciiByte);
    }

    /// <summary>Indicates whether a character is categorized as a lowercase ASCII letter.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterLower(byte asciiByte)
    {
        return IsBetween(asciiByte, (byte)'a', (byte)'z');
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is a lowercase ASCII letter ('a'-'z').</summary>
    public static bool IsLetterLower(AsciiChar c)
    {
        return IsLetterLower(c._asciiByte);
    }

    /// <summary>Indicates whether the specified character is a lowercase ASCII letter ('a'-'z').</summary>
    public static bool IsLetterLower(char c)
    {
        return IsAscii(c) && IsLetterLower((byte)c);
    }

    /// <summary>Indicates whether a character is categorized as an uppercase ASCII letter.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsLetterUpper(byte asciiByte)
    {
        return IsBetween(asciiByte, (byte)'A', (byte)'Z');
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is an uppercase ASCII letter ('A'-'Z').</summary>
    public static bool IsLetterUpper(AsciiChar asciiChar)
    {
        return IsLetterUpper(asciiChar._asciiByte);
    }

    /// <summary>Indicates whether the specified character is an uppercase ASCII letter ('A'-'Z').</summary>
    public static bool IsLetterUpper(char c)
    {
        return IsAscii(c) && IsLetterUpper((byte)c);
    }

    /// <summary>Indicates whether a character is categorized as an ASCII letter or digit.</summary>
    /// <param name="asciiByte">The character to evaluate.</param>
    /// <returns>true if <paramref name="asciiByte"/> is an ASCII letter or digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'A' through 'Z', inclusive,
    /// 'a' through 'z', inclusive, or '0' through '9', inclusive.
    /// </remarks>
    public static bool IsLetterOrDigit(byte asciiByte)
    {
        return IsLetter(asciiByte) || IsBetween(asciiByte, (byte)'0', (byte)'9');
    }

    /// <summary>Indicates whether the specified <see cref="AsciiChar"/> is an ASCII letter or digit.</summary>
    public static bool IsLetterOrDigit(AsciiChar asciiChar)
    {
        return IsLetterOrDigit(asciiChar._asciiByte);
    }

    /// <summary>Indicates whether the specified character is an ASCII letter or digit.</summary>
    public static bool IsLetterOrDigit(char c)
    {
        return IsAscii(c) && IsLetterOrDigit((byte)c);
    }

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(byte asciiByte)
    {
        return asciiByte is >= 97 and <= 122;
    }

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(uint asciiByte)
    {
        return asciiByte is >= 97 and <= 122;
    }

    /// <summary>
    /// Indicates if the value represents a lower case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a lower case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsLower(char asciiByte)
    {
        return (uint)asciiByte is >= 97 and <= 122;
    }

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(byte asciiByte)
    {
        return asciiByte is >= 65 and <= 90;
    }

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(uint asciiByte)
    {
        return asciiByte is >= 65 and <= 90;
    }

    /// <summary>
    /// Indicates if the value represents a upper case ASCII character.
    /// </summary>
    /// <param name="asciiByte"></param>
    /// <returns>
    /// <see langword="true"/> if the value represents a upper case ASCII character, otherwise
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsUpper(char asciiByte)
    {
        return (uint)asciiByte is >= 65 and <= 90;
    }

    // https://en.wikipedia.org/wiki/Whitespace_character
    // https://developer.mozilla.org/en-US/docs/Glossary/Whitespace
    // though: https://infra.spec.whatwg.org/#ascii-whitespace

    /// <summary>
    /// Indicates whether the specified ASCII byte is a whitespace character
    /// (space, tab, line feed, vertical tab, form feed or carriage return).
    /// </summary>
    public static bool IsWhiteSpace(byte asciiByte)
    {
        return asciiByte
                   is 0x20 // space
                   or 0x09 // tab
                           // line feed, vertical tab, form feed, carriage return
               || IsBetween(asciiByte, 0x0A, 0x0D);
    }

    // https://en.cppreference.com/w/cpp/string/byte/ispunct

    /// <summary>
    /// Indicates whether the specified ASCII byte is a punctuation character
    /// (any printable ASCII character that is not a letter, digit or whitespace).
    /// </summary>
    public static bool IsPunctuation(byte asciiByte)
    {
        return IsBetween(asciiByte, 0x21, 0x2F)
               || IsBetween(asciiByte, 0x3A, 0x40)
               || IsBetween(asciiByte, 0x5B, 0x60)
               || IsBetween(asciiByte, 0x7B, 0x7E);
    }

    /// <summary>
    /// Returns the lowercase equivalent of an ASCII byte. Non-letters are returned unchanged.
    /// </summary>
    public static byte ToLower(byte asciiByte)
    {
        if (IsUpper(asciiByte))
        {
            asciiByte += 32;
        }

        return asciiByte;
    }

    /// <summary>
    /// Returns the lowercase equivalent of an ASCII character. Non-letters are returned unchanged.
    /// </summary>
    public static char ToLower(char asciiChar)
    {
        if (IsUpper(asciiChar))
        {
            asciiChar += (char)32;
        }

        return asciiChar;
    }

    /// <summary>
    /// Returns the uppercase equivalent of an ASCII byte. Non-letters are returned unchanged.
    /// </summary>
    public static byte ToUpper(byte asciiByte)
    {
        if (IsLower(asciiByte))
        {
            asciiByte -= 32;
        }

        return asciiByte;
    }

    /// <summary>
    /// Returns the uppercase equivalent of an ASCII character. Non-letters are returned unchanged.
    /// </summary>
    public static char ToUpper(char asciiChar)
    {
        if (IsLower(asciiChar))
        {
            asciiChar -= (char)32;
        }

        return asciiChar;
    }
}
