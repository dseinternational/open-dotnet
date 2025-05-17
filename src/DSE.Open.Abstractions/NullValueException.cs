// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// An exception that is throw when attempting to access the value of a nullable value that is null.
/// </summary>
public class NullValueException : InvalidOperationException
{
    private const string DefaultMessage = "Cannot access value as the value is null.";

    public NullValueException() : base(DefaultMessage)
    {
    }

    public NullValueException(string message) : base(message ?? DefaultMessage)
    {
    }

    public NullValueException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
