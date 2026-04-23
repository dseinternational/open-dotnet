// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the harmonic mean of a non-empty sequence: <c>n / Σ(1/xᵢ)</c>.
    /// </summary>
    /// <remarks>
    /// The harmonic mean is defined only for positive real numbers; a zero element produces
    /// positive infinity in the reciprocal sum and therefore a result of zero.
    /// </remarks>
    public static T HarmonicMean<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return HarmonicMean<T, T>(sequence);
    }

    /// <summary>
    /// Gets the harmonic mean of a non-empty sequence, accumulating into
    /// <typeparamref name="TResult"/>: <c>n / Σ(1/xᵢ)</c>.
    /// </summary>
    public static TResult HarmonicMean<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);

        // https://en.wikipedia.org/wiki/Harmonic_mean
        var reciprocalSum = TResult.AdditiveIdentity;
        for (var i = 0; i < sequence.Length; i++)
        {
            reciprocalSum += TResult.One / TResult.CreateChecked(sequence[i]);
        }

        return TResult.CreateChecked(sequence.Length) / reciprocalSum;
    }
}
