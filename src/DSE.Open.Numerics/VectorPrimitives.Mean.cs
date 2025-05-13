// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Calculates the sample arithmetic mean of a sequence. If the sequence is empty, an <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <param name="vector">The vector to calculate the mean of.</param>
    /// <typeparam name="T">The type of the elements in the vector.</typeparam>
    public static T Mean<T>(Vector<T> vector)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return Mean<T, T>(vector.AsSpan());
    }

    /// <summary>
    /// Calculates the sample arithmetic mean of a sequence. If the sequence is empty, an <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <param name="span">The vector to calculate the mean of.</param>
    /// <typeparam name="T">The type of the elements in the vector.</typeparam>
    public static T Mean<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(span);
    }

    /// <summary>
    /// Calculates the sample arithmetic mean of a sequence. If the sequence is empty, an <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <param name="span">The vector to calculate the mean of.</param>
    /// <typeparam name="T">The type of the elements in the vector.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public static TResult Mean<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(span);
        var sum = Sum<T, TResult>(span);
        return sum / TResult.CreateChecked(span.Length);
    }
}
