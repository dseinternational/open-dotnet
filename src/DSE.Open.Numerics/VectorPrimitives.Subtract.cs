// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Subtract<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Subtract(x.AsSpan(), y, destination);
    }

    public static void Subtract<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Subtract(x, y.AsSpan(), destination);
        Subtract(x, y.AsSpan(), destination);
    }

    public static void Subtract<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Subtract(x, y, destination.AsSpan());
    }

    public static Vector<T> Subtract<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length);
        var destination = Vector.Create<T>(x.Length);
        Subtract(x, y, destination.AsSpan());
        return destination;
    }

    public static Vector<T> Subtract<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Subtract(x, y.AsSpan());
    }

    public static void Subtract<T>(this IReadOnlyVector<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Subtract(x.AsSpan(), y, destination);
    }

    public static void Subtract<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Subtract(x, y, destination.AsSpan());
    }

    public static Vector<T> Subtract<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Subtract(x, y, destination.AsSpan());
        return destination;
    }

    public static void SubtractInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Subtract(x, y, x.AsSpan());
    }

    public static void SubtractInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        SubtractInPlace(x, y.AsSpan());
    }

    public static void SubtractInPlace<T>(this IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Subtract(x, y, x.AsSpan());
    }
}
