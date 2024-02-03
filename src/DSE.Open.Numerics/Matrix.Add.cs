// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Matrix
{
    public static void Add<T>(ReadOnlyMatrix<T> x, T y, Matrix<T> destination)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.Add(x.Span, y, destination.Span);
    }

    public static void Add<T>(ReadOnlySpanMatrix<T> x, T y, SpanMatrix<T> destination)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.Add(x.Span, y, destination.Span);
    }

    public static void Add<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y, Matrix<T> destination)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    public static void Add<T>(ReadOnlySpanMatrix<T> x, ReadOnlySpanMatrix<T> y, SpanMatrix<T> destination)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.Add(x.Span, y.Span, destination.Span);
    }

    public static void AddInPlace<T>(Matrix<T> x, T y)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.AddInPlace(x.Span, y);
    }

    public static void AddInPlace<T>(SpanMatrix<T> x, T y)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.AddInPlace(x.Span, y);
    }

    public static void AddInPlace<T>(Matrix<T> x, ReadOnlyMatrix<T> y)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.AddInPlace(x.Span, y.Span);
    }

    public static void AddInPlace<T>(SpanMatrix<T> x, ReadOnlySpanMatrix<T> y)
        where T : struct, INumber<T>
    {
        MatrixPrimitives.AddInPlace(x.Span, y.Span);
    }
}
