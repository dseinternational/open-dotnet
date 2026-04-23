// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Sqrt<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Sqrt(x, destination);
    }

    public static void Sqrt<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Sqrt(x.AsSpan(), destination);
    }

    public static void Sqrt<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sqrt(x, destination.AsSpan());
    }

    public static Vector<T> Sqrt<T>(this IReadOnlyVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Sqrt(x, destination.AsSpan());
        return destination;
    }

    public static void SqrtInPlace<T>(this IVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Sqrt(x, x.AsSpan());
    }

    public static void Cbrt<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Cbrt(x, destination);
    }

    public static void Cbrt<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Cbrt(x.AsSpan(), destination);
    }

    public static void Cbrt<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Cbrt(x, destination.AsSpan());
    }

    public static Vector<T> Cbrt<T>(this IReadOnlyVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Cbrt(x, destination.AsSpan());
        return destination;
    }

    public static void CbrtInPlace<T>(this IVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Cbrt(x, x.AsSpan());
    }
}
