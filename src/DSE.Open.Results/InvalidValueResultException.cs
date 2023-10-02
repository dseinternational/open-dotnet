// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Results;

#pragma warning disable CA1032 // Implement standard exception constructors
public class InvalidValueResultException<T> : InvalidResultException<ValueResult<T>>
#pragma warning restore CA1032 // Implement standard exception constructors
{
    public InvalidValueResultException(ValueResult<T> result)
        : this(result, null)
    {
    }

    public InvalidValueResultException(ValueResult<T> result, string? message)
        : this(result, message, null)
    {
    }

    public InvalidValueResultException(ValueResult<T> result, string? message, Exception? innerException)
        : base(result, message, innerException)
    {
    }
}

public static class InvalidValueResultException
{
    [StackTraceHidden]
    public static void ThrowIfNotHasValue<T>(ValueResult<T> result) where T : struct
    {
        Guard.IsNotNull(result);

        if (!result.HasValue)
        {
            Throw(result);
        }
    }

    [StackTraceHidden]
    public static void ThrowIfAnyErrorNotifications<T>(ValueResult<T> result)
    {
        Guard.IsNotNull(result);

        if (result.HasAnyErrorNotifications())
        {
            Throw(result);
        }
    }

    [StackTraceHidden]
    public static void ThrowIfNotHasValueOrAnyErrorNotifications<T>(ValueResult<T> result)
    {
        Guard.IsNotNull(result);

        if (!result.HasValue || result.HasAnyErrorNotifications())
        {
            Throw(result);
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void Throw<T>(ValueResult<T> result) => throw new InvalidValueResultException<T>(result);
}
