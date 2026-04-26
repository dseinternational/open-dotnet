// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>
    public static Series<bool> GreaterThan<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(x.Vector.GreaterThan(y.Vector), x);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        _ = VectorPrimitives.GreaterThan(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        GreaterThan(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        GreaterThan(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static Series<bool> GreaterThan<T>(this IReadOnlySeries<T> x, T y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapTypeChange<T, bool>(x.Vector.GreaterThan(y), x);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this IReadOnlySeries<T> x, T y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        _ = VectorPrimitives.GreaterThan(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this IReadOnlySeries<T> x, T y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        GreaterThan(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static Series<bool> GreaterThan<T>(this T x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(x.GreaterThan(y.Vector), y);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this T x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        _ = VectorPrimitives.GreaterThan(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x &gt; y</c> comparison.</summary>

    public static void GreaterThan<T>(this T x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        GreaterThan(x, y, destination.AsSpan());
    }
}
