// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>
    public static Series<bool> LessThan<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(x.Vector.LessThan(y.Vector), x);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        _ = VectorPrimitives.LessThan(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        LessThan(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        LessThan(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static Series<bool> LessThan<T>(this IReadOnlySeries<T> x, T y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapTypeChange<T, bool>(x.Vector.LessThan(y), x);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this IReadOnlySeries<T> x, T y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        _ = VectorPrimitives.LessThan(x.Vector.AsSpan(), y, destination);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this IReadOnlySeries<T> x, T y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        LessThan(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static Series<bool> LessThan<T>(this T x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(x.LessThan(y.Vector), y);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this T x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        _ = VectorPrimitives.LessThan(x, y.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise <c>x &lt; y</c> comparison.</summary>

    public static void LessThan<T>(this T x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        LessThan(x, y, destination.AsSpan());
    }
}
