// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public static partial class TensorSpan
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ReadOnlyTensorSpan<T> x, ReadOnlyTensorSpan<T> y, TensorSpan<T> destination)
        where T : struct, INumber<T>
    {
        MultiSpanPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(TensorSpan<T> x, ReadOnlyTensorSpan<T> y)
        where T : struct, INumber<T>
    {
        MultiSpanPrimitives.AddInPlace(x.Span, y.Span);
    }
}
