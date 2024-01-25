// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class StringExtensions
{
    /// <summary>
    /// Determines if the string contains only Unicode characters classified
    /// as whitespace, or, optionally, is <see langword="null"/> or empty.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="allowNullOrEmpty"></param>
    /// <returns>
    /// <see langword="true"/> if the sequence of characters contains only Unicode characters
    /// classified as whitespace or if <paramref name="allowNullOrEmpty"/> is <see langword="true"/>
    /// and the sequence is <see langword="null"/> or empty; otherwise <see langword="false"/>.
    /// </returns>
    public static bool ContainsOnlyWhitespace(this string? value, bool allowNullOrEmpty = true)
    {
        return value.AsSpan().ContainsOnlyWhitespace(allowNullOrEmpty);
    }

    /// <summary>
    /// Determines if the string contains only Unicode characters classified
    /// as a Unicode letters, or, optionally, is <see langword="null"/> or empty.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="allowNullOrEmpty"></param>
    /// <returns>
    /// <see langword="true"/> if the sequence of characters contains only Unicode characters
    /// classified as Unicode letters or if <paramref name="allowNullOrEmpty"/> is <see langword="true"/>
    /// and the sequence is <see langword="null"/> or empty; otherwise <see langword="false"/>.
    /// </returns>
    public static bool ContainsOnlyLetters(this string? value, bool allowNullOrEmpty = true)
    {
        return value.AsSpan().ContainsOnlyLetters(allowNullOrEmpty);
    }

    /// <summary>
    /// Determines if the string contains only Unicode characters classified
    /// as a Unicode digits, or, optionally, is <see langword="null"/> or empty.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="allowNullOrEmpty"></param>
    /// <returns>
    /// <see langword="true"/> if the sequence of characters contains only Unicode characters
    /// classified as Unicode digits or if <paramref name="allowNullOrEmpty"/> is <see langword="true"/>
    /// and the sequence is <see langword="null"/> or empty; otherwise <see langword="false"/>.
    /// </returns>
    public static bool ContainsOnlyDigits(this string? value, bool allowNullOrEmpty = true)
    {
        return value.AsSpan().ContainsOnlyDigits(allowNullOrEmpty);
    }

    /// <summary>
    /// Determines if the string contains only Unicode characters classified
    /// as a Unicode letters or digits, or, optionally, is <see langword="null"/> or empty.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="allowNullOrEmpty"></param>
    /// <returns>
    /// <see langword="true"/> if the sequence of characters contains only Unicode characters
    /// classified as Unicode letters or digits or if <paramref name="allowNullOrEmpty"/>
    /// is <see langword="true"/> and the sequence is <see langword="null"/> or empty;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool ContainsOnlyLettersOrDigits(this string? value, bool allowNullOrEmpty = true)
    {
        return value.AsSpan().ContainsOnlyLettersOrDigits(allowNullOrEmpty);
    }
}
