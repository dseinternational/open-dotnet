// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Diagnostics;

public class UnexpectedConditionException : Exception
{
    public UnexpectedConditionException() : base(EnsureMessage(null))
    {
    }

    public UnexpectedConditionException(string? message) : base(EnsureMessage(message))
    {
    }

    public UnexpectedConditionException(string message, Exception innerException) : base(EnsureMessage(message), innerException)
    {
    }

    private static string EnsureMessage(string? message) => message ?? "Encountered an unexpected condition.";

    public static void ThrowIf(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null)
    {
        if (condition)
        {
            Throw($"Encountered an unexpected condition: {message}");
        }
    }

    public static void ThrowIfNull([NotNull] object? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (value is null)
        {
            Throw($"Encountered an unexpected condition: {valueName} was null.");
        }
    }

    public static void ThrowIfNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : struct
    {
        if (value is null)
        {
            Throw($"Encountered an unexpected condition: {valueName} was null.");
        }
    }

    public static void ThrowIfEmpty<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
    {
        Guard.IsNotNull(collection);

        if (!collection.Any())
        {
            Throw($"Encountered an unexpected condition: {collectionName} was empty.");
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void Throw(string message) => throw new UnexpectedConditionException(message);
}
