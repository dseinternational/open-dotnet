// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
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
public readonly struct ReadOnlyNumericMatrix<T> : IEquatable<ReadOnlyNumericMatrix<T>>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMemory2D<T> _data;

    public static readonly ReadOnlyMemory2D<T> Empty;

    public ReadOnlyNumericMatrix(int rows, int columns)
        : this(new ReadOnlyMemory2D<T>(new T[rows * columns], rows, columns))
    {
    }

    public ReadOnlyNumericMatrix(T[,] data) : this(new Memory2D<T>(data))
    {
    }

    public ReadOnlyNumericMatrix(T[] data, int rows, int columns) : this(new Memory2D<T>(data, rows, columns))
    {
    }

    public ReadOnlyNumericMatrix(ReadOnlyMemory2D<T> values)
    {
        _data = values;
    }

    internal ReadOnlySpan2D<T> Span => _data.Span;

    public T this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[row, column];
    }

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
    public T this[MatrixIndex index]
#pragma warning restore CA1043 // Use Integral Or String Argument For Indexers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[index.Row, index.Column];
    }

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    /// <summary>
    /// Gets a <see cref="ReadOnlyVector{T}"/> representing the specified row of the matrix.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public ReadOnlySpan<T> RowSpan(int row)
    {
        return _data.Span.GetRowSpan(row);
    }

    public NumericMatrix<T> Add(NumericMatrix<T> other)
    {
        var destination = new NumericMatrix<T>(RowCount, ColumnCount);
        Matrix.Add(this, other, destination);
        return destination;
    }

    public bool Equals(ReadOnlyNumericMatrix<T> other)
    {
        return _data.Equals(other._data);
    }

    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyNumericMatrix<T> matrix && Equals(matrix);
    }

    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }

    /// <summary>
    /// Copies the elements of the matrix to a new 2D array.
    /// </summary>
    /// <returns></returns>
    public T[,] ToArray()
    {
        return _data.ToArray();
    }

    public static bool operator ==(ReadOnlyNumericMatrix<T> left, ReadOnlyNumericMatrix<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyNumericMatrix<T> left, ReadOnlyNumericMatrix<T> right)
    {
        return !(left == right);
    }
}
