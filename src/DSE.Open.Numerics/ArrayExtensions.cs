// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public static class ArrayExtensions
{
    /// <summary>
    /// Copys the data from a jagged array to a new 2-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T[,] ToArray2D<T>(this T[][] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var rows = data.Length;
        var columns = data[0].Length;

        var result = new T[rows, columns];

        for (var i = 0; i < rows; i++)
        {
            var row = data[i];

            for (var j = 0; j < columns; j++)
            {
                result[i, j] = row[j];
            }
        }

        return result;
    }

    /// <summary>
    /// Copys the data from a 1-dimensional array to a new 2-dimensional array
    /// with the specified number of rows and columns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    public static T[,] ToArray2D<T>(this T[] data, int rows, int columns)
    {
        ArgumentNullException.ThrowIfNull(data);

        var result = new T[rows, columns];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                result[i, j] = data[(i * columns) + j];
            }
        }

        return result;
    }
}

public static class MemoryExtensions
{
    /// <summary>
    /// Copys the data from a sequence of values to a new 2-dimensional array
    /// with the specified number of rows and columns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    public static T[,] ToArray2D<T>(this Span<T> data, int rows, int columns)
    {
        return ToArray2D((ReadOnlySpan<T>)data, rows, columns);
    }

    /// <summary>
    /// Copys the data from a sequence of values to a new 2-dimensional array
    /// with the specified number of rows and columns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    public static T[,] ToArray2D<T>(this ReadOnlySpan<T> data, int rows, int columns)
    {
        if (data.Length > (rows * columns))
        {
            ThrowHelper.ThrowArgumentException("The size of the target array is too small.", nameof(rows));
            return null!; // unreachable
        }

        var result = new T[rows, columns];

        data.CopyToArray2D(result);

        return result;
    }

    public static void CopyToArray2D<T>(this Span<T> data, T[,] destination)
    {
        CopyToArray2D((ReadOnlySpan<T>)data, destination);
    }

    public static void CopyToArray2D<T>(this ReadOnlySpan<T> data, T[,] destination)
    {
        ArgumentNullException.ThrowIfNull(destination);

        var rows = destination.GetLength(0);
        var columns = destination.GetLength(1);

        if (data.Length > (rows * columns))
        {
            ThrowHelper.ThrowArgumentException("The destination is too small.", nameof(destination));
            return; // unreachable
        }

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                destination[i, j] = data[(i * columns) + j];
            }
        }
    }
}
