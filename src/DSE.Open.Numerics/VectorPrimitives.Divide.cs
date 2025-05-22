// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Divide<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Divide(x.AsSpan(), y, destination);
    }

    public static void Divide<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Divide(x, y.AsSpan(), destination);
        Divide(x, y.AsSpan(), destination);
    }

    public static void Divide<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Divide(x, y, destination.AsSpan());
    }

    public static Vector<T> Divide<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length);
        var destination = Vector.Create<T>(x.Length);
        Divide(x, y, destination.AsSpan());
        return destination;
    }

    public static Vector<T> Divide<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Divide(x, y.AsSpan());
    }

    public static void Divide<T>(this IReadOnlyVector<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Divide(x.AsSpan(), y, destination);
    }

    public static void Divide<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Divide(x, y, destination.AsSpan());
    }

    public static Vector<T> Divide<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Divide(x, y, destination.AsSpan());
        return destination;
    }

    public static void DivideInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Divide(x, y, x.AsSpan());
    }

    public static void DivideInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        DivideInPlace(x, y.AsSpan());
    }

    public static void DivideInPlace<T>(this IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Divide(x, y, x.AsSpan());
    }
}
