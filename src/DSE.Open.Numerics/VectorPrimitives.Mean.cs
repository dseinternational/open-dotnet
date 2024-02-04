// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Return the sample arithmetic mean of a sequence. If the sequence is empty, an
    /// <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="span">The sequence of elements to use for the calculation.</param>
    /// <returns></returns>
    public static T Mean<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(span);
    }

    public static TResult Mean<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(span);
        var sum = Sum<T, TResult>(span);
        return sum / TResult.CreateChecked(span.Length);
    }
}
