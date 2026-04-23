// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

// Mirrors VectorPrimitives.IsPredicates. Each predicate exposes:
//   - IsXxx(IReadOnlySeries<T>, Span<bool>)  — element-wise mask
//   - IsXxxAll(IReadOnlySeries<T>) : bool    — all-true reduction
//   - IsXxxAny(IReadOnlySeries<T>) : bool    — any-true reduction
// Where a caller needs a `ReadOnlySpan<T>`-based call, use
// `VectorPrimitives.IsXxx(series.Vector.AsSpan(), …)` directly.
public static partial class SeriesPrimitives
{
    // -------- IsFinite --------

    public static void IsFinite<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsFinite(in span, destination);
    }

    public static bool IsFiniteAll<T>(this IReadOnlySeries<T> x)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return VectorPrimitives.IsFiniteAll(in span);
    }

    public static bool IsFiniteAny<T>(this IReadOnlySeries<T> x)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return VectorPrimitives.IsFiniteAny(in span);
    }

    // -------- IsInfinity --------

    public static void IsInfinity<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsInfinity(in span, destination);
    }

    public static bool IsInfinityAll<T>(this IReadOnlySeries<T> x)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return VectorPrimitives.IsInfinityAll(in span);
    }

    public static bool IsInfinityAny<T>(this IReadOnlySeries<T> x)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return VectorPrimitives.IsInfinityAny(in span);
    }

    // -------- IsZero / IsPositive / IsNegative --------

    public static void IsZero<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsZero(in span, destination);
    }

    public static bool IsZeroAll<T>(this IReadOnlySeries<T> x)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return VectorPrimitives.IsZeroAll(in span);
    }

    public static bool IsZeroAny<T>(this IReadOnlySeries<T> x)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return VectorPrimitives.IsZeroAny(in span);
    }

    public static void IsPositive<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsPositive(in span, destination);
    }

    public static void IsNegative<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsNegative(in span, destination);
    }

    // -------- IsInteger / IsEvenInteger / IsOddInteger --------

    public static void IsInteger<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsInteger(in span, destination);
    }

    public static void IsEvenInteger<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsEvenInteger(in span, destination);
    }

    public static void IsOddInteger<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsOddInteger(in span, destination);
    }

    // -------- IsNormal / IsSubnormal --------

    public static void IsNormal<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsNormal(in span, destination);
    }

    public static void IsSubnormal<T>(this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        VectorPrimitives.IsSubnormal(in span, destination);
    }
}
