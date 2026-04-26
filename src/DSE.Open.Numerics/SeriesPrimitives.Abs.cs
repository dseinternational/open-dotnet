// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Element-wise primitive operations over <see cref="IReadOnlySeries{T}"/> and
/// <see cref="ISeries{T}"/>. Most members delegate to the corresponding
/// <see cref="VectorPrimitives"/> kernel after extracting the underlying span;
/// the series wrappers also propagate metadata (e.g. <see cref="ISeries.Name"/>)
/// across the operation where applicable.
/// </summary>
public static partial class SeriesPrimitives
{
    /// <summary>Element-wise absolute value.</summary>
    public static void Abs<T>(this IReadOnlySeries<T> x, Span<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Abs(x.Vector.AsSpan(), destination);
    }

    /// <summary>Element-wise absolute value.</summary>

    public static void Abs<T>(this IReadOnlySeries<T> x, ISeries<T> destination)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(destination);
        Abs(x, destination.AsSpan());
    }

    /// <summary>Element-wise absolute value.</summary>

    public static Series<T> Abs<T>(this IReadOnlySeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return WrapUnary(x.Vector.Abs(), x);
    }

    /// <summary>Element-wise absolute value (in place).</summary>

    public static void AbsInPlace<T>(this ISeries<T> x)
        where T : struct, INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        VectorPrimitives.Abs(x.AsSpan(), x.AsSpan());
    }
}
