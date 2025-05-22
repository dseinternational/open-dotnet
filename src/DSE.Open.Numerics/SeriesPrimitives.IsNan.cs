// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class SeriesPrimitives
{
    public static void IsNaN<T>([NotNull] this IReadOnlySeries<T> x, Span<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        x.Vector.IsNaN(destination);
    }

    public static void IsNaN<T>([NotNull] this IReadOnlySeries<T> x, [NotNull] ISeries<bool> destination)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(destination);
        x.Vector.IsNaN(destination.AsSpan());
    }

    public static bool IsNaNAll<T>([NotNull] this IReadOnlySeries<T> x)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.IsNaNAll();
    }

    public static bool IsNaNAny<T>([NotNull] this IReadOnlySeries<T> x)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        return x.Vector.IsNaNAny();
    }
}
