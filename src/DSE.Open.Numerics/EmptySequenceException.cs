// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class EmptySequenceException : NumericsException
{
    private const string s_defaultMessage = "Sequence contains no elements.";

    public EmptySequenceException() : base(s_defaultMessage)
    {
    }

    public EmptySequenceException(string message) : base(message ?? s_defaultMessage)
    {
    }

    public EmptySequenceException(string message, Exception innerException) : base(message ?? s_defaultMessage, innerException)
    {
    }

    public static void ThrowIfEmpty<T>(ReadOnlySpan<T> sequence)
    {
        if (sequence.IsEmpty)
        {
            throw new EmptySequenceException("Sequence contains no elements.");
        }
    }

    public static void ThrowIfEmpty<T>(ICollection<T> sequence)
    {
        Guard.IsNotNull(sequence);

        if (sequence.Count == 0)
        {
            throw new EmptySequenceException("Sequence contains no elements.");
        }
    }

    public static new void Throw()
    {
        throw new EmptySequenceException("Sequence contains no elements.");
    }

    public static new void Throw(string message)
    {
        throw new EmptySequenceException(message);
    }

    public static new void Throw(string message, Exception innerException)
    {
        throw new EmptySequenceException(message, innerException);
    }
}
