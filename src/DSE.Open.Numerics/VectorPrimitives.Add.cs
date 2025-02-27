// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(NumericVector<T> x, NumericVector<T> y, Span<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y, destination);
        TensorPrimitives.Add(x.Span, y.Span, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(NumericVector<T> x, NumericVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        AddInPlace(x, y.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(NumericVector<T> x, ReadOnlySpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotEqualLength(x, y);
        TensorPrimitives.Add(x.Span, y, x.Span);
    }
}
