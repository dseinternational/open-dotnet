// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// String-shape assertions — presence, and whether the characters are ASCII,
/// ASCII digits, or ASCII letters.
/// </summary>
public static class AssertString
{
    /// <summary>
    /// Asserts that <paramref name="value"/> is neither <see langword="null"/> nor empty.
    /// </summary>
    /// <exception cref="StringException"><paramref name="value"/> is
    /// <see langword="null"/> or empty.</exception>
    public static void IsNotNullOrEmpty(
        string value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new StringException($"Expected string to be not null or empty: {message}");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is neither <see langword="null"/> nor
    /// composed entirely of whitespace.
    /// </summary>
    /// <exception cref="StringException"><paramref name="value"/> is
    /// <see langword="null"/>, empty, or whitespace.</exception>
    public static void IsNotNullOrWhiteSpace(
        string value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new StringException($"Expected string to be not null or whitespace: {message}");
        }
    }

    /// <summary>
    /// Asserts that every character in <paramref name="value"/> is an ASCII character
    /// (<see cref="char.IsAscii(char)"/>). An empty span passes.
    /// </summary>
    /// <exception cref="StringException">At least one non-ASCII character was found.</exception>
    public static void IsAscii(
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (!value.All(char.IsAscii))
        {
            throw new StringException($"Expected string to contain only ASCII characters: {message}");
        }
    }

    /// <summary>
    /// Asserts that every character in <paramref name="value"/> is an ASCII digit
    /// (<see cref="char.IsAsciiDigit(char)"/>). An empty span passes.
    /// </summary>
    /// <exception cref="StringException">At least one non-digit character was found.</exception>
    public static void IsAsciiDigits(
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (!value.All(char.IsAsciiDigit))
        {
            throw new StringException($"Expected string to contain only ASCII digits: {message}");
        }
    }

    /// <summary>
    /// Asserts that every character in <paramref name="value"/> is an ASCII letter
    /// (<see cref="char.IsAsciiLetter(char)"/>). An empty span passes.
    /// </summary>
    /// <exception cref="StringException">At least one non-letter character was found.</exception>
    public static void IsAsciiLetters(
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (!value.All(char.IsAsciiLetter))
        {
            throw new StringException($"Expected string to contain only ASCII letters: {message}");
        }
    }
}
