// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>
    /// Gets the geometric mean of a non-empty sequence, computed as <c>exp(mean(log(x)))</c>
    /// for numerical stability.
    /// </summary>
    /// <remarks>
    /// All elements must be strictly positive; zero or negative inputs produce <c>NaN</c>
    /// (from <c>log</c>), which will propagate through to the result.
    /// </remarks>
    public static T GeometricMean<T>(ReadOnlySpan<T> sequence)
        where T : struct, ILogarithmicFunctions<T>, IExponentialFunctions<T>
    {
        return GeometricMean<T, T>(sequence);
    }

    /// <summary>
    /// Gets the geometric mean of a non-empty sequence, accumulating into
    /// <typeparamref name="TResult"/>, computed as <c>exp(mean(log(x)))</c> for numerical
    /// stability.
    /// </summary>
    public static TResult GeometricMean<T, TResult>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, ILogarithmicFunctions<TResult>, IExponentialFunctions<TResult>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);

        // https://en.wikipedia.org/wiki/Geometric_mean
        // Accumulate in log-space to avoid overflow for long sequences or large values.
        var sumLogs = TResult.AdditiveIdentity;
        for (var i = 0; i < sequence.Length; i++)
        {
            sumLogs += TResult.Log(TResult.CreateChecked(sequence[i]));
        }

        return TResult.Exp(sumLogs / TResult.CreateChecked(sequence.Length));
    }
}
