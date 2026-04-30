// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Results;

/// <summary>
/// An exception that is thrown when a <see cref="ValueResult{T}"/> is not valid or is unexpected
/// in the current context.
/// </summary>
/// <typeparam name="T">The type of the carried value.</typeparam>
#pragma warning disable CA1032 // Implement standard exception constructors
public class InvalidValueResultException<T> : InvalidResultException<ValueResult<T>>
#pragma warning restore CA1032 // Implement standard exception constructors
{
    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/>.
    /// </summary>
    public InvalidValueResultException(ValueResult<T> result)
        : this(result, null)
    {
    }

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/> with a custom message.
    /// </summary>
    public InvalidValueResultException(ValueResult<T> result, string? message)
        : this(result, message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/> with a custom message
    /// and inner exception.
    /// </summary>
    public InvalidValueResultException(ValueResult<T> result, string? message, Exception? innerException)
        : base(result, message, innerException)
    {
    }
}

/// <summary>
/// Provides static guard helpers that throw <see cref="InvalidValueResultException{T}"/>.
/// </summary>
public static class InvalidValueResultException
{
    /// <summary>
    /// Throws <see cref="InvalidValueResultException{T}"/> if <paramref name="result"/> does not
    /// carry a value.
    /// </summary>
    [StackTraceHidden]
    public static void ThrowIfNotHasValue<T>(ValueResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (!result.HasValue)
        {
            Throw(result);
        }
    }

    /// <summary>
    /// Throws <see cref="InvalidValueResultException{T}"/> if <paramref name="result"/> carries any
    /// error-level notifications.
    /// </summary>
    [StackTraceHidden]
    public static void ThrowIfAnyErrorNotifications<T>(ValueResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.HasAnyErrorNotifications())
        {
            Throw(result);
        }
    }

    /// <summary>
    /// Throws <see cref="InvalidValueResultException{T}"/> if <paramref name="result"/> does not
    /// carry a value, or if it carries any error-level notifications.
    /// </summary>
    [StackTraceHidden]
    public static void ThrowIfNotHasValueOrAnyErrorNotifications<T>(ValueResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (!result.HasValue || result.HasAnyErrorNotifications())
        {
            Throw(result);
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void Throw<T>(ValueResult<T> result)
    {
        throw new InvalidValueResultException<T>(result);
    }
}
