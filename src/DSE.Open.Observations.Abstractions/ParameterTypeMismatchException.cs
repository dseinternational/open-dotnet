// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public class ParameterTypeMismatchException : InvalidOperationException
{
    public ParameterTypeMismatchException()
    {
    }

    public ParameterTypeMismatchException(string message) : base(message)
    {
    }

    public ParameterTypeMismatchException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
