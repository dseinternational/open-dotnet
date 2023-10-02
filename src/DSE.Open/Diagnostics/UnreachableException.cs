// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Diagnostics;

public class UnreachableException : UnexpectedConditionException
{
    private const string s_defaultMessage = "Code that should be unreachable was executed.";

    public UnreachableException() : this(s_defaultMessage)
    {
    }

    public UnreachableException(string message) : base(message)
    {
    }

    public UnreachableException(string message, Exception innerException) : base(message, innerException)
    {
    }

    [DoesNotReturn]
    [StackTraceHidden]
    public static new void Throw(string? message = null)
        => throw new UnreachableException(message ?? "Code that should be unreachable was executed.");
}
