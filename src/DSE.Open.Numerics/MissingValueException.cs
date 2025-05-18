// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// An exception that is throw when attempting to access the value of a nullable value that is null.
/// </summary>
public class MissingValueException : InvalidOperationException
{
    private const string DefaultMessage = "Cannot access value as the value is missing.";

    public MissingValueException() : base(DefaultMessage)
    {
    }

    public MissingValueException(string message) : base(message ?? DefaultMessage)
    {
    }

    public MissingValueException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
