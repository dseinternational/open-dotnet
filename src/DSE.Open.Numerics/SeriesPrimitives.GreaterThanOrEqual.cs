// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>
    public static Series<bool> GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(x.Vector.GreaterThanOrEqual(y.Vector), x);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        _ = VectorPrimitives.GreaterThanOrEqual(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        GreaterThanOrEqual(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        GreaterThanOrEqual(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static Series<bool> GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, T y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapTypeChange<T, bool>(x.Vector.GreaterThanOrEqual(y), x);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, T y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        _ = VectorPrimitives.GreaterThanOrEqual(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this IReadOnlySeries<T> x, T y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        GreaterThanOrEqual(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static Series<bool> GreaterThanOrEqual<T>(this T x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(x.GreaterThanOrEqual(y.Vector), y);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this T x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        _ = VectorPrimitives.GreaterThanOrEqual(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x &gt;= y</c> comparison.</summary>

    public static void GreaterThanOrEqual<T>(this T x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        GreaterThanOrEqual(x, y, destination.AsSpan());
    }
}
