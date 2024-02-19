// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public static partial class ArrayExtensions
{
    /// <summary>
    /// Copys the data from a jagged array to a new 2-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T[,] ToArray2D<T>(this T[][] data)
    {
        Guard.IsNotNull(data);

        if (data.Length == 0)
        {
            return new T[0, 0];
        }

        var rows = data.GetLength(0);
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
        Guard.IsNotNull(data);
        return data.AsSpan().ToArray2D(rows, columns);
    }
}
