// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Text;

/// <summary>
/// Provides helper functions for working with strings.
/// </summary>
public static partial class StringHelper
{
    public static readonly IReadOnlyList<char> ValidWordPunctuationCharacters = ['’', '\'', '-', ' '];

    public static string? CapitalizeInvariant(string? text, CapitalizationStyle style)
    {
        return Capitalize(text, style, CultureInfo.InvariantCulture);
    }

    /// <summary>Adjusts the capitalization of a given string according the specified
    ///     <see cref="CapitalizationStyle" /> and <see cref="CultureInfo" />.</summary>
    /// <param name="text">The text to adjust the capitalization of.</param>
    /// <param name="style">The capitalization style to use.</param>
    /// <param name="cultureInfo"></param>
    [return: NotNullIfNotNull("text")]
    public static string? Capitalize(string? text, CapitalizationStyle style, CultureInfo? cultureInfo = null)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var ci = cultureInfo ?? CultureInfo.CurrentCulture;

        return style switch
        {
            CapitalizationStyle.CamelCase => ToCamelCase(text),
            CapitalizationStyle.Lowercase => text.ToLower(ci),
            CapitalizationStyle.PascalCase => ToPascalCase(text),
            CapitalizationStyle.Uppercase => text.ToUpper(ci),
            _ => text,
        };
    }

    /// <summary>Creates a <see cref="string" /> by copying all of the characters in the original
    ///     <see cref="string" /> that are alphanumeric.</summary>
    /// <param name="text">The text to parse.</param>
    /// <returns>A <see cref="string" /> including all of the characters from the original
    ///     <see cref="string" /> that are alphanumeric.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? ExtractAlphaNumeric(string? text)
    {
        return GetCharacters(text, char.IsLetterOrDigit);
    }

    /// <summary>Creates a <see cref="string" /> by copying all of the characters in the original
    ///     <see cref="string" /> that are alphanumeric or whitespace.</summary>
    /// <param name="text">The text to parse.</param>
    /// <returns>A <see cref="string" /> including all of the characters from the original
    ///     <see cref="string" /> that are alphanumeric or whitespace.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? ExtractLetterOrDigitOrWhitespace(string? text)
    {
        return GetCharacters(text, c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
    }

    /// <summary>
    ///     Removes any character that is not a numeric digit (0-9) from the supplied
    ///     character array.
    /// </summary>
    /// <param name="text">A character array containing the text to remove the digits from.</param>
    /// <returns>The input character array, with non-digit characters removed.</returns>
    /// <remarks>
    ///     This method removes a character if it is not a radix-10 digit. Valid digits are
    ///     members of the DecimalDigitNumber UnicodeCategory (
    ///     <see cref="UnicodeCategory" />
    ///     Enumeration).
    /// </remarks>
    [return: NotNullIfNotNull("text")]
    public static string? ExtractDigits(string? text)
    {
        return GetCharacters(text, char.IsDigit);
    }

    /// <summary>Removes any character that is not a letter from the supplied character array.</summary>
    /// <param name="text">A character array containing the text.</param>
    /// <returns>The input character array, with non-letter characters removed.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? ExtractLetters(string? text)
    {
        return GetCharacters(text, char.IsLetter);
    }

    [return: NotNullIfNotNull("text")]
    private static string? GetCharacters(string? text, Predicate<char> predicate)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        char[]? rentedBuffer = null;

        try
        {
            Span<char> buffer = MemoryThresholds.CanStackalloc<char>(text.Length)
                ? (rentedBuffer = ArrayPool<char>.Shared.Rent(text.Length))
                : stackalloc char[text.Length];

            var index = 0;

            foreach (var @char in text)
            {
                if (predicate(@char))
                {
                    buffer[index++] = @char;
                }
            }

            return buffer[..index].ToString();
        }
        finally
        {
            if (rentedBuffer is not null)
            {
                ArrayPool<char>.Shared.Return(rentedBuffer);
            }
        }
    }

    [return: NotNullIfNotNull("text")]
    public static string? ExtractLettersWithDashesForWhitespace(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var rented = SpanOwner<char>.Empty;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(text.Length)
            ? stackalloc char[text.Length]
            : (rented = SpanOwner<char>.Allocate(text.Length)).Span;

        using (rented)
        {
            var index = 0;

            foreach (var @char in text)
            {
                if (char.IsLetter(@char))
                {
                    buffer[index++] = @char;
                }
                else if (char.IsWhiteSpace(@char))
                {
                    buffer[index++] = '-';
                }
            }

            return buffer[..index].ToString();
        }
    }

    public static IEnumerable<string> ExtractWords(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return [];
        }

        var textBlocks = SplitOnWhitespace(text);
        return textBlocks
            .Select(s => RemovePunctuation(s, ValidWordPunctuationCharacters))
            .Where(w => !string.IsNullOrWhiteSpace(w));
    }

    /// <summary>
    ///     Retrieves a substring from the supplied string. The substring starts at the last
    ///     occurence of the specified string and continues to the end of the supplied string.
    /// </summary>
    /// <param name="original">The string from which to retrieve the substring.</param>
    /// <param name="marker">The string after which the substring can be found</param>
    /// <param name="comparison"></param>
    /// <returns>
    ///     If the marker is found then a string representing the substring, otherwise
    ///     <see cref="string.Empty" />.
    /// </returns>
    public static string GetSubstringAfterLast(string original, string marker,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        Guard.IsNotNull(original);
        Guard.IsNotNull(marker);

        var pos = original.LastIndexOf(marker, comparison);
        var rtn = pos > 0 ? original[(pos + 1)..] : string.Empty;
        return rtn;
    }

    /// <summary>
    ///     Retrieves a substring from the supplied string. The substring starts at the last
    ///     occurence of the specified character and continues to the end of the supplied string.
    /// </summary>
    /// <param name="original">The string from which to retrieve the substring.</param>
    /// <param name="marker">The character after which the substring can be found.</param>
    /// <returns>
    ///     If the marker is found then a string representing the substring, otherwise
    ///     <see cref="string.Empty" />.
    /// </returns>
    public static string GetSubstringAfterLast(string original, char marker)
    {
        Guard.IsNotNull(original);

        var pos = original.LastIndexOf(marker);
        var rtn = pos > 0 ? original[(pos + 1)..] : string.Empty;
        return rtn;
    }

    /// <summary>
    ///     Retrieves a substring from the supplied string. The substring starts at the
    ///     beginning of the string and ends immediately before the first occurence of the
    ///     specified character.
    /// </summary>
    /// <param name="original">The string from which to retrieve the substring.</param>
    /// <param name="marker">The character before which the substring can be found</param>
    /// <param name="comparison"></param>
    /// <returns>
    ///     If the marker is found then a string representing the substring, otherwise
    ///     <see cref="string.Empty" />.
    /// </returns>
    public static string GetSubstringBeforeFirst(string original, char marker,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        Guard.IsNotNull(original);

        var pos = original.IndexOf(marker, comparison);
        var rtn = pos > 0 ? original[..pos] : string.Empty;
        return rtn;
    }

    /// <summary>
    ///     Retrieves a substring from the supplied string. The substring starts at the
    ///     beginning of the string and ends immediately before the first occurence of the
    ///     specified string.
    /// </summary>
    /// <param name="original">The string from which to retrieve the substring.</param>
    /// <param name="marker">The string before which the substring can be found</param>
    /// <param name="comparison"></param>
    /// <returns>
    ///     If the marker is found then a string representing the substring, otherwise
    ///     <see cref="string.Empty" />.
    /// </returns>
    public static string GetSubstringBeforeFirst(string original, string marker,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        Guard.IsNotNull(original);
        Guard.IsNotNull(marker);

        var pos = original.IndexOf(marker, comparison);
        var rtn = pos > 0 ? original[..pos] : string.Empty;
        return rtn;
    }

    /// <summary>
    ///     Retrieves a substring from the supplied string. The substring starts at the
    ///     beginning of the string and ends immediately before the last occurence of the
    ///     specified string.
    /// </summary>
    /// <param name="original">The string from which to retrieve the substring.</param>
    /// <param name="marker">The string before which the substring can be found</param>
    /// <param name="comparison"></param>
    /// <returns>
    ///     If the marker is found then a string representing the substring, otherwise
    ///     <see cref="string.Empty" />.
    /// </returns>
    public static string GetSubstringBeforeLast(string original, string marker,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        Guard.IsNotNull(original);
        Guard.IsNotNull(marker);

        var pos = original.LastIndexOf(marker, comparison);
        var rtn = pos > 0 ? original[..pos] : string.Empty;
        return rtn;
    }

    /// <summary>
    ///     Retrieves a substring from the supplied string. The substring starts at the
    ///     beginning of the string and ends immediately before the last occurence of the
    ///     specified character.
    /// </summary>
    /// <param name="original">The string from which to retrieve the substring.</param>
    /// <param name="marker">The character before which the substring can be found</param>
    /// <returns>
    ///     If the marker is found then a string representing the substring, otherwise
    ///     <see cref="string.Empty" />.
    /// </returns>
    public static string GetSubstringBeforeLast(string original, char marker)
    {
        Guard.IsNotNull(original);

        var pos = original.LastIndexOf(marker);
        var rtn = pos > 0 ? original[..pos] : string.Empty;
        return rtn;
    }

    /// <summary>
    ///     Removes any character that is categorised as a punctuation mark from the
    ///     supplied character array and returns the resulting string.
    /// </summary>
    /// <param name="text">A character array with the characters to be scanned.</param>
    /// <param name="exceptions">Characters that should not be removed.</param>
    /// <returns>A string containing the characters with punctuation characters removed.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? RemovePunctuation(string? text, IEnumerable<char>? exceptions = null)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        char[]? rented = null;

        var result = MemoryThresholds.CanStackalloc<char>(text.Length)
            ? stackalloc char[text.Length]
            : [];

        try
        {
            var index = 0;

            var exceptionsList = exceptions?.ToList();
            var exceptionsListIsNotNullOrEmpty = exceptionsList is not null && exceptionsList.Count > 0;

            // Keeping track of whether any changes have been made allows us to avoid allocating a
            // new string if no changes are made.
            var hasChanged = false;

            foreach (var c in text)
            {
                // Nothing mutates the `exceptionsList` so we can hoist the null check out of the loop,
                // but a null suppression is required.
                if (!char.IsPunctuation(c) || (exceptionsListIsNotNullOrEmpty && exceptionsList!.Contains(c)))
                {
                    if (hasChanged)
                    {
                        result[index] = c;
                    }

                    index++;
                }
                else if (!hasChanged)
                {
                    hasChanged = true;

                    if (result.IsEmpty)
                    {
                        // Was too big to stack allocate, rent from pool.
                        result = rented = ArrayPool<char>.Shared.Rent(text.Length);
                    }

                    text.AsSpan(0, index).CopyTo(result);
                }
            }

            if (!hasChanged)
            {
                return text;
            }

            Debug.Assert(!result.IsEmpty);

            return new string(result[..index]);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    /// <summary>Removes all spaces from a string.</summary>
    /// <param name="text">The string from which to remove the spaces.</param>
    /// <returns>A copy of the input string, with spaces removed.</returns>
    [return: NotNullIfNotNull("text")]
    public static string? RemoveWhitespace(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var result = new char[text.Length];
        var index = 0;

        foreach (var t in text.Where(t => !char.IsWhiteSpace(t)))
        {
            result[index++] = t;
        }

        return new string(result, 0, index);
    }

    public static IEnumerable<string> SplitOnWhitespace(string? text)
    {
        return string.IsNullOrEmpty(text)
            ? Enumerable.Empty<string>()
            : text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
    }

    [return: NotNullIfNotNull("text")]
    public static string? ToCamelCase(string? text, CultureInfo? cultureInfo = null)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var ci = cultureInfo ?? CultureInfo.CurrentCulture;

        if (text.All(char.IsUpper))
        {
            text = text.ToLower(ci);
            return char.ToLower(text[0], ci) + text.Remove(0, 1);
        }

        return char.IsUpper(text, 0) ? char.ToLower(text[0], ci) + text.Remove(0, 1) : text;
    }

    [return: NotNullIfNotNull("text")]
    public static string? ToPascalCase(string? text, CultureInfo? cultureInfo = null)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var ci = cultureInfo ?? CultureInfo.CurrentCulture;

        if (text.All(char.IsUpper))
        {
            text = text.ToLower(ci);
            return char.ToUpper(text[0], ci) + text.Remove(0, 1);
        }

        return char.IsLower(text, 0) ? char.ToUpper(text[0], ci) + text.Remove(0, 1) : text;
    }

    internal enum SeparatedCaseState
    {
        Start,
        Lower,
        Upper,
        NewWord
    }

    [return: NotNullIfNotNull("name")]
    public static string? ToSlugCase(string? name)
    {
        return ToSeparatedCase(name, '-');
    }

    [return: NotNullIfNotNull("name")]
    public static string? ToSnakeCase(string? name)
    {
        return ToSeparatedCase(name, '_');
    }

    [return: NotNullIfNotNull("name")]
    internal static string? ToSeparatedCase(string? name, char separator)
    {
        if (name is null || string.IsNullOrEmpty(name))
        {
            return name;
        }

        var buffer = ArrayPool<char>.Shared.Rent((name.Length * 2) - 1);
        var pos = 0;

        var state = SeparatedCaseState.Start;

        var nameSpan = name.AsSpan();

        for (var i = 0; i < nameSpan.Length; i++)
        {
            if (nameSpan[i] == ' ')
            {
                if (state != SeparatedCaseState.Start)
                {
                    state = SeparatedCaseState.NewWord;
                }
            }
            else if (char.IsUpper(nameSpan[i]))
            {
                switch (state)
                {
                    case SeparatedCaseState.Upper:
                        var hasNext = i + 1 < nameSpan.Length;
                        if (i > 0 && hasNext)
                        {
                            var nextChar = nameSpan[i + 1];
                            if (!char.IsUpper(nextChar) && nextChar != separator)
                            {
                                buffer[pos++] = separator;
                            }
                        }

                        break;
                    case SeparatedCaseState.Lower:
                    case SeparatedCaseState.NewWord:
                        buffer[pos++] = separator;
                        break;
                }

                buffer[pos++] = char.ToLowerInvariant(nameSpan[i]);
                state = SeparatedCaseState.Upper;
            }
            else if (nameSpan[i] == separator)
            {
                buffer[pos++] = separator;
                state = SeparatedCaseState.Start;
            }
            else
            {
                if (state == SeparatedCaseState.NewWord)
                {
                    buffer[pos++] = separator;
                }

                buffer[pos++] = nameSpan[i];
                state = SeparatedCaseState.Lower;
            }
        }

        var result = new string(buffer, 0, pos);
        ArrayPool<char>.Shared.Return(buffer);
        return result;
    }

    /// <summary>
    /// Returns a string converted to title casing using the current culture.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns></returns>
    public static string ToTitleCase(this string value)
    {
        return value.ToTitleCase(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a string converted to title casing using the invariant culture.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns></returns>
    public static string ToTitleCaseInvariant(this string value)
    {
        return value.ToTitleCase(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns a string converted to title casing using the specified culture.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string ToTitleCase(this string value, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var chars = value.ToCharArray();
        var nextIsUpper = true;
        for (var i = 0; i < chars.Length; i++)
        {
            if (char.IsWhiteSpace(chars[i]))
            {
                nextIsUpper = true;
            }
            else
            {
                if (nextIsUpper)
                {
                    chars[i] = char.ToUpper(chars[i], culture);
                    nextIsUpper = false;
                }
                else
                {
                    chars[i] = char.ToLower(chars[i], culture);
                }
            }
        }

        return new string(chars);
    }
}
