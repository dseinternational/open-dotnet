// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

public static class MatrixMath
{
    public static void Add<T>(SpanMatrix<T> x, SpanMatrix<T> y, SpanMatrix<T> destination)
        where T : struct, INumber<T>
    {
        Add(x.Span, y.Span, destination.Span);
    }

    public static void Add<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y, Matrix<T> destination)
        where T : struct, INumber<T>
    {
        Add(x.Span, y.Span, destination.Span);
    }

    public static void Add<T>(Span2D<T> x, Span2D<T> y, Span2D<T> destination)
        where T : struct, INumber<T>
    {
        Add((ReadOnlySpan2D<T>)x, (ReadOnlySpan2D<T>)y, destination);
    }

    public static void Add<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, Span2D<T> destination)
        where T : struct, INumber<T>
    {
        Matrix.EnsureSameDimensions(x, y, destination);

        if (x.TryGetSpan(out var thisSpan)
            && y.TryGetSpan(out var otherSpan)
            && destination.TryGetSpan(out var destSpan))
        {
            Debug.Assert(thisSpan.Length == otherSpan.Length);
            TensorPrimitives.Add(thisSpan, otherSpan, destSpan);
        }
        else
        {
            for (var i = 0; i < x.Height; i++)
            {
                var xRow = x.GetRowSpan(i);
                var yRow = y.GetRowSpan(i);
                var destinationRow = destination.GetRowSpan(i);
                TensorPrimitives.Add(xRow, yRow, destinationRow);
            }
        }
    }
}
