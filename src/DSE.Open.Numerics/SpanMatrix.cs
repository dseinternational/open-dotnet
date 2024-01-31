// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

/// <summary>
/// <b>[Experimental]</b> A matrix of numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>This is an <b>experimental</b> exploration of a matrix type implemented over
/// <see cref="Span2D{T}"/> and utilising <see cref="TensorPrimitives"/>.
/// </remarks>
public readonly ref struct SpanMatrix<T>
    where T : struct, INumber<T>
{
    private readonly Span2D<T> _data;

    public SpanMatrix(Span2D<T> data)
    {
        _data = data;
    }

    internal Span2D<T> Span => _data;

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    public Span<T> this[int row] => _data.GetRowSpan(row);

    public T this[int row, int column]
    {
        get => _data[row, column];
        set => _data[row, column] = value;
    }
}
