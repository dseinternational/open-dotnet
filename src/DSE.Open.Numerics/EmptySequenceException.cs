// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

/// <summary>
/// Thrown when a numeric operation that requires at least one element is called
/// on an empty sequence (e.g. <c>Mean</c>, <c>Min</c>, <c>Max</c> over an empty
/// collection).
/// </summary>
public class EmptySequenceException : NumericsArgumentException
{
    private const string DefaultMessage = "Sequence contains no elements.";

    /// <summary>Creates a new exception with the default message.</summary>
    public EmptySequenceException() : base(DefaultMessage)
    {
    }

    /// <summary>Creates a new exception with the given <paramref name="message"/>.</summary>
    public EmptySequenceException(string message) : base(message ?? DefaultMessage)
    {
    }

    /// <summary>Creates a new exception with the given <paramref name="message"/> and <paramref name="innerException"/>.</summary>
    public EmptySequenceException(string message, Exception innerException) : base(message ?? DefaultMessage, innerException)
    {
    }

    /// <summary>Throws <see cref="EmptySequenceException"/> when <paramref name="sequence"/> is empty.</summary>
    public static void ThrowIfEmpty<T>(ReadOnlySpan<T> sequence)
    {
        if (sequence.IsEmpty)
        {
            throw new EmptySequenceException("Sequence contains no elements.");
        }
    }

    /// <summary>Throws <see cref="EmptySequenceException"/> when <paramref name="sequence"/> is empty.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static void ThrowIfEmpty<T>(ICollection<T> sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence);

        if (sequence.Count == 0)
        {
            throw new EmptySequenceException("Sequence contains no elements.");
        }
    }

    /// <summary>Throws <see cref="EmptySequenceException"/> with the default message.</summary>
    [DoesNotReturn]
    public static new void Throw()
    {
        throw new EmptySequenceException("Sequence contains no elements.");
    }

    /// <summary>Throws <see cref="EmptySequenceException"/> with the given <paramref name="message"/>.</summary>
    [DoesNotReturn]
    public static new void Throw(string message)
    {
        throw new EmptySequenceException(message);
    }

    /// <summary>Throws <see cref="EmptySequenceException"/> with the given <paramref name="message"/> and <paramref name="innerException"/>.</summary>
    [DoesNotReturn]
    public static new void Throw(string message, Exception innerException)
    {
        throw new EmptySequenceException(message, innerException);
    }
}
