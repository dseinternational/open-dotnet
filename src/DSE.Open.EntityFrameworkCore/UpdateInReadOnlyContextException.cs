// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore;

public class UpdateInReadOnlyContextException : InvalidOperationException
{
    private const string DefaultMessage = "SaveChanges() or SaveChangesAsync() was called, " +
        "or a DbCommand with an INSERT, UPDATE or DELETE statement was executed on a DbContext " +
        "that is configured to be read-only.";

    public UpdateInReadOnlyContextException() : this(null)
    {
    }

    public UpdateInReadOnlyContextException(string? message) : this(message, null)
    {
    }

    public UpdateInReadOnlyContextException(string? message, Exception? innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw() => throw new UpdateInReadOnlyContextException();

    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string? message) => throw new UpdateInReadOnlyContextException(message);

    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string? message, Exception? innerException) => throw new UpdateInReadOnlyContextException(message, innerException);
}
