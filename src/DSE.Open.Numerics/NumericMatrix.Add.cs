// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Matrix
{
    public static void Add<T>(ReadOnlyNumericMatrix<T> x, T y, NumericMatrix<T> destination)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.Add(x.Span, y, destination.Span);
    }

    public static void Add<T>(ReadOnlyNumericMatrix<T> x, ReadOnlyNumericMatrix<T> y, NumericMatrix<T> destination)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    public static void AddInPlace<T>(NumericMatrix<T> x, T y)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.AddInPlace(x.Span, y);
    }

    public static void AddInPlace<T>(NumericMatrix<T> x, ReadOnlyNumericMatrix<T> y)
        where T : struct, INumber<T>
    {
        NumericMatrixPrimitives.AddInPlace(x.Span, y.Span);
    }
}
