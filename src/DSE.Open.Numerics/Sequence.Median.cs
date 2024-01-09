// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Gets the median (middle value) value of a numeric sequence.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TAcc"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="median"></param>
    /// <returns></returns>
    public static TAcc Median<T, TAcc>(
        ReadOnlySpan<T> sequence,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        // TODO: can this be done efficiently without sorting?
        // if so, the next overload is not needed

        // https://rcoh.me/posts/linear-time-median-finding/

        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the median (middle value) value of a numeric sequence by first sorting the sequence in place.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TAcc"></typeparam>
    /// <typeparam name="TComparer"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="comparer"></param>
    /// <param name="median"></param>
    /// <returns></returns>
    public static TAcc Median<T, TAcc, TComparer>(
        Span<T> sequence,
        TComparer comparer,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
        where TComparer : IComparer<T>
    {
        throw new NotImplementedException();
    }
}
