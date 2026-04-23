// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the most frequent value (the mode) of a non-empty sequence. Ties are broken
    /// by returning the first value to reach the peak frequency as the input is scanned
    /// left to right.
    /// </summary>
    public static T Mode<T>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
    {
        return Mode<T, T>(span);
    }

    /// <summary>
    /// Gets the most frequent value (the mode) of a non-empty sequence, projected to
    /// <typeparamref name="TResult"/>.
    /// </summary>
    public static TResult Mode<T, TResult>(ReadOnlySpan<T> span)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(span);

        var counts = new Dictionary<T, int>(span.Length);
        var mode = span[0];
        var modeCount = 0;

        for (var i = 0; i < span.Length; i++)
        {
            var value = span[i];
            counts.TryGetValue(value, out var count);
            count++;
            counts[value] = count;

            if (count > modeCount)
            {
                modeCount = count;
                mode = value;
            }
        }

        return TResult.CreateChecked(mode);
    }
}
