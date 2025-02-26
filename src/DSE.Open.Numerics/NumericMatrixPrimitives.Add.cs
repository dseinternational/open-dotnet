// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

public static partial class NumericMatrixPrimitives
{
    public static void Add<T>(ReadOnlySpan2D<T> x, T y, Span2D<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        GuardMatrix.EnsureSameDimensions(x, destination);

        if (x.TryGetSpan(out var thisSpan)
            && destination.TryGetSpan(out var destSpan))
        {
            TensorPrimitives.Add(thisSpan, y, destSpan);
        }
        else
        {
            for (var i = 0; i < x.Height; i++)
            {
                var xRow = x.GetRowSpan(i);
                var destinationRow = destination.GetRowSpan(i);
                TensorPrimitives.Add(xRow, y, destinationRow);
            }
        }
    }

    public static void Add<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, Span2D<T> destination)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        GuardMatrix.EnsureSameDimensions(x, y, destination);

        if (x.TryGetSpan(out var thisSpan)
            && y.TryGetSpan(out var otherSpan)
            && destination.TryGetSpan(out var destSpan))
        {
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

    public static void AddInPlace<T>(Span2D<T> x, T y)
        where T : struct, INumber<T>
    {
        if (x.TryGetSpan(out var thisSpan))
        {
            TensorPrimitives.Add(thisSpan, y, thisSpan);
        }
        else
        {
            for (var i = 0; i < x.Height; i++)
            {
                var xRow = x.GetRowSpan(i);
                TensorPrimitives.Add(xRow, y, xRow);
            }
        }
    }

    public static void AddInPlace<T>(Span2D<T> x, ReadOnlySpan2D<T> y)
        where T : struct, INumber<T>
    {
        GuardMatrix.EnsureSameDimensions(x, y);

        if (x.TryGetSpan(out var thisSpan)
            && y.TryGetSpan(out var otherSpan))
        {
            TensorPrimitives.Add(thisSpan, otherSpan, thisSpan);
        }
        else
        {
            for (var i = 0; i < x.Height; i++)
            {
                var xRow = x.GetRowSpan(i);
                var yRow = y.GetRowSpan(i);
                TensorPrimitives.Add(xRow, yRow, xRow);
            }
        }
    }
}
