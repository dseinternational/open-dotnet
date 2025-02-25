// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

public static partial class MatrixPrimitives
{
    public static void Multiply<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, Span2D<T> destination)
        where T : struct, INumber<T>
    {
        GuardMatrix.EnsureSameDimensions(x, y, destination);

        if (x.TryGetSpan(out var thisSpan)
            && y.TryGetSpan(out var otherSpan)
            && destination.TryGetSpan(out var destSpan))
        {
            Debug.Assert(thisSpan.Length == otherSpan.Length);
            TensorPrimitives.Multiply(thisSpan, otherSpan, destSpan);
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
