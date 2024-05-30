// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Enumerables;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// <b>[Experimental]</b> A matrix of numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>This is an <b>experimental</b> exploration of a matrix type implemented over
/// <see cref="ReadOnlySpan2D{T}"/> and utilising <see cref="TensorPrimitives"/>.
/// </remarks>
public readonly ref struct ReadOnlySpanMatrix<T> // : IReadOnlyMatrix<T> (https://github.com/dotnet/csharplang/issues/7608)
    where T : struct, INumber<T>
{
    private readonly ReadOnlySpan2D<T> _data;

    public ReadOnlySpanMatrix(int rows, int columns)
        : this(new Span2D<T>(new T[rows * columns], rows, columns))
    {
    }

    public ReadOnlySpanMatrix(T[,] data) : this(new Span2D<T>(data))
    {
    }

    public ReadOnlySpanMatrix(T[] data, int rows, int columns)
        : this(new Span2D<T>(data, rows, columns))
    {
    }

    public ReadOnlySpanMatrix(T[][] data)
        : this(new Span2D<T>(data.ToArray2D()))
    {
    }

    public ReadOnlySpanMatrix(Span<T> sequence, int rows, int columns)
        : this(Span2D<T>.DangerousCreate(ref sequence.DangerousGetReference(), rows, columns, 0))
    {
    }

    public ReadOnlySpanMatrix(ReadOnlySpan2D<T> data)
    {
        _data = data;
    }

    internal ReadOnlySpan2D<T> Span => _data;

    public T this[int row, int column] => _data[row, column];

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    public ReadOnlySpan<T> Row(int row)
    {
        return _data.GetRowSpan(row);
    }

    public ReadOnlyRefEnumerable<T> RowItems(int row)
    {
        return _data.GetRow(row);
    }

    public ReadOnlyRefEnumerable<T> ColumnItems(int row)
    {
        return _data.GetColumn(row);
    }

    public ReadOnlySpanMatrix<T> Add(T value)
    {
        var result = new SpanMatrix<T>(RowCount, ColumnCount);
        Matrix.Add(this, value, result);
        return result;
    }

    public ReadOnlySpanMatrix<T> Add(ReadOnlySpanMatrix<T> other)
    {
        var result = new SpanMatrix<T>(RowCount, ColumnCount);
        Matrix.Add(this, other, result);
        return result;
    }

    public static ReadOnlySpanMatrix<T> operator +(ReadOnlySpanMatrix<T> x, ReadOnlySpanMatrix<T> y)
    {
        return x.Add(y);
    }

    public static implicit operator ReadOnlySpanMatrix<T>(T[,] matrix)
    {
        return new(matrix);
    }
}
