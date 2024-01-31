// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

/// <summary>
/// <b>[Experimental]</b> A read-only matrix of numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>This is an <b>experimental</b> exploration of a matrix type implemented over
/// <see cref="ReadOnlyMemory2D{T}"/> and utilising <see cref="TensorPrimitives"/>.
/// </remarks>
public readonly struct ReadOnlyMatrix<T> : IEquatable<ReadOnlyMatrix<T>>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMemory2D<T> _data;

    public static readonly ReadOnlyMemory2D<T> Empty;

    public ReadOnlyMatrix(int rows, int columns)
        : this(new ReadOnlyMemory2D<T>(new T[rows * columns], rows, columns))
    {
    }

    public ReadOnlyMatrix(T[,] data) : this(new Memory2D<T>(data))
    {
    }

    public ReadOnlyMatrix(T[] data, int rows, int columns) : this(new Memory2D<T>(data, rows, columns))
    {
    }

    internal ReadOnlyMatrix(ReadOnlyMemory2D<T> values)
    {
        _data = values;
    }

    internal ReadOnlySpan2D<T> Span => _data.Span;

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    public ReadOnlySpan<T> this[int row] => _data.Span.GetRowSpan(row);

    public T this[int row, int column] => _data.Span[row, column];

    public Matrix<T> Add(Matrix<T> other)
    {
        var destination = new Matrix<T>(RowCount, ColumnCount);
        MatrixMath.Add(this, other, destination);
        return destination;
    }

    public bool Equals(ReadOnlyMatrix<T> other)
    {
        return _data.Equals(other._data);
    }

    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyMatrix<T> matrix && Equals(matrix);
    }

    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }

    public static bool operator ==(ReadOnlyMatrix<T> left, ReadOnlyMatrix<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyMatrix<T> left, ReadOnlyMatrix<T> right)
    {
        return !(left == right);
    }
}
