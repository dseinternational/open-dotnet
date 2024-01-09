// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Return the sample arithmetic mean of a sequence. If the sequence is empty, an
    /// <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    public static T Mean<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(sequence);
    }

    public static TResult Mean<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);
        var sum = Sum<T, TResult>(sequence);
        return sum / TResult.CreateChecked(sequence.Length);
    }

    /// <summary>
    /// Return the sample arithmetic mean of a sequence. If the sequence is empty, an
    /// <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    public static T Mean<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(sequence);
    }

    public static TResult Mean<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        // TODO: consider accumulating method to avoid TResult overflow
        var sum = Sum<T, TResult>(sequence, out var size);

        if (size == 0)
        {
            EmptySequenceException.Throw();
        }

        return sum / TResult.CreateChecked(size);
    }
}
