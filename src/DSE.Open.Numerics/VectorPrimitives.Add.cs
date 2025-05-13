// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlyVector<T> x, IReadOnlyVector<T> y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        Add(x, y, destination.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlyVector<T> x, IReadOnlyVector<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        TensorPrimitives.Add(x.AsSpan(), y.AsSpan(), destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(IVector<T> x, IReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        AddInPlace(x, y.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(IVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y);
        TensorPrimitives.Add(x.AsSpan(), y, x.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlyVector<T> x, T y, IVector<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        Add(x, y, destination.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlyVector<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        TensorPrimitives.Add(x.AsSpan(), y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(IVector<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        TensorPrimitives.Add(x.AsSpan(), y, x.AsSpan());
    }
}
