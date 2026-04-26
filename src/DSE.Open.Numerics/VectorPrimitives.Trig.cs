// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise sine.</summary>
    public static void Sin<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Sin(x, destination);
    }

    /// <summary>Element-wise sine.</summary>

    public static Vector<T> Sin<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Sin(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise sine (in place).</summary>

    public static void SinInPlace<T>(this IVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Sin(x.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise cosine.</summary>

    public static void Cos<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Cos(x, destination);
    }

    /// <summary>Element-wise cosine.</summary>

    public static Vector<T> Cos<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Cos(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise cosine (in place).</summary>

    public static void CosInPlace<T>(this IVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Cos(x.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise tangent.</summary>

    public static void Tan<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Tan(x, destination);
    }

    /// <summary>Element-wise tangent.</summary>

    public static Vector<T> Tan<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Tan(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise tangent (in place).</summary>

    public static void TanInPlace<T>(this IVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Tan(x.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise sine and cosine.</summary>

    public static void SinCos<T>(ReadOnlySpan<T> x, in Span<T> sinDestination, in Span<T> cosDestination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(
            x.Length == sinDestination.Length && sinDestination.Length == cosDestination.Length);
        TensorPrimitives.SinCos(x, sinDestination, cosDestination);
    }

    /// <summary>Element-wise inverse sine.</summary>

    public static void Asin<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Asin(x, destination);
    }

    /// <summary>Element-wise inverse sine.</summary>

    public static Vector<T> Asin<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Asin(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise inverse cosine.</summary>

    public static void Acos<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Acos(x, destination);
    }

    /// <summary>Element-wise inverse cosine.</summary>

    public static Vector<T> Acos<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Acos(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise inverse tangent.</summary>

    public static void Atan<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ITrigonometricFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Atan(x, destination);
    }

    /// <summary>Element-wise inverse tangent.</summary>

    public static Vector<T> Atan<T>(this IReadOnlyVector<T> x)
        where T : struct, ITrigonometricFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Atan(x.AsSpan(), destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise <c>atan2(x, y)</c>.</summary>

    public static void Atan2<T>(ReadOnlySpan<T> y, ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == x.Length && x.Length == destination.Length);
        TensorPrimitives.Atan2(y, x, destination);
    }

    /// <summary>Element-wise <c>atan2(x, y)</c>.</summary>

    public static void Atan2<T>(ReadOnlySpan<T> y, T x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        TensorPrimitives.Atan2(y, x, destination);
    }

    /// <summary>Element-wise <c>atan2(x, y)</c>.</summary>

    public static void Atan2<T>(T y, ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, IFloatingPointIeee754<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Atan2(y, x, destination);
    }
}
