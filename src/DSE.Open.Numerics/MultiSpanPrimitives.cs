// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public static partial class MultiSpanPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ReadOnlyMultiSpan<T> x, ReadOnlyMultiSpan<T> y, MultiSpan<T> destination)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotSameShape(x, y, destination);
        VectorPrimitives.Add(x.Elements, y.Elements, destination.Elements);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(MultiSpan<T> x, ReadOnlyMultiSpan<T> y)
        where T : struct, INumber<T>
    {
        NumericsException.ThrowIfNotSameShape(x, y);
        VectorPrimitives.AddInPlace(x.Elements, y.Elements);
    }
}
