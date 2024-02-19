// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Matrix
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

        if (source.Length > rows * columns)
        {
            ThrowHelper.ThrowArgumentException(nameof(source),
                "The source is too large to copy to the specified number of rows and columns.");
            return default; // unreachable
        }

        var result = new Matrix<T>(rows, columns);

        for (var i = 0; i < rows; i++)
        {
            source[(i * columns)..((i * columns) + columns)].CopyTo(result.RowVector(i).Span);
        }

        return result;
    }

    public static Matrix<T> Create<T>(IReadOnlyList<IReadOnlyList<T>> source)
        where T : struct, INumber<T>
    {
        Guard.IsNotNull(source);

        if (source.Count == 0)
        {
            return Matrix<T>.Empty;
        }

        var rows = source.Count;
        var columns = source[0].Count;

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
}
