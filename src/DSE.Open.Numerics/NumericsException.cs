// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NumericsException : Exception
{
    private const string s_defaultMessage = "Numerics error.";

    public NumericsException()
        : base(s_defaultMessage)
    {
    }

    public NumericsException(string message)
        : base(message ?? s_defaultMessage)
    {
    }

    public NumericsException(string message, Exception innerException)
        : base(message ?? s_defaultMessage, innerException)
    {
    }

    public static void Throw()
    {
        throw new NumericsException(s_defaultMessage);
    }

    public static void Throw(string message)
    {
        throw new NumericsException(message);
    }

    public static void Throw(string message, Exception innerException)
    {
        throw new NumericsException(message, innerException);
    }
}
