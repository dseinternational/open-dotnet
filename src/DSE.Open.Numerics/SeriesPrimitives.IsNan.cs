// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    /// <summary>Element-wise <c>IsNaN</c> predicate.</summary>
    public static void IsNaN<T>([NotNull] this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        x.Vector.IsNaN(destination);
    }

    /// <summary>Element-wise <c>IsNaN</c> predicate.</summary>

    public static void IsNaN<T>([NotNull] this IReadOnlySeries<T> x, [NotNull] ISeries<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(destination);
        x.Vector.IsNaN(destination.AsSpan());
    }

    /// <summary>Returns <see langword="true"/> when every element is NaN.</summary>

    public static bool IsNaNAll<T>([NotNull] this IReadOnlySeries<T> x)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.IsNaNAll();
    }

    /// <summary>Returns <see langword="true"/> when any element is NaN.</summary>

    public static bool IsNaNAny<T>([NotNull] this IReadOnlySeries<T> x)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.IsNaNAny();
    }
}
