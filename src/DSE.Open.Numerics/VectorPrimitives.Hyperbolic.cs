// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise hyperbolic sine.</summary>
    public static void Sinh<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Sinh(x, destination);
    }

    /// <summary>Element-wise hyperbolic sine.</summary>

    public static Vector<T> Sinh<T>(this IReadOnlyVector<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Sinh(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise hyperbolic cosine.</summary>

    public static void Cosh<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Cosh(x, destination);
    }

    /// <summary>Element-wise hyperbolic cosine.</summary>

    public static Vector<T> Cosh<T>(this IReadOnlyVector<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Cosh(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise hyperbolic tangent.</summary>

    public static void Tanh<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Tanh(x, destination);
    }

    /// <summary>Element-wise hyperbolic tangent.</summary>

    public static Vector<T> Tanh<T>(this IReadOnlyVector<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Tanh(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise inverse hyperbolic sine.</summary>

    public static void Asinh<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Asinh(x, destination);
    }

    /// <summary>Element-wise inverse hyperbolic sine.</summary>

    public static Vector<T> Asinh<T>(this IReadOnlyVector<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Asinh(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise inverse hyperbolic cosine.</summary>

    public static void Acosh<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Acosh(x, destination);
    }

    /// <summary>Element-wise inverse hyperbolic cosine.</summary>

    public static Vector<T> Acosh<T>(this IReadOnlyVector<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Acosh(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise inverse hyperbolic tangent.</summary>

    public static void Atanh<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IHyperbolicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Atanh(x, destination);
    }

    /// <summary>Element-wise inverse hyperbolic tangent.</summary>

    public static Vector<T> Atanh<T>(this IReadOnlyVector<T> x)
        where T : struct, IHyperbolicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Atanh(x.AsSpan(), destination.AsSpan());
        return destination;
    }
}
