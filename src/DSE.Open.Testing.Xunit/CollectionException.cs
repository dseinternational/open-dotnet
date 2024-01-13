// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit.Sdk;

namespace DSE.Open.Testing.Xunit;

public class CollectionException : XunitException
{
    private const string DefaultMessage = "Collection assertion failed";

    public CollectionException()
        : base(DefaultMessage)
    {
    }

    public CollectionException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public CollectionException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }
}
