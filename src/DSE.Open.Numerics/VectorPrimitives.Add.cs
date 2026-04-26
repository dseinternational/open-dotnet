// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise <paramref name="x"/> + <paramref name="y"/>, written to <paramref name="destination"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="x"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException">The lengths of <paramref name="x"/>, <paramref name="y"/> and <paramref name="destination"/> differ.</exception>
    public static void Add<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Add(x.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <paramref name="x"/> + <paramref name="y"/>, written to <paramref name="destination"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="y"/> is <see langword="null"/>.</exception>
    public static void Add<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        Add(x, y.AsSpan(), destination);
    }

    /// <summary>Element-wise <paramref name="x"/> + <paramref name="y"/>, written to <paramref name="destination"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="destination"/> is <see langword="null"/>.</exception>
    public static void Add<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Add(x, y, destination.AsSpan());
    }

    /// <summary>Returns a new vector containing the element-wise sum of <paramref name="x"/> and <paramref name="y"/>.</summary>
    public static Vector<T> Add<T>(this IReadOnlyVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        var destination = Vector.Create<T>(x.Length);
        Add(x, y, destination.AsSpan());
        return destination;
    }

    /// <summary>Returns a new vector containing the element-wise sum of <paramref name="x"/> and <paramref name="y"/>.</summary>
    public static Vector<T> Add<T>(this IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Add(x, y.AsSpan());
    }

    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/>, written to <paramref name="destination"/>.</summary>
    public static void Add<T>(this IReadOnlyVector<T> x, T y, in Span<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Add(x.AsSpan(), y, destination);
    }

    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/>, written to <paramref name="destination"/>.</summary>
    public static void Add<T>(this IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Add(x, y, destination.AsSpan());
    }

    /// <summary>Returns a new vector with the scalar <paramref name="y"/> added to every element of <paramref name="x"/>.</summary>
    public static Vector<T> Add<T>(this IReadOnlyVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Add(x, y, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise <paramref name="x"/> += <paramref name="y"/> in place.</summary>
    public static void AddInPlace<T>(this IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Add(x, y, x.AsSpan());
    }

    /// <summary>Element-wise <paramref name="x"/> += <paramref name="y"/> in place.</summary>
    public static void AddInPlace<T>(this IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        AddInPlace(x, y.AsSpan());
    }

    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/> in place.</summary>
    public static void AddInPlace<T>(this IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Add(x, y, x.AsSpan());
    }
}
