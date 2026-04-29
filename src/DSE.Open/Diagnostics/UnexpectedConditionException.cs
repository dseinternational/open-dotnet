// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Diagnostics;

/// <summary>
/// The exception that is thrown when code encounters an unexpected runtime condition that
/// indicates a defect rather than invalid input.
/// </summary>
public class UnexpectedConditionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedConditionException"/> class with
    /// a default message.
    /// </summary>
    public UnexpectedConditionException() : base(EnsureMessage(null))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedConditionException"/> class with
    /// the specified message, falling back to a default message if <paramref name="message"/> is null.
    /// </summary>
    public UnexpectedConditionException(string? message) : base(EnsureMessage(message))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedConditionException"/> class with
    /// the specified message and inner exception.
    /// </summary>
    public UnexpectedConditionException(string message, Exception innerException) : base(EnsureMessage(message), innerException)
    {
    }

    private static string EnsureMessage(string? message)
    {
        return message ?? "Encountered an unexpected condition.";
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="condition"/> is true.
    /// </summary>
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null)
    {
        if (condition)
        {
            Throw($"Encountered an unexpected condition: {message}");
        }
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is the
    /// default value of <typeparamref name="T"/>.
    /// </summary>
    public static void ThrowIfDefault<T>([NotNull] T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
        {
            Throw($"Encountered an unexpected condition: {valueName} was the default value.");
        }
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null.
    /// </summary>
    public static void ThrowIfNull([NotNull] object? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (value is null)
        {
            Throw($"Encountered an unexpected condition: {valueName} was null.");
        }
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null.
    /// </summary>
    public static void ThrowIfNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : struct
    {
        if (value is null)
        {
            Throw($"Encountered an unexpected condition: {valueName} was null.");
        }
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null
    /// or equal to the default value of <typeparamref name="T"/>.
    /// </summary>
    public static void ThrowIfNullOrDefault<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : struct
    {
        ThrowIfNull(value, valueName);
        ThrowIfDefault(value.Value, valueName);
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="value"/> is null or empty.
    /// </summary>
    public static void ThrowIfNullOrEmpty([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            Throw($"Encountered an unexpected condition: {valueName} was null or empty.");
        }
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="collection"/> contains no elements.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    public static void ThrowIfEmpty<T>(ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            Throw($"Encountered an unexpected condition: {collectionName} was empty.");
        }
    }

    /// <summary>
    /// Throws an <see cref="UnexpectedConditionException"/> if <paramref name="collection"/> contains no elements.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
    public static void ThrowIfEmpty<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (!collection.Any())
        {
            Throw($"Encountered an unexpected condition: {collectionName} was empty.");
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void Throw(string message)
    {
        throw new UnexpectedConditionException(message);
    }
}
