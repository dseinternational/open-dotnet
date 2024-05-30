// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// <b>[Experimental]</b> A matrix of numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>This is an <b>experimental</b> exploration of a matrix type implemented over
/// <see cref="Memory2D{T}"/> and utilising <see cref="TensorPrimitives"/>.
/// </remarks>
[CollectionBuilder(typeof(Matrix), "Create")]
public readonly struct Matrix<T> : IEquatable<Matrix<T>>
    where T : struct, INumber<T>
{
    private readonly Memory2D<T> _data;

    public static readonly Matrix<T> Empty;

    public Matrix(int rows, int columns) : this(new Memory2D<T>(new T[rows * columns], rows, columns))
    {
    }

    public Matrix(T[,] data) : this(new Memory2D<T>(data))
    {
    }

    public Matrix(T[] data, int rows, int columns)
        : this(new Memory2D<T>(data, rows, columns))
    {
    }

    public Matrix(T[][] data)
        : this(new Memory2D<T>(data.ToArray2D()))
    {
    }

    public Matrix(Memory2D<T> values)
    {
        _data = values;
    }

    internal Span2D<T> Span => _data.Span;

    /// <summary>
    /// Gets or sets the value at the specified row and column of the matrix.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public T this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[row, column];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data.Span[row, column] = value;
    }

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
    public T this[MatrixIndex index]
#pragma warning restore CA1043 // Use Integral Or String Argument For Indexers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[index.Row, index.Column];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data.Span[index.Row, index.Column] = value;
    }

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    /// <summary>
    /// Gets a <see cref="SpanVector{T}"/> representing the specified row of the matrix.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public SpanVector<T> RowVector(int row)
    {
        return new(_data.Span.GetRowSpan(row));
    }

    /// <summary>
    /// Adds the specified matrix to this matrix and returns a <b>new</b> matrix representing
    /// the result of the operation.
    /// </summary>
    /// <param name="other">The other matrix to add to this matrix.</param>
    /// <returns></returns>
    public Matrix<T> Add(Matrix<T> other)
    {
        var destination = new Matrix<T>(RowCount, ColumnCount);
        Matrix.Add(this, other, destination);
        return destination;
    }

    public bool Equals(Matrix<T> other)
    {
        return _data.Equals(other._data);
    }

    public override bool Equals(object? obj)
    {
        return obj is Matrix<T> matrix && Equals(matrix);
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

    public static bool operator ==(Matrix<T> left, Matrix<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Matrix<T> left, Matrix<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyMatrix<T>(Matrix<T> matrix)
    {
        return new(matrix._data);
    }
}
