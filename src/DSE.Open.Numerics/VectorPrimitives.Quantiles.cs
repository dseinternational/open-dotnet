// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Divides the data into <paramref name="n"/> continuous intervals with equal
    /// probability and returns the <c>n - 1</c> cut points separating the intervals.
    /// </summary>
    /// <remarks>
    /// Uses the exclusive-linear-interpolation method (Python's
    /// <c>statistics.quantiles(method="exclusive")</c>): the i-th cut point is positioned
    /// at <c>(i/n) * (length + 1) - 1</c> in the sorted sequence, which assumes the sample
    /// is drawn from a larger continuous population.
    /// </remarks>
    public static ReadOnlySpan<T> Quantiles<T>(ReadOnlySpan<T> span, int n = 4)
        where T : struct, IFloatingPointIeee754<T>
    {
        EmptySequenceException.ThrowIfEmpty(span);
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 2);

        var sorted = span.ToArray();
        sorted.AsSpan().Sort();

        var result = new T[n - 1];
        var length = T.CreateChecked(sorted.Length + 1);
        var nT = T.CreateChecked(n);

        for (var i = 1; i < n; i++)
        {
            var h = T.CreateChecked(i) * length / nT;
            var hFloor = T.Floor(h);
            var j = int.CreateChecked(hFloor);
            var frac = h - hFloor;

            if (j < 1)
            {
                result[i - 1] = sorted[0];
            }
            else if (j >= sorted.Length)
            {
                result[i - 1] = sorted[^1];
            }
            else
            {
                result[i - 1] = sorted[j - 1] + frac * (sorted[j] - sorted[j - 1]);
            }
        }

        return result;
    }
}
