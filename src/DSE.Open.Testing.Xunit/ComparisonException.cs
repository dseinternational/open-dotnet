// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Assertion thrown when a <see cref="AssertComparison.True{T}(Func{T, bool}, T, string?)"/> fails.
/// </summary>
public class ComparisonException : XunitException
{
    private const string DefaultMessage = "Expected comparison failed";

    public ComparisonException()
        : base(DefaultMessage)
    {
    }

    public ComparisonException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public ComparisonException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
