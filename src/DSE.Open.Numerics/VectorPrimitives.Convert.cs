// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise conversion to <typeparamref name="TTo"/> with overflow checking.</summary>
    public static void ConvertChecked<TFrom, TTo>(ReadOnlySpan<TFrom> source, in Span<TTo> destination)
        where TFrom : struct, INumberBase<TFrom>
        where TTo : struct, INumberBase<TTo>
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertChecked(source, destination);
    }

    /// <summary>Element-wise conversion to <typeparamref name="TTo"/>, saturating on overflow.</summary>

    public static void ConvertSaturating<TFrom, TTo>(ReadOnlySpan<TFrom> source, in Span<TTo> destination)
        where TFrom : struct, INumberBase<TFrom>
        where TTo : struct, INumberBase<TTo>
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertSaturating(source, destination);
    }

    /// <summary>Element-wise conversion to <typeparamref name="TTo"/>, truncating on overflow.</summary>

    public static void ConvertTruncating<TFrom, TTo>(ReadOnlySpan<TFrom> source, in Span<TTo> destination)
        where TFrom : struct, INumberBase<TFrom>
        where TTo : struct, INumberBase<TTo>
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertTruncating(source, destination);
    }

    /// <summary>Element-wise conversion to <typeparamref name="TTo"/> with rounding.</summary>

    public static void ConvertToInteger<TFrom, TTo>(ReadOnlySpan<TFrom> source, in Span<TTo> destination)
        where TFrom : struct, IFloatingPoint<TFrom>
        where TTo : struct, IBinaryInteger<TTo>
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertToInteger<TFrom, TTo>(source, destination);
    }

    /// <summary>Element-wise conversion to <typeparamref name="TTo"/> using native rounding.</summary>

    public static void ConvertToIntegerNative<TFrom, TTo>(ReadOnlySpan<TFrom> source, in Span<TTo> destination)
        where TFrom : struct, IFloatingPoint<TFrom>
        where TTo : struct, IBinaryInteger<TTo>
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertToIntegerNative<TFrom, TTo>(source, destination);
    }

    /// <summary>Element-wise conversion to <see cref="Half"/>.</summary>

    public static void ConvertToHalf(ReadOnlySpan<float> source, in Span<Half> destination)
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertToHalf(source, destination);
    }

    /// <summary>Element-wise conversion to <see cref="float"/>.</summary>

    public static void ConvertToSingle(ReadOnlySpan<Half> source, in Span<float> destination)
    {
        NumericsArgumentException.ThrowIfNot(source.Length == destination.Length);
        TensorPrimitives.ConvertToSingle(source, destination);
    }
}
