// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    public static void Add<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Add(x.AsSpan(), y, destination);
    }

    public static void Add<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Add(x, y.AsSpan(), destination);
        Add(x, y.AsSpan(), destination);
    }

    public static void Add<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Add(x, y, destination.AsSpan());
    }

    public static Vector<T> Add<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == y.Length);
        var destination = Vector.Create<T>(x.Length);
        Add(x, y, destination.AsSpan());
        return destination;
    }

    public static Vector<T> Add<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Add(x, y.AsSpan());
    }

    public static void Add<T>(this IReadOnlyVector<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Add(x.AsSpan(), y, destination);
    }

    public static void Add<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Add(x, y, destination.AsSpan());
    }

    public static Vector<T> Add<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Add(x, y, destination.AsSpan());
        return destination;
    }

    public static void AddInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Add(x, y, x.AsSpan());
    }

    public static void AddInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        AddInPlace(x, y.AsSpan());
    }

    public static void AddInPlace<T>(this IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Add(x, y, x.AsSpan());
    }
}
