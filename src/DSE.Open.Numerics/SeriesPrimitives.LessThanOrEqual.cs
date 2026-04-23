// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static Series<bool> LessThanOrEqual<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return x.Vector.LessThanOrEqual(y.Vector);
    }

    public static void LessThanOrEqual<T>(this IReadOnlySeries<T> x, ReadOnlySpan<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        _ = VectorPrimitives.LessThanOrEqual(x.Vector.AsSpan(), y, destination);
    }

    public static void LessThanOrEqual<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        LessThanOrEqual(x, y.Vector.AsSpan(), destination);
    }

    public static void LessThanOrEqual<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        LessThanOrEqual(x, y, destination.AsSpan());
    }

    public static Series<bool> LessThanOrEqual<T>(this IReadOnlySeries<T> x, T y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.LessThanOrEqual(y);
    }

    public static void LessThanOrEqual<T>(this IReadOnlySeries<T> x, T y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        _ = VectorPrimitives.LessThanOrEqual(x.Vector.AsSpan(), y, destination);
    }

    public static void LessThanOrEqual<T>(this IReadOnlySeries<T> x, T y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        LessThanOrEqual(x, y, destination.AsSpan());
    }

    public static Series<bool> LessThanOrEqual<T>(this T x, IReadOnlySeries<T> y)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        return x.LessThanOrEqual(y.Vector);
    }

    public static void LessThanOrEqual<T>(this T x, IReadOnlySeries<T> y, Span<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(y.Length == destination.Length);
        _ = VectorPrimitives.LessThanOrEqual(x, y.Vector.AsSpan(), destination);
    }

    public static void LessThanOrEqual<T>(this T x, IReadOnlySeries<T> y, ISeries<bool> destination)
        where T : IComparisonOperators<T, T, bool>, IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        LessThanOrEqual(x, y, destination.AsSpan());
    }
}
