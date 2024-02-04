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
    public static void Subtract<T>(T[] x, T[] y, T[] destination)
        where T : struct, INumber<T>
    {
        Subtract(x.AsSpan(), y.AsSpan(), destination.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(ImmutableArray<T> x, ImmutableArray<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        Subtract(x.AsSpan(), y.AsSpan(), destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Subtract<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        TensorPrimitives.Subtract(x, y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(T[] x, T[] y)
        where T : struct, INumber<T>
    {
        SubtractInPlace(x.AsSpan(), y.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(Span<T> x, ImmutableArray<T> y)
        where T : struct, INumber<T>
    {
        SubtractInPlace(x, y.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SubtractInPlace<T>(Span<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y);
        TensorPrimitives.Subtract(x, y, x);
    }
}
