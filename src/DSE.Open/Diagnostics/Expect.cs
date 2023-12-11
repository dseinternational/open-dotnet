// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Diagnostics;

public static class Expect
{
    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="condition"/> is not true.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="condition"/> is true.</exception>
    public static void True(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null)
    {
        UnexpectedConditionException.ThrowIf(!condition, message);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="condition"/> is not false.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="condition"/> is true.</exception>
    public static void False(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null)
    {
        True(!condition, message);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="valueName">The name of the value to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="value"/> is null.</exception>
    public static void NotNull([NotNull] object? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        UnexpectedConditionException.ThrowIfNull(value, valueName);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="valueName">The name of the value to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="value"/> is null.</exception>
    public static void NotNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : struct
    {
        UnexpectedConditionException.ThrowIfNull(value, valueName);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null or empty.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="valueName">The name of the value to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="value"/> is null.</exception>
    public static void NotNullOrEmpty([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        UnexpectedConditionException.ThrowIfNullOrEmpty(value, valueName);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="collection"/> is empty.
    /// </summary>
    /// <param name="collection">The collection to check.</param>
    /// <param name="collectionName">The name of the collection to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="collection"/> is empty.</exception>
    public static void NotEmpty<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
    {
        UnexpectedConditionException.ThrowIfEmpty(collection, collectionName);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="collection"/> is empty.
    /// </summary>
    /// <param name="collection">The collection to check.</param>
    /// <param name="collectionName">The name of the collection to include in the exception.</param>
    /// <exception cref="UnexpectedConditionException">Thrown when <paramref name="collection"/> is empty.</exception>
    public static void NotEmpty<T>(ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
    {
        UnexpectedConditionException.ThrowIfEmpty(collection, collectionName);
    }

    [DoesNotReturn]
    public static void Unreachable(string? message = null)
    {
        throw new UnreachableException(message);
    }
}
