// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

internal static class GuardMatrix
{
    public static bool HaveSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y)
        where T : struct, INumber<T>
    {
        return x.RowCount == y.RowCount && x.ColumnCount == y.ColumnCount;
    }

    public static bool HaveSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y, ReadOnlyMatrix<T> z)
        where T : struct, INumber<T>
    {
        return x.RowCount == y.RowCount && x.ColumnCount == y.ColumnCount
            && x.RowCount == z.RowCount && x.ColumnCount == z.ColumnCount;
    }

    public static bool HaveSameDimensions<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y)
        where T : struct, INumber<T>
    {
        return x.Height == y.Width && x.Height == y.Width;
    }

    public static bool HaveSameDimensions<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, ReadOnlySpan2D<T> z)
        where T : struct, INumber<T>
    {
        return x.Height == y.Height && x.Width == y.Width
            && x.Height == z.Height && x.Width == z.Width;
    }

    public static void EnsureSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y))
        {
            ThrowHelper.ThrowArgumentException("Matrices must have the same dimensions.");
        }
    }

    public static void EnsureSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y, ReadOnlyMatrix<T> z)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y, z))
        {
            ThrowHelper.ThrowArgumentException("Matrices must have the same dimensions.");
        }
    }

    public static void EnsureSameDimensions<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y))
        {
            ThrowHelper.ThrowArgumentException("Span2Ds must have the same dimensions.");
        }
    }

    public static void EnsureSameDimensions<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, Span2D<T> z)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y, z))
        {
            ThrowHelper.ThrowArgumentException("Span2Ds must have the same dimensions.");
        }
    }
}
