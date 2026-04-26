// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

/// <summary>
/// Element-wise primitive operations over <see cref="ReadOnlySpan2D{T}"/> and
/// <see cref="Span2D{T}"/>. Where the underlying spans are contiguous the
/// operation runs as a single <see cref="System.Numerics.Tensors.TensorPrimitives"/>
/// call; otherwise it falls back to a row-by-row loop.
/// </summary>
public static partial class NumericMatrixPrimitives
{
    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/>, writing into <paramref name="destination"/>.</summary>
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

    /// <summary>Element-wise <paramref name="x"/> + <paramref name="y"/>, written to <paramref name="destination"/>.</summary>
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

    /// <summary>Adds the scalar <paramref name="y"/> to every element of <paramref name="x"/> in place.</summary>
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

    /// <summary>Element-wise <paramref name="x"/> += <paramref name="y"/> in place.</summary>
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
