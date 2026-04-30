// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore;

/// <summary>
/// The exception thrown when an attempt is made to write changes against a
/// <see cref="Microsoft.EntityFrameworkCore.DbContext"/> that has been configured to be read-only.
/// </summary>
public class UpdateInReadOnlyContextException : InvalidOperationException
{
    private const string DefaultMessage = "SaveChanges() or SaveChangesAsync() was called, " +
        "or a DbCommand with an INSERT, UPDATE or DELETE statement was executed on a DbContext " +
        "that is configured to be read-only.";

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInReadOnlyContextException"/> class
    /// with the default message.
    /// </summary>
    public UpdateInReadOnlyContextException() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInReadOnlyContextException"/> class
    /// with the specified message (or the default message when <paramref name="message"/> is null).
    /// </summary>
    /// <param name="message">The exception message, or <see langword="null"/> to use the default message.</param>
    public UpdateInReadOnlyContextException(string? message) : this(message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInReadOnlyContextException"/> class
    /// with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The exception message, or <see langword="null"/> to use the default message.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public UpdateInReadOnlyContextException(string? message, Exception? innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    /// <summary>
    /// Throws an <see cref="UpdateInReadOnlyContextException"/> with the default message.
    /// </summary>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw()
    {
        throw new UpdateInReadOnlyContextException();
    }

    /// <summary>
    /// Throws an <see cref="UpdateInReadOnlyContextException"/> with the specified message.
    /// </summary>
    /// <param name="message">The exception message, or <see langword="null"/> to use the default message.</param>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string? message)
    {
        throw new UpdateInReadOnlyContextException(message);
    }

    /// <summary>
    /// Throws an <see cref="UpdateInReadOnlyContextException"/> with the specified message
    /// and inner exception.
    /// </summary>
    /// <param name="message">The exception message, or <see langword="null"/> to use the default message.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string? message, Exception? innerException)
    {
        throw new UpdateInReadOnlyContextException(message, innerException);
    }
}
