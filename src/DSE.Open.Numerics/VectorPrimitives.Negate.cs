// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise unary negation.</summary>
    public static void Negate<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Negate(x, destination);
    }

    /// <summary>Element-wise unary negation.</summary>

    public static void Negate<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Negate(x.AsSpan(), destination);
    }

    /// <summary>Element-wise unary negation.</summary>

    public static void Negate<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Negate(x, destination.AsSpan());
    }

    /// <summary>Element-wise unary negation.</summary>

    public static Vector<T> Negate<T>(this IReadOnlyVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Negate(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise unary negation (in place).</summary>

    public static void NegateInPlace<T>(this IVector<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Negate(x, x.AsSpan());
    }
}
