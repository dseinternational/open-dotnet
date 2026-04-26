// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise square root.</summary>
    public static void Sqrt<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Sqrt(x, destination);
    }

    /// <summary>Element-wise square root.</summary>

    public static void Sqrt<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Sqrt(x.AsSpan(), destination);
    }

    /// <summary>Element-wise square root.</summary>

    public static void Sqrt<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Sqrt(x, destination.AsSpan());
    }

    /// <summary>Element-wise square root.</summary>

    public static Vector<T> Sqrt<T>(this IReadOnlyVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Sqrt(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise square root (in place).</summary>

    public static void SqrtInPlace<T>(this IVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Sqrt(x, x.AsSpan());
    }

    /// <summary>Element-wise cube root.</summary>

    public static void Cbrt<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Cbrt(x, destination);
    }

    /// <summary>Element-wise cube root.</summary>

    public static void Cbrt<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Cbrt(x.AsSpan(), destination);
    }

    /// <summary>Element-wise cube root.</summary>

    public static void Cbrt<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Cbrt(x, destination.AsSpan());
    }

    /// <summary>Element-wise cube root.</summary>

    public static Vector<T> Cbrt<T>(this IReadOnlyVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Cbrt(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise cube root (in place).</summary>

    public static void CbrtInPlace<T>(this IVector<T> x)
        where T : struct, IRootFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Cbrt(x, x.AsSpan());
    }
}
