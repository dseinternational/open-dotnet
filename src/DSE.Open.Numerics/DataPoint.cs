// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Factory helpers for creating <see cref="DataPoint{T}"/> and
/// <see cref="DataPoint3D{T}"/> instances individually or in bulk.
/// </summary>
public static class DataPoint
{
    /// <summary>Creates a 2-D <see cref="DataPoint{T}"/>.</summary>
    public static DataPoint<T> Create<T>(T x, T y)
        where T : struct, INumber<T>
    {
        return new DataPoint<T>(x, y);
    }

    /// <summary>Creates a 3-D <see cref="DataPoint3D{T}"/>.</summary>
    public static DataPoint3D<T> Create<T>(T x, T y, T z)
        where T : struct, INumber<T>
    {
        return new DataPoint3D<T>(x, y, z);
    }

    /// <summary>
    /// Creates a sequence of 2-D data points by zipping two parallel coordinate lists.
    /// </summary>
    /// <param name="x">The X coordinates.</param>
    /// <param name="y">The Y coordinates. Must have the same length as <paramref name="x"/>.</param>
    /// <exception cref="NumericsArgumentException"><paramref name="x"/> and <paramref name="y"/> have different lengths.</exception>
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

    /// <summary>
    /// Creates a sequence of 3-D data points by zipping three parallel coordinate lists.
    /// </summary>
    /// <param name="x">The X coordinates.</param>
    /// <param name="y">The Y coordinates. Must have the same length as <paramref name="x"/>.</param>
    /// <param name="z">The Z coordinates. Must have the same length as <paramref name="x"/>.</param>
    /// <exception cref="NumericsArgumentException">Inputs have different lengths.</exception>
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
