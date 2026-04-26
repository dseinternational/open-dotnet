// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise <c>e^x</c>.</summary>
    public static void Exp<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Exp(x, destination);
    }

    /// <summary>Element-wise <c>e^x</c>.</summary>

    public static void Exp<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Exp(x.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>e^x</c>.</summary>

    public static void Exp<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Exp(x, destination.AsSpan());
    }

    /// <summary>Element-wise <c>e^x</c>.</summary>

    public static Vector<T> Exp<T>(this IReadOnlyVector<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Exp(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise <c>e^x</c> (in place).</summary>

    public static void ExpInPlace<T>(this IVector<T> x)
        where T : struct, IExponentialFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Exp(x, x.AsSpan());
    }
}
