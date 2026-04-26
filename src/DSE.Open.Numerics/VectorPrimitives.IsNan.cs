// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise <c>IsNaN</c> predicate.</summary>
    public static void IsNaN<T>(in ReadOnlySpan<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        TensorPrimitives.IsNaN(x, destination);
    }

    /// <summary>Element-wise <c>IsNaN</c> predicate.</summary>

    public static void IsNaN<T>(this IReadOnlyVector<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        IsNaN(x.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>IsNaN</c> predicate.</summary>

    public static void IsNaN<T>(this IReadOnlyVector<T> x, IVector<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        IsNaN(x, destination.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when every element is NaN.</summary>

    public static bool IsNaNAll<T>(in ReadOnlySpan<T> x)
        where T : INumberBase<T>
    {
        return TensorPrimitives.IsNaNAll(x);
    }

    /// <summary>Returns <see langword="true"/> when every element is NaN.</summary>

    public static bool IsNaNAll<T>(this IReadOnlyVector<T> x)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsNaNAll(x.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when any element is NaN.</summary>

    public static bool IsNaNAny<T>(in ReadOnlySpan<T> x)
        where T : INumberBase<T>
    {
        return TensorPrimitives.IsNaNAny(x);
    }

    /// <summary>Returns <see langword="true"/> when any element is NaN.</summary>

    public static bool IsNaNAny<T>(this IReadOnlyVector<T> x)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return IsNaNAny(x.AsSpan());
    }
}
