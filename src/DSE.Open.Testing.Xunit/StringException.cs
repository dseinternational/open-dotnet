// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Thrown when an <see cref="AssertString"/> assertion fails.
/// </summary>
public class StringException : XunitException
{
    private const string DefaultMessage = "String assertion failed";

    /// <summary>Initialises a new <see cref="StringException"/> with a default message.</summary>
    public StringException()
        : base(DefaultMessage)
    {
    }

    /// <summary>Initialises a new <see cref="StringException"/> with the specified message.</summary>
    public StringException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    /// <summary>Initialises a new <see cref="StringException"/> with a message and inner exception.</summary>
    public StringException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
