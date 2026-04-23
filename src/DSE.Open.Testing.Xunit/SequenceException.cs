// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Thrown when an <see cref="AssertSequence"/> assertion fails.
/// </summary>
public class SequenceException : XunitException
{
    private const string DefaultMessage = "Sequence assertion failed";

    /// <summary>Initialises a new <see cref="SequenceException"/> with a default message.</summary>
    public SequenceException()
        : base(DefaultMessage)
    {
    }

    /// <summary>Initialises a new <see cref="SequenceException"/> with the specified message.</summary>
    public SequenceException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    /// <summary>Initialises a new <see cref="SequenceException"/> with a message and inner exception.</summary>
    public SequenceException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
