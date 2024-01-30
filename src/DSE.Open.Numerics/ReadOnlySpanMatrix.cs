// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

/// <summary>
/// <b>[Experimental]</b> A matrix of numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly ref struct ReadOnlySpanMatrix<T>
    where T : struct, INumber<T>
{
    private readonly ReadOnlySpan2D<T> _data;

    public ReadOnlySpanMatrix(Span2D<T> data)
    {
        _data = data;
    }

    internal ReadOnlySpan2D<T> Span => _data;

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    public ReadOnlySpan<T> this[int row] => _data.GetRowSpan(row);

    public T this[int row, int column] => _data[row, column];
}
