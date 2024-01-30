// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

/// <summary>
/// <b>[Experimental]</b> A matrix of numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
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

    internal Matrix(Memory2D<T> values)
    {
        _data = values;
    }

    internal Span2D<T> Span => _data.Span;

    public int RowCount => _data.Height;

    public int ColumnCount => _data.Width;

    public Span<T> this[int row] => _data.Span.GetRowSpan(row);

    public T this[int row, int column]
    {
        get => _data.Span[row, column];
        set => _data.Span[row, column] = value;
    }

    public Matrix<T> Add(Matrix<T> other)
    {
        var destination = new Matrix<T>(RowCount, ColumnCount);
        MatrixMath.Add(this, other, destination);
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

    public static bool operator ==(Matrix<T> left, Matrix<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Matrix<T> left, Matrix<T> right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (AsReadOnly())
    public static implicit operator ReadOnlyMatrix<T>(Matrix<T> matrix)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new ReadOnlyMatrix<T>(matrix._data);
    }

    public ReadOnlyMatrix<T> AsReadOnly()
    {
        return new ReadOnlyMatrix<T>(_data);
    }
}

public static class Matrix
{
    /// <summary>
    /// Creates a new matrix from a sequence of values, copying them into the specified number of
    /// rows and columns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    public static Matrix<T> Create<T>(Span<T> source, int rows, int columns)
        where T : struct, INumber<T>
    {
        return Create((ReadOnlySpan<T>)source, rows, columns);
    }

    public static Matrix<T> Create<T>(ReadOnlySpan<T> source, int rows, int columns)
        where T : struct, INumber<T>
    {
        if (source.Length == 0)
        {
            if (rows == 0 && columns == 0)
            {
                return Matrix<T>.Empty;
            }
        }

        if (source.Length > rows*columns)
        {
            ThrowHelper.ThrowArgumentException(nameof(source),
                "The source is too large to copy to the specified number of rows and columns.");
            return default; // unreachable
        }

        var result = new Matrix<T>(rows, columns);

        for (var i = 0; i < rows; i++)
        {
            source[(i * columns)..((i * columns) + columns)].CopyTo(result[i]);
        }

        return result;
    }

    public static Matrix<T> Create<T>(ReadOnlySpan<T[]> source)
        where T : struct, INumber<T>
    {
        if (source.Length == 0)
        {
            return Matrix<T>.Empty;
        }

        var rows = source.Length;
        var columns = source[0].Length;

        var result = new Matrix<T>(rows, columns);

        for (var i = 0; i < rows; i++)
        {
            var row = source[i];

            // 'missing' columns left initialized to default(T)

            for (var j = 0; j < columns; j++)
            {
                result[i, j] = row[j];
            }
        }

        return result;
    }

    public static bool HaveSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y)
        where T : struct, INumber<T>
    {
        return x.RowCount == y.RowCount && x.ColumnCount == y.ColumnCount;
    }

    public static bool HaveSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y, ReadOnlyMatrix<T> z)
        where T : struct, INumber<T>
    {
        return x.RowCount == y.RowCount && x.ColumnCount == y.ColumnCount
            && x.RowCount == z.RowCount && x.ColumnCount == z.ColumnCount;
    }

    public static bool HaveSameDimensions<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, Span2D<T> z)
        where T : struct, INumber<T>
    {
        return x.Height == y.Height && x.Width == y.Width
            && x.Height == z.Height && x.Width == z.Width;
    }

    public static void EnsureSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y))
        {
            ThrowHelper.ThrowArgumentException("Matrices must have the same dimensions.");
        }
    }

    public static void EnsureSameDimensions<T>(ReadOnlyMatrix<T> x, ReadOnlyMatrix<T> y, ReadOnlyMatrix<T> z)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y, z))
        {
            ThrowHelper.ThrowArgumentException("Matrices must have the same dimensions.");
        }
    }

    public static void EnsureSameDimensions<T>(ReadOnlySpan2D<T> x, ReadOnlySpan2D<T> y, Span2D<T> z)
        where T : struct, INumber<T>
    {
        if (!HaveSameDimensions(x, y, z))
        {
            ThrowHelper.ThrowArgumentException("Span2Ds must have the same dimensions.");
        }
    }
}
