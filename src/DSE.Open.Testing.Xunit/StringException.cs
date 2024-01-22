// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

public class StringException : XunitException
{
    private const string DefaultMessage = "String assertion failed";

    public StringException()
        : base(DefaultMessage)
    {
    }

    public StringException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public StringException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
