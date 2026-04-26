// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorPrimitives
{
    /// <summary>Element-wise natural log.</summary>
    public static void Log<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Log(x, destination);
    }

    /// <summary>Element-wise natural log.</summary>

    public static void Log<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Log(x.AsSpan(), destination);
    }

    /// <summary>Element-wise natural log.</summary>

    public static void Log<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Log(x, destination.AsSpan());
    }

    /// <summary>Element-wise natural log.</summary>

    public static Vector<T> Log<T>(this IReadOnlyVector<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Log(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise natural log (in place).</summary>

    public static void LogInPlace<T>(this IVector<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Log(x, x.AsSpan());
    }

    /// <summary>Element-wise natural log.</summary>

    public static void Log<T>(ReadOnlySpan<T> x, T newBase, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Log(x, newBase, destination);
    }

    /// <summary>Element-wise natural log.</summary>

    public static void Log<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        TensorPrimitives.Log(x, y, destination);
    }

    /// <summary>Element-wise base-2 log.</summary>

    public static void Log2<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Log2(x, destination);
    }

    /// <summary>Element-wise base-2 log.</summary>

    public static void Log2<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Log2(x.AsSpan(), destination);
    }

    /// <summary>Element-wise base-2 log.</summary>

    public static void Log2<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Log2(x, destination.AsSpan());
    }

    /// <summary>Element-wise base-2 log.</summary>

    public static Vector<T> Log2<T>(this IReadOnlyVector<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Log2(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise base-2 log (in place).</summary>

    public static void Log2InPlace<T>(this IVector<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Log2(x, x.AsSpan());
    }

    /// <summary>Element-wise base-10 log.</summary>

    public static void Log10<T>(ReadOnlySpan<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        TensorPrimitives.Log10(x, destination);
    }

    /// <summary>Element-wise base-10 log.</summary>

    public static void Log10<T>(this IReadOnlyVector<T> x, in Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Log10(x.AsSpan(), destination);
    }

    /// <summary>Element-wise base-10 log.</summary>

    public static void Log10<T>(this IReadOnlyVector<T> x, IVector<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Log10(x, destination.AsSpan());
    }

    /// <summary>Element-wise base-10 log.</summary>

    public static Vector<T> Log10<T>(this IReadOnlyVector<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var destination = Vector.Create<T>(x.Length);
        Log10(x, destination.AsSpan());
        return destination;
    }

    /// <summary>Element-wise base-10 log (in place).</summary>

    public static void Log10InPlace<T>(this IVector<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        Log10(x, x.AsSpan());
    }
}
