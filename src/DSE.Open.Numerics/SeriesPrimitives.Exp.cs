// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>e^x</c>.</summary>
    public static void Exp<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Exp(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>e^x</c>.</summary>

    public static void Exp<T>(this IReadOnlySeries<T> x, ISeries<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Exp(x, destination.AsSpan());
    }

    /// <summary>Element-wise <c>e^x</c>.</summary>

    public static Series<T> Exp<T>(this IReadOnlySeries<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Exp(), x);
    }

    /// <summary>Element-wise <c>e^x</c> (in place).</summary>

    public static void ExpInPlace<T>(this ISeries<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Exp(x.AsSpan(), x.AsSpan());
    }
}
