// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;

namespace DSE.Open.Numerics;

// Uses IEqualityOperators{T,T,bool} rather than IEquatable<T> so NA/NaN semantics match
// VectorPrimitives.Equals (see VectorPrimitives.Equals for details).
public static partial class SeriesPrimitives
{
    /// <summary>Reserved — call the typed <c>Equals</c> overloads instead.</summary>
    [Obsolete("Not supported", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static new bool Equals(object? a, object? b)
    {
        throw new NotSupportedException();
    }

    /// <summary>Element-wise <c>x == y</c> comparison.</summary>

    public static Series<bool> Equals<T>(IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return WrapTypeChange<T, bool>(VectorPrimitives.Equals(x.Vector, y.Vector), x);
    }

    /// <summary>Element-wise <c>x == y</c> comparison.</summary>

    public static Series<bool> Equals<T>(IReadOnlySeries<T> x, T y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        var span = x.Vector.AsSpan();
        return WrapTypeChange<T, bool>(VectorPrimitives.Equals(in span, in y), x);
    }

    /// <summary>Element-wise <c>x == y</c> comparison.</summary>

    public static void Equals<T>(
        IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        Span<bool> destination)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);
        var xSpan = x.Vector.AsSpan();
        var ySpan = y.Vector.AsSpan();
        _ = VectorPrimitives.Equals(in xSpan, in ySpan, destination);
    }

    /// <summary>Element-wise <c>x == y</c> comparison.</summary>

    public static void Equals<T>(
        IReadOnlySeries<T> x,
        IReadOnlySeries<T> y,
        ISeries<bool> destination)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Equals(x, y, destination.AsSpan());
    }

    /// <summary>Element-wise <c>x == y</c> comparison.</summary>

    public static void Equals<T>(IReadOnlySeries<T> x, T y, Span<bool> destination)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        NumericsArgumentException.ThrowIfNot(x.Length == destination.Length);
        var xSpan = x.Vector.AsSpan();
        _ = VectorPrimitives.Equals(in xSpan, in y, destination);
    }

    /// <summary>Element-wise <c>x == y</c> comparison.</summary>

    public static void Equals<T>(IReadOnlySeries<T> x, T y, ISeries<bool> destination)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Equals(x, y, destination.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when every element equals the corresponding element of the other sequence (or scalar).</summary>

    public static bool EqualsAll<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        var xSpan = x.Vector.AsSpan();
        var ySpan = y.Vector.AsSpan();
        return VectorPrimitives.EqualsAll(in xSpan, in ySpan);
    }

    /// <summary>Returns <see langword="true"/> when every element equals the corresponding element of the other sequence (or scalar).</summary>

    public static bool EqualsAll<T>(this IReadOnlySeries<T> x, T y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        var xSpan = x.Vector.AsSpan();
        return VectorPrimitives.EqualsAll(in xSpan, in y);
    }

    /// <summary>Returns <see langword="true"/> when any element equals the corresponding element of the other sequence (or scalar).</summary>

    public static bool EqualsAny<T>(this IReadOnlySeries<T> x, IReadOnlySeries<T> y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        NumericsArgumentException.ThrowIfNot(x.Length == y.Length);
        var xSpan = x.Vector.AsSpan();
        var ySpan = y.Vector.AsSpan();
        return VectorPrimitives.EqualsAny(in xSpan, in ySpan);
    }

    /// <summary>Returns <see langword="true"/> when any element equals the corresponding element of the other sequence (or scalar).</summary>

    public static bool EqualsAny<T>(this IReadOnlySeries<T> x, T y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        var xSpan = x.Vector.AsSpan();
        return VectorPrimitives.EqualsAny(in xSpan, in y);
    }
}
