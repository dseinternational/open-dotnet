// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

public class SequenceException : XunitException
{
    private const string DefaultMessage = "Sequence assertion failed";

    public SequenceException()
        : base(DefaultMessage)
    {
    }

    public SequenceException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public SequenceException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
