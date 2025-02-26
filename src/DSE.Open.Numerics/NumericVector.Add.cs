// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class NumericVector
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ReadOnlyNumericVector<T> x, ReadOnlyNumericVector<T> y, NumericVector<T> destination)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        VectorPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPace<T>(NumericVector<T> x, ReadOnlyNumericVector<T> y)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.AddInPlace(x.Span, y.Span);
    }
}
