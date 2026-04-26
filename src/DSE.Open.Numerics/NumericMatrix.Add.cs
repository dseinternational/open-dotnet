// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>Static-only matrix operations and factories.</summary>
public static partial class Matrix
{
    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/>, writing the result to <paramref name="destination"/>.</summary>
    public static void Add<T>(ReadOnlyNumericMatrix<T> x, T y, NumericMatrix<T> destination)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.Add(x.Span, y, destination.Span);
    }

    /// <summary>Element-wise <paramref name="x"/> + <paramref name="y"/>, written to <paramref name="destination"/>.</summary>
    public static void Add<T>(ReadOnlyNumericMatrix<T> x, ReadOnlyNumericMatrix<T> y, NumericMatrix<T> destination)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/> in place.</summary>
    public static void AddInPlace<T>(NumericMatrix<T> x, T y)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.AddInPlace(x.Span, y);
    }

    /// <summary>Element-wise <paramref name="x"/> += <paramref name="y"/> in place.</summary>
    public static void AddInPlace<T>(NumericMatrix<T> x, ReadOnlyNumericMatrix<T> y)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.AddInPlace(x.Span, y.Span);
    }
}
