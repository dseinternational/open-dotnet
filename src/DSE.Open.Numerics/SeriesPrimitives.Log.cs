// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise natural log.</summary>
    public static void Log<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise natural log.</summary>

    public static Series<T> Log<T>(this IReadOnlySeries<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Log(), x);
    }

    /// <summary>Element-wise natural log (in place).</summary>

    public static void LogInPlace<T>(this ISeries<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log(x.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise natural log.</summary>

    public static void Log<T>(this IReadOnlySeries<T> x, T newBase, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log(x.Vector.AsSpan(), newBase, destination);
    }

    /// <summary>Element-wise base-2 log.</summary>

    public static void Log2<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log2(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise base-2 log.</summary>

    public static Series<T> Log2<T>(this IReadOnlySeries<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Log2(), x);
    }

    /// <summary>Element-wise base-2 log (in place).</summary>

    public static void Log2InPlace<T>(this ISeries<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log2(x.AsSpan(), x.AsSpan());
    }

    /// <summary>Element-wise base-10 log.</summary>

    public static void Log10<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log10(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise base-10 log.</summary>

    public static Series<T> Log10<T>(this IReadOnlySeries<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Log10(), x);
    }

    /// <summary>Element-wise base-10 log (in place).</summary>

    public static void Log10InPlace<T>(this ISeries<T> x)
        where T : struct, ILogarithmicFunctions<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Log10(x.AsSpan(), x.AsSpan());
    }
}
