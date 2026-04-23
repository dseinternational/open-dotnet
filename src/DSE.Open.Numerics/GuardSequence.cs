// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Argument-validation helpers for numeric sequences. Throws
/// <see cref="NumericsArgumentException"/> when invariants are violated.
/// </summary>
public static class GuardSequence
{
    /// <summary>
    /// Throws if <paramref name="x"/> and <paramref name="y"/> have different lengths.
    /// </summary>
    /// <exception cref="NumericsArgumentException">The spans differ in length.</exception>
    public static void SameLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length)
        {
            NumericsArgumentException.Throw("Sequences must be the same length.");
        }
    }

    /// <summary>
    /// Throws if any of <paramref name="x"/>, <paramref name="y"/>, or
    /// <paramref name="z"/> has a different length from the others.
    /// </summary>
    /// <exception cref="NumericsArgumentException">The spans differ in length.</exception>
    public static void SameLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> z)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length || x.Length != z.Length)
        {
            NumericsArgumentException.Throw("Sequences must be the same length.");
        }
    }
}
