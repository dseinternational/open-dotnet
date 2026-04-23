// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Thrown when an <see cref="AssertComparison"/> assertion fails.
/// </summary>
public class ComparisonException : XunitException
{
    private const string DefaultMessage = "Expected comparison failed";

    /// <summary>Initialises a new <see cref="ComparisonException"/> with a default message.</summary>
    public ComparisonException()
        : base(DefaultMessage)
    {
    }

    /// <summary>Initialises a new <see cref="ComparisonException"/> with the specified message.</summary>
    public ComparisonException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    /// <summary>Initialises a new <see cref="ComparisonException"/> with a message and inner exception.</summary>
    public ComparisonException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
