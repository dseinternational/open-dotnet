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
public readonly struct NumericMatrix<T> : IEquatable<NumericMatrix<T>>
    where T : struct, INumber<T>
{
    private readonly Memory2D<T> _data;

    /// <summary>The shared empty matrix.</summary>
    public static readonly NumericMatrix<T> Empty;

    /// <summary>Creates a matrix of the given dimensions backed by a fresh <c>T[rows*columns]</c>, zero-initialised.</summary>
    public NumericMatrix(int rows, int columns) : this(new Memory2D<T>(new T[rows * columns], rows, columns))
    {
    }

    /// <summary>Wraps a 2-D array as a matrix; no copy is made.</summary>
    public NumericMatrix(T[,] data) : this(new Memory2D<T>(data))
    {
    }

    /// <summary>Wraps a flat row-major <paramref name="data"/> array of <c>rows*columns</c> elements as a matrix.</summary>
    public NumericMatrix(T[] data, int rows, int columns)
        : this(new Memory2D<T>(data, rows, columns))
    {
    }

    /// <summary>Builds a matrix by copying a jagged <paramref name="data"/> array into a 2-D array.</summary>
    public NumericMatrix(T[][] data)
        : this(new Memory2D<T>(data.ToArray2D()))
    {
    }

    /// <summary>Wraps a <see cref="Memory2D{T}"/> as a matrix.</summary>
    public NumericMatrix(Memory2D<T> values)
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

    /// <summary>Gets or sets the element at <paramref name="index"/>.</summary>
#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers
    public T this[MatrixIndex index]
#pragma warning restore CA1043 // Use Integral Or String Argument For Indexers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data.Span[index.Row, index.Column];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data.Span[index.Row, index.Column] = value;
    }

    /// <summary>Gets the number of rows.</summary>
    public int RowCount => _data.Height;

    /// <summary>Gets the number of columns.</summary>
    public int ColumnCount => _data.Width;

    /// <summary>
    /// Gets a <see cref="Span{T}"/> representing the specified row of the matrix.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public Span<T> RowSpan(int row)
    {
        return _data.Span.GetRowSpan(row);
    }

    /// <summary>
    /// Adds the specified matrix to this matrix and returns a <b>new</b> matrix representing
    /// the result of the operation.
    /// </summary>
    /// <param name="other">The other matrix to add to this matrix.</param>
    /// <returns></returns>
    public NumericMatrix<T> Add(NumericMatrix<T> other)
    {
        var destination = new NumericMatrix<T>(RowCount, ColumnCount);
        Matrix.Add(this, other, destination);
        return destination;
    }

    /// <summary>Returns <see langword="true"/> when the underlying <see cref="Memory2D{T}"/> instances refer to the same data and shape.</summary>
    public bool Equals(NumericMatrix<T> other)
    {
        return _data.Equals(other._data);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is NumericMatrix<T> matrix && Equals(matrix);
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

    /// <summary>Equality operator. See <see cref="Equals(NumericMatrix{T})"/>.</summary>
    public static bool operator ==(NumericMatrix<T> left, NumericMatrix<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>Inequality operator. See <see cref="Equals(NumericMatrix{T})"/>.</summary>
    public static bool operator !=(NumericMatrix<T> left, NumericMatrix<T> right)
    {
        return !(left == right);
    }

    /// <summary>Implicitly returns a read-only view of <paramref name="matrix"/>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlyNumericMatrix<T>(NumericMatrix<T> matrix)
    {
        return new(matrix._data);
    }
}
