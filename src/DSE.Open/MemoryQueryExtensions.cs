// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

public static class MemoryQueryExtensions
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All<T>(this ReadOnlyMemory<T> values, Func<T, bool> predicate) => values.Span.All(predicate);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this ReadOnlyMemory<T> values, Func<T, bool> predicate) => values.Span.Any(predicate);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAscii(this ReadOnlyMemory<char> values) => values.Span.AllAreAscii();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAscii(this ReadOnlySpan<char> values) => values.All(char.IsAscii);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiDigit(this ReadOnlyMemory<char> values) => values.Span.AllAreAsciiDigit();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiDigit(this ReadOnlySpan<char> values) => values.All(char.IsAsciiDigit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetter(this ReadOnlyMemory<char> values) => values.Span.AllAreAsciiLetter();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetter(this ReadOnlySpan<char> values) => values.All(char.IsAsciiLetter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetterLower(this ReadOnlyMemory<char> values) => values.Span.AllAreAsciiLetterLower();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetterLower(this ReadOnlySpan<char> values) => values.All(char.IsAsciiLetterLower);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetterOrDigit(this ReadOnlyMemory<char> values) => values.Span.AllAreAsciiLetterOrDigit();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetterOrDigit(this ReadOnlySpan<char> values) => values.All(char.IsAsciiLetterOrDigit);

    public static bool AllAreAsciiLetterUpper(this ReadOnlyMemory<char> values) => values.Span.AllAreAsciiLetterUpper();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllAreAsciiLetterUpper(this ReadOnlySpan<char> values) => values.All(char.IsAsciiLetterUpper);

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
