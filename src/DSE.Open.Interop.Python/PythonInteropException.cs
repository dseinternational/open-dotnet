// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Interop.Python;

public sealed class PythonInteropException : Exception
{
    public PythonInteropException(string message) : base(message)
    {
    }

    public PythonInteropException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
