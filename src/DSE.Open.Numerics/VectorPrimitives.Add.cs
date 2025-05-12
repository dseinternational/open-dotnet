// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        Add(x, y, destination.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        TensorPrimitives.Add(x.AsReadOnlySpan(), y.AsReadOnlySpan(), destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(ISeries<T> x, IReadOnlySeries<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        AddInPlace(x, y.AsReadOnlySpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(ISeries<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y);
        TensorPrimitives.Add(x.AsSpan(), y, x.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlySeries<T> x, T y, ISeries<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        Add(x, y, destination.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(IReadOnlySeries<T> x, T y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, destination);
        TensorPrimitives.Add(x.AsReadOnlySpan(), y, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(ISeries<T> x, T y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        TensorPrimitives.Add(x.AsSpan(), y, x.AsSpan());
    }
}
