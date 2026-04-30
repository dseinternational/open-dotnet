// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Memory;
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

/// <summary>
/// Extension methods for spans and memory.
/// </summary>
public static partial class MemoryExtensions
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
        return ((ReadOnlySpan<T>)data).ToArray2D(rows, columns);
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
        if (data.Length < rows * columns)
        {
            ThrowHelper.ThrowArgumentException("The size of the target array is too small.", nameof(rows));
            return null!; // unreachable
        }

        var result = new T[rows, columns];

        data.CopyToArray2D(result);

        return result;
    }

    /// <summary>
    /// Copies the elements of <paramref name="data"/> into the supplied 2-dimensional array,
    /// laid out in row-major order.
    /// </summary>
    public static void CopyToArray2D<T>(this Span<T> data, T[,] destination)
    {
        ((ReadOnlySpan<T>)data).CopyToArray2D(destination);
    }

    /// <summary>
    /// Copies the elements of <paramref name="data"/> into the supplied 2-dimensional array,
    /// laid out in row-major order.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="destination"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="destination"/> is too small to hold <paramref name="data"/>.</exception>
    public static void CopyToArray2D<T>(this ReadOnlySpan<T> data, T[,] destination)
    {
        ArgumentNullException.ThrowIfNull(destination);

        var rows = destination.GetLength(0);
        var columns = destination.GetLength(1);

        if (data.Length > rows * columns)
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
