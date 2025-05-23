// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static class GuardSequence
{
    public static void SameLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length)
        {
            NumericsArgumentException.Throw("Sequences must be the same length.");
        }
    }

    public static void SameLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> z)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length || x.Length != z.Length)
        {
            NumericsArgumentException.Throw("Sequences must be the same length.");
        }
    }
}
