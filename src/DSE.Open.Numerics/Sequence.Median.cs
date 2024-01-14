// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Gets the median (middle value) value of a numeric sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <param name="median"></param>
    /// <returns></returns>
    public static TResult Median<T, TResult>(
        ReadOnlySpan<T> sequence,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        // TODO: can this be done efficiently without sorting?
        // if so, the next overload is not needed

        // https://rcoh.me/posts/linear-time-median-finding/

        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the median (middle value) value of a numeric sequence by first sorting the sequence in place.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TComparer"></typeparam>
    /// <param name="sequence">The sequence of elements to use for the calculation.</param>
    /// <param name="comparer"></param>
    /// <param name="median"></param>
    /// <returns></returns>
    public static TResult Median<T, TResult, TComparer>(
        Span<T> sequence,
        TComparer comparer,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
        where TComparer : IComparer<T>
    {
        throw new NotImplementedException();
    }
}
