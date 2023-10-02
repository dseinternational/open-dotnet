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

    public InvalidResultException(TResult result) : this(result, null)
    {
    }

    public InvalidResultException(TResult result, string? message) : this(result, message, null)
    {
    }

    public InvalidResultException(TResult result, string? message, Exception? innerException)
        : base(message ?? DefaultMessage, innerException)
    {
        Guard.IsNotNull(result);
        Result = result;
    }

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
    public InvalidResultException(Result result) : base(result)
    {
    }

    public InvalidResultException(Result result, string? message) : base(result, message)
    {
    }

    public InvalidResultException(Result result, string? message, Exception? innerException)
        : base(result, message, innerException)
    {
    }

    [StackTraceHidden]
    public static void ThrowIfAnyErrorNotifications(Result result)
    {
        Guard.IsNotNull(result);

        if (result.HasAnyErrorNotifications())
        {
            Throw(result);
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void Throw(Result result) => throw new InvalidResultException(result);
}
