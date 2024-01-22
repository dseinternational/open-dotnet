// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static class AssertString
{
    public static void IsNotNullOrEmpty(
        string value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new StringException($"Expected string to be not null or empty: {message}");
        }
    }

    public static void IsNotNullOrWhiteSpace(
        string value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new StringException($"Expected string to be not null or whitespace: {message}");
        }
    }

    public static void IsAscii(
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (!value.All(char.IsAscii))
        {
            throw new StringException($"Expected string to contain only ASCII characters: {message}");
        }
    }

    public static void IsAsciiDigits(
        ReadOnlySpan<char> value,
        [CallerArgumentExpression(nameof(value))] string? message = default)
    {
        if (!value.All(char.IsAsciiDigit))
        {
            throw new StringException($"Expected string to contain only ASCII digits: {message}");
        }
    }

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
