// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class MemoryQueryExtensions
{
    public static bool All<T>(this ReadOnlyMemory<T> values, Func<T, bool> predicate)
    {
        return values.Span.All(predicate);
    }

    public static bool All<T>(this ReadOnlySpan<T> values, Func<T, bool> predicate)
    {
        Guard.IsNotNull(predicate);

        foreach (var value in values)
        {
            if (!predicate(value))
            {
                return false;
            }
        }

        return true;
    }

    public static bool Any<T>(this ReadOnlyMemory<T> values, Func<T, bool> predicate)
    {
        return values.Span.Any(predicate);
    }

    public static bool Any<T>(this ReadOnlySpan<T> values, Func<T, bool> predicate)
    {
        Guard.IsNotNull(predicate);

        foreach (var value in values)
        {
            if (predicate(value))
            {
                return true;
            }
        }

        return false;
    }

    public static bool AllAreAscii(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAscii();
    }

    public static bool AllAreAscii(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAscii);
    }

    public static bool AllAreAsciiDigit(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiDigit();
    }

    public static bool AllAreAsciiDigit(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiDigit);
    }

    public static bool AllAreAsciiLetter(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetter();
    }

    public static bool AllAreAsciiLetter(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetter);
    }

    public static bool AllAreAsciiLetterLower(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetterLower();
    }

    public static bool AllAreAsciiLetterLower(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetterLower);
    }

    public static bool AllAreAsciiLetterOrDigit(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetterOrDigit();
    }

    public static bool AllAreAsciiLetterOrDigit(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetterOrDigit);
    }

    public static bool AllAreAsciiLetterUpper(this ReadOnlyMemory<char> values)
    {
        return values.Span.AllAreAsciiLetterUpper();
    }

    public static bool AllAreAsciiLetterUpper(this ReadOnlySpan<char> values)
    {
        return values.All(char.IsAsciiLetterUpper);
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
        Guard.IsNotNull(predicate);

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
