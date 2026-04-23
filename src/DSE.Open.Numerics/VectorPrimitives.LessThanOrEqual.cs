// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;


public static partial class VectorPrimitives
{
    public static Vector<bool> LessThanOrEqual<T>(
        this IReadOnlyVector<T> x,
        IReadOnlyVector<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return Vector.Create(LessThanOrEqual(x.AsSpan(), y.AsSpan()));
    }

    public static Span<bool> LessThanOrEqual<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
        where T : IComparisonOperators<T, T, bool>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        var destination = CreateUninitializedArray<bool>(x.Length);
        _ = LessThanOrEqual(x, y, destination);
        return destination;
    }

    public static Vector<bool> LessThanOrEqual<T>(
        this IReadOnlyVector<T> x,
        IReadOnlyVector<T> y,
        Vector<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(destination);
        return Vector.Create(LessThanOrEqual(x.AsSpan(), y.AsSpan(), destination.AsSpan()));
    }

    public static ref readonly Span<bool> LessThanOrEqual<T>(
        scoped in ReadOnlySpan<T> x,
        scoped in ReadOnlySpan<T> y,
        in Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && destination.Length >= x.Length);

        _ = Tensor.LessThanOrEqual(
            new ReadOnlyTensorSpan<T>(x),
            new ReadOnlyTensorSpan<T>(y),
            new TensorSpan<bool>(destination));

        return ref destination;
    }

    public static Vector<bool> LessThanOrEqual<T>(this IReadOnlyVector<T> x, in T y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return Vector.Create(LessThanOrEqual(x.AsSpan(), y));
    }

    public static Span<bool> LessThanOrEqual<T>(in ReadOnlySpan<T> x, in T y)
        where T : IComparisonOperators<T, T, bool>
    {
        var destination = CreateUninitializedArray<bool>(x.Length);
        _ = LessThanOrEqual(x, y, destination);
        return destination;
    }

    public static Vector<bool> LessThanOrEqual<T>(
        this IReadOnlyVector<T> x,
        T y,
        in Vector<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(destination);
        _ = LessThanOrEqual(x.AsSpan(), y, destination.AsSpan());
        return destination;
    }

    public static ref readonly Span<bool> LessThanOrEqual<T>(
        scoped in ReadOnlySpan<T> x,
        T y,
        in Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>
    {
        NumericsArgumentException.ThrowIfNot(destination.Length >= x.Length);

        _ = Tensor.LessThanOrEqual(
            new ReadOnlyTensorSpan<T>(x),
            y,
            new TensorSpan<bool>(destination));

        return ref destination;
    }

    public static Vector<bool> LessThanOrEqual<T>(this T x, IReadOnlyVector<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return Vector.Create(LessThanOrEqual(x, y.AsSpan()));
    }

    public static Span<bool> LessThanOrEqual<T>(this T x, in ReadOnlySpan<T> y)
        where T : IComparisonOperators<T, T, bool>
    {
        var destination = CreateUninitializedArray<bool>(y.Length);
        _ = LessThanOrEqual(x, y, destination);
        return destination;
    }

    public static Vector<bool> LessThanOrEqual<T>(
        this T x,
        IReadOnlyVector<T> y,
        in Vector<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(destination);
        _ = LessThanOrEqual(x, y.AsSpan(), destination.AsSpan());
        return destination;
    }

    public static ref readonly Span<bool> LessThanOrEqual<T>(
        this T x,
        scoped in ReadOnlySpan<T> y,
        in Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>
    {
        NumericsArgumentException.ThrowIfNot(destination.Length >= y.Length);

        _ = Tensor.LessThanOrEqual(
            x,
            new ReadOnlyTensorSpan<T>(y),
            new TensorSpan<bool>(destination));

        return ref destination;
    }
}
