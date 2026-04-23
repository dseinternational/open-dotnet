// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

// Internal helpers used by returning-Series<T> primitives to set Name on the
// result series instead of falling through the implicit Vector<T> → Series<T>
// conversion (which drops all metadata).
//
// Categories and ValueLabels are intentionally *not* carried by these helpers:
// element-wise transforms generally produce values that are outside the
// input's Categories set and invalidate the value/label mapping, so the
// result is no longer categorical in a meaningful sense.
//
// Name convention:
//   - Unary transforms and series-scalar binary transforms: Name comes from
//     the input series (WrapUnary).
//   - Series-series binary transforms: LHS.Name wins by convention
//     (WrapBinary); y is carried in the signature to document the binary
//     nature of the call.
//   - Type-changing transforms (Series<TIn> → Series<TOut>): Name is
//     carried across the type boundary (WrapTypeChange).
public static partial class SeriesPrimitives
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Series<T> WrapUnary<T>(Vector<T> result, IReadOnlySeries<T> source)
        where T : IEquatable<T>
    {
        return new Series<T>(result, source.Name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Series<T> WrapBinary<T>(Vector<T> result, IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IEquatable<T>
    {
        _ = y;
        return new Series<T>(result, x.Name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Series<TOut> WrapTypeChange<TIn, TOut>(Vector<TOut> result, IReadOnlySeries<TIn> source)
        where TIn : IEquatable<TIn>
        where TOut : IEquatable<TOut>
    {
        return new Series<TOut>(result, source.Name);
    }
}
