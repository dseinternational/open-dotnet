// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Numerics;

public static class DataPoint
{
    public static DataPoint<T> Create<T>(T x, T y)
        where T : struct, INumber<T>
    {
        return new DataPoint<T>(x, y);
    }

    public static DataPoint3D<T> Create<T>(T x, T y, T z)
        where T : struct, INumber<T>
    {
        return new DataPoint3D<T>(x, y, z);
    }

    public static IEnumerable<DataPoint<T>> CreateRange<T>(
        [NotNull] IReadOnlyList<T> x,
        [NotNull] IReadOnlyList<T> y)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNotEqualLength(x, y);

        for (var i = 0; i < x.Count; i++)
        {
            yield return new DataPoint<T>(x[i], y[i]);
        }
    }

    public static IEnumerable<DataPoint3D<T>> CreateRange<T>(
        [NotNull] IReadOnlyList<T> x,
        [NotNull] IReadOnlyList<T> y,
        [NotNull] IReadOnlyList<T> z)
        where T : struct, INumber<T>
    {
        NumericsArgumentException.ThrowIfNotEqualLength(x, y, z);

        for (var i = 0; i < x.Count; i++)
        {
            yield return new DataPoint3D<T>(x[i], y[i], z[i]);
        }
    }
}
