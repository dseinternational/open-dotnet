// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Query-style extensions over <see cref="ReadOnlySpan{T}"/> and <see cref="ReadOnlyMemory{T}"/>.
/// </summary>
public static class MemoryQueryExtensions
{
    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="values"/> satisfies <paramref name="predicate"/>.
    /// Returns <see langword="true"/> when the sequence is empty.
    /// </summary>
    public static bool All<T>(this ReadOnlyMemory<T> values, Func<T, bool> predicate)
    {
        return values.Span.All(predicate);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every element of <paramref name="values"/> satisfies <paramref name="predicate"/>.
    /// Returns <see langword="true"/> when the sequence is empty.
    /// </summary>
    public static bool All<T>(this ReadOnlySpan<T> values, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (var value in values)
        {
            if (!predicate(value))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns <see langword="true"/> if any element of <paramref name="values"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    public static bool Any<T>(this ReadOnlyMemory<T> values, Func<T, bool> predicate)
    {
        return values.Span.Any(predicate);
    }

    /// <summary>
    /// Returns <see langword="true"/> if any element of <paramref name="values"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    public static bool Any<T>(this ReadOnlySpan<T> values, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (var value in values)
        {
            if (predicate(value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is ASCII.
    /// </summary>
    public static bool AllAreAscii(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAscii();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is ASCII.
    /// </summary>
    public static bool AllAreAscii(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAscii);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an ASCII decimal digit.
    /// </summary>
    public static bool AllAreAsciiDigit(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiDigit();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an ASCII decimal digit.
    /// </summary>
    public static bool AllAreAsciiDigit(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiDigit);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an ASCII letter.
    /// </summary>
    public static bool AllAreAsciiLetter(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetter();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an ASCII letter.
    /// </summary>
    public static bool AllAreAsciiLetter(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetter);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is a lower-case ASCII letter.
    /// </summary>
    public static bool AllAreAsciiLetterLower(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetterLower();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is a lower-case ASCII letter.
    /// </summary>
    public static bool AllAreAsciiLetterLower(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetterLower);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an ASCII letter or digit.
    /// </summary>
    public static bool AllAreAsciiLetterOrDigit(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetterOrDigit();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an ASCII letter or digit.
    /// </summary>
    public static bool AllAreAsciiLetterOrDigit(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetterOrDigit);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an upper-case ASCII letter.
    /// </summary>
    public static bool AllAreAsciiLetterUpper(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetterUpper();
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is an upper-case ASCII letter.
    /// </summary>
    public static bool AllAreAsciiLetterUpper(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetterUpper);
    }

    /// <summary>
    /// Returns <see langword="true"/> if every character is whitespace as classified by <see cref="char.IsWhiteSpace(char)"/>.
    /// </summary>
    public static bool AllAreWhiteSpace(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsWhiteSpace);
    }

    /// <summary>
    /// Returns the number of items matching the condition specified by <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns>The number of items matching the condition specified by <paramref name="predicate"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/></exception>
    public static int Count<T>(ReadOnlySpan<T> items, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var count = 0;

        foreach (var item in items)
        {
            if (predicate(item))
            {
                count++;
            }
        }

        return count;
    }
}
