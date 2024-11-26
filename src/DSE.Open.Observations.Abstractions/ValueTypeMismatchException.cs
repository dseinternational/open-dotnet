// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public class ValueTypeMismatchException : InvalidOperationException
{
    public ValueTypeMismatchException()
    {
    }

    public ValueTypeMismatchException(string message) : base(message)
    {
    }

    public ValueTypeMismatchException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
