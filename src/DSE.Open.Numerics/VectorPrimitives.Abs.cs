// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

/// <summary>
/// SIMD-friendly element-wise primitive operations over <see cref="ReadOnlySpan{T}"/>,
/// <see cref="IReadOnlyVector{T}"/> and <see cref="IVector{T}"/>. The methods
/// generally delegate to <see cref="System.Numerics.Tensors.TensorPrimitives"/>
/// after argument-length validation, with overloads for span/vector inputs,
/// span/vector destinations, and an <c>InPlace</c> variant where applicable.
/// </summary>
public static partial class VectorPrimitives
{
    /// <summary>Element-wise absolute value.</summary>
    public static void Abs<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Abs(x, destination);
    }

    /// <summary>Element-wise absolute value.</summary>

    public static void Abs<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Abs(x.AsSpan(), destination);
    }

    /// <summary>Element-wise absolute value.</summary>

    public static void Abs<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Abs(x, destination.AsSpan());
    }

    /// <summary>Element-wise absolute value.</summary>

    public static Vector<T> Abs<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Abs(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise absolute value (in place).</summary>

    public static void AbsInPlace<T>(this IVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Abs(x, x.AsSpan());
    }
}
