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

    /// <summary>The shared empty backing storage for default-initialized read-only matrices.</summary>
    public static readonly ReadOnlyMemory2D<T> Empty;

    /// <summary>Creates a read-only matrix of the given dimensions backed by a fresh zero-initialised <c>T[rows*columns]</c>.</summary>
    public ReadOnlyNumericMatrix(int rows, int columns)
        : this(new ReadOnlyMemory2D<T>(new T[rows * columns], rows, columns))
    {
    }

    /// <summary>Wraps a 2-D array as a read-only matrix; no copy is made.</summary>
    public ReadOnlyNumericMatrix(T[,] data) : this(new Memory2D<T>(data))
    {
    }

    /// <summary>Wraps a flat row-major <paramref name="data"/> array of <c>rows*columns</c> elements as a read-only matrix.</summary>
    public ReadOnlyNumericMatrix(T[] data, int rows, int columns) : this(new Memory2D<T>(data, rows, columns))
    {
    }

    /// <summary>Wraps a <see cref="ReadOnlyMemory2D{T}"/> as a read-only matrix.</summary>
    public ReadOnlyNumericMatrix(ReadOnlyMemory2D<T> values)
    {
        _data = values;
    }

    internal ReadOnlySpan2D<T> Span => _data.Span;

    /// <summary>Gets the element at the given row and column.</summary>
    public T this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[row, column];
    }

    /// <summary>Gets the element at the given <paramref name="index"/>.</summary>
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
    public T this[MatrixIndex index]
#pragma warning restore CA1043 // Use Integral Or String Argument For Indexers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[index.Row, index.Column];
    }

    /// <summary>Gets the number of rows.</summary>
    public int RowCount => _data.Height;

    /// <summary>Gets the number of columns.</summary>
    public int ColumnCount => _data.Width;

    /// <summary>
    /// Gets a <see cref="ReadOnlySeries{T}"/> representing the specified row of the matrix.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public ReadOnlySpan<T> RowSpan(int row)
    {
        return _data.Span.GetRowSpan(row);
    }

    /// <summary>Adds <paramref name="other"/> to this matrix and returns a new matrix containing the result.</summary>
    public NumericMatrix<T> Add(NumericMatrix<T> other)
    {
        var destination = new NumericMatrix<T>(RowCount, ColumnCount);
        Matrix.Add(this, other, destination);
        return destination;
    }

    /// <summary>Returns <see langword="true"/> when the underlying <see cref="ReadOnlyMemory2D{T}"/> instances refer to the same data and shape.</summary>
    public bool Equals(ReadOnlyNumericMatrix<T> other)
    {
        return _data.Equals(other._data);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ReadOnlyNumericMatrix<T> matrix && Equals(matrix);
    }

    /// <inheritdoc />
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

    /// <summary>Equality operator. See <see cref="Equals(ReadOnlyNumericMatrix{T})"/>.</summary>
    public static bool operator ==(ReadOnlyNumericMatrix<T> left, ReadOnlyNumericMatrix<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(ReadOnlyNumericMatrix<T> left, ReadOnlyNumericMatrix<T> right)
    {
        return !(left == right);
    }
}
