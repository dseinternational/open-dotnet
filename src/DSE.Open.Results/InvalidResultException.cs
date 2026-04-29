// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Results;

/// <summary>
/// An exception that is thrown when the <typeparamref name="TResult"/> returned is not valid or is
/// unexpected in the current context.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors",
    Justification = "Result is required.")]
public class InvalidResultException<TResult> : Exception
    where TResult : Result
{
    private const string DefaultMessage = "Invalid or unexpected result returned.";

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/>.
    /// </summary>
    public InvalidResultException(TResult result) : this(result, null)
    {
    }

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/> with a custom message.
    /// </summary>
    public InvalidResultException(TResult result, string? message) : this(result, message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/> with a custom message
    /// and inner exception.
    /// </summary>
    public InvalidResultException(TResult result, string? message, Exception? innerException)
        : base(message ?? DefaultMessage, innerException)
    {
        ArgumentNullException.ThrowIfNull(result);
        Result = result;
    }

    /// <summary>
    /// Gets the result that triggered this exception.
    /// </summary>
    public TResult Result { get; }
}

/// <summary>
/// An exception that is thrown when the <see cref="Result"/> returned is not valid or is unexpected
/// in the current context.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors",
    Justification = "Result is required.")]
public class InvalidResultException : InvalidResultException<Result>
{
    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/>.
    /// </summary>
    public InvalidResultException(Result result) : base(result)
    {
    }

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/> with a custom message.
    /// </summary>
    public InvalidResultException(Result result, string? message) : base(result, message)
    {
    }

    /// <summary>
    /// Initializes a new instance for the specified <paramref name="result"/> with a custom message
    /// and inner exception.
    /// </summary>
    public InvalidResultException(Result result, string? message, Exception? innerException)
        : base(result, message, innerException)
    {
    }

    /// <summary>
    /// Throws an <see cref="InvalidResultException"/> if <paramref name="result"/> carries any
    /// error-level notifications.
    /// </summary>
    [StackTraceHidden]
    public static void ThrowIfAnyErrorNotifications(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.HasAnyErrorNotifications())
        {
            Throw(result);
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void Throw(Result result)
    {
        throw new InvalidResultException(result);
    }
}
