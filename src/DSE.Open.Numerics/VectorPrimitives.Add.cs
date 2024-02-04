// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(T[] x, T[] y, T[] destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        Add(x.AsSpan(), y.AsSpan(), destination.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ImmutableArray<T> x, ImmutableArray<T> y, Span<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        Add(x.AsSpan(), y.AsSpan(), destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        TensorPrimitives.Add(x, y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(T[] x, T[] y)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        AddInPlace(x.AsSpan(), y.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(Span<T> x, ImmutableArray<T> y)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        AddInPlace(x, y.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y);
        TensorPrimitives.Add(x, y, x);
    }
}
