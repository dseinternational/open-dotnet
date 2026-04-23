// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Pow<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Pow(x, y, destination);
    }

    public static void Pow<T>(ReadOnlySpan<T> x, T y, in Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Pow(x, y, destination);
    }

    public static void Pow<T>(T x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Pow(x, y, destination);
    }

    public static void Pow<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, in Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        Pow(x.AsSpan(), y.AsSpan(), destination);
    }

    public static void Pow<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Pow(x, y, destination.AsSpan());
    }

    public static Vector<T> Pow<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        var destination = Vector.Create<T>(x.Length);
        Pow(x, y, destination.AsSpan());
        return destination;
    }

    public static void Pow<T>(this IReadOnlyVector<T> x, T y, in Span<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Pow(x.AsSpan(), y, destination);
    }

    public static void Pow<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Pow(x, y, destination.AsSpan());
    }

    public static Vector<T> Pow<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Pow(x, y, destination.AsSpan());
        return destination;
    }

    public static void PowInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Pow(x.AsSpan(), y, x.AsSpan());
    }

    public static void PowInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        PowInPlace(x, y.AsSpan());
    }

    public static void PowInPlace<T>(this IVector<T> x, T y)
        where T : struct, IPowerFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Pow(x.AsSpan(), y, x.AsSpan());
    }
}
