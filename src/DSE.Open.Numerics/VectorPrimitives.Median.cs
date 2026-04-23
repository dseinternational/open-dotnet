// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the median (middle value) of a non-empty sequence.
    /// </summary>
    /// <remarks>
    /// The input is copied before sorting so the original span is not mutated. Use the
    /// destructive <see cref="Median{T, TResult, TComparer}(Span{T}, TComparer, MedianMethod)"/>
    /// overload to sort in place and avoid the copy.
    /// </remarks>
    public static T Median<T>(
        ReadOnlySpan<T> span,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumber<T>
    {
        return Median<T, T>(span, median);
    }

    /// <summary>
    /// Gets the median (middle value) of a non-empty sequence, accumulating into
    /// <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult Median<T, TResult>(
        ReadOnlySpan<T> span,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumber<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(span);

        var buffer = CreateUninitializedArray<T>(span.Length);
        span.CopyTo(buffer);
        return Median<T, TResult, Comparer<T>>(buffer, Comparer<T>.Default, median);
    }

    /// <summary>
    /// Gets the median (middle value) of a non-empty sequence by sorting <paramref name="span"/>
    /// in place.
    /// </summary>
    public static TResult Median<T, TResult, TComparer>(
        Span<T> span,
        TComparer comparer,
        MedianMethod median = MedianMethod.MeanOfMiddleTwo)
        where T : struct, INumber<T>
        where TResult : struct, INumberBase<TResult>
        where TComparer : IComparer<T>
    {
        EmptySequenceException.ThrowIfEmpty(span);

        span.Sort(comparer);

        var n = span.Length;
        var mid = n / 2;

        if ((n & 1) == 1)
        {
            return TResult.CreateChecked(span[mid]);
        }

        return median switch
        {
            MedianMethod.SmallerOfMiddleTwo => TResult.CreateChecked(span[mid - 1]),
            MedianMethod.LargerOfMiddleTwo => TResult.CreateChecked(span[mid]),
            _ => (TResult.CreateChecked(span[mid - 1]) + TResult.CreateChecked(span[mid]))
                / (TResult.One + TResult.One),
        };
    }
}
