// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using DSE.Open.Memory;

namespace DSE.Open.Memory;

public static partial class Span2DExtensions
{
    /// <summary>
    /// Returns a value indicating whether the 2D span contains the specified value. The
    /// span is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this Span2D<T> matrix, T value)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).Contains(value);
    }

    /// <summary>
    /// Returns a value indicating whether the 2D span contains the specified value. The
    /// span is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains<T>(this ReadOnlySpan2D<T> matrix, T value)
        where T : struct, IEquatable<T>
    {
        if (matrix.TryGetSpan(out var span))
        {
            return span.Contains(value);
        }
        else
        {
            for (var r = 0; r < matrix.Height; r++)
            {
                var row = matrix.GetRowSpan(r);

                if (row.Contains(value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the matrix and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this Span2D<T> matrix, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the matrix and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> matrix, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        if (matrix.TryGetSpan(out var span))
        {
            return span.ContainsAny(values);
        }
        else
        {
            for (var r = 0; r < matrix.Height; r++)
            {
                var row = matrix.GetRowSpan(r);

                if (row.ContainsAny(values))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the matrix and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this Span2D<T> matrix, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the matrix and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> matrix, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        if (matrix.TryGetSpan(out var span))
        {
            return span.ContainsAny(values);
        }
        else
        {
            for (var r = 0; r < matrix.Height; r++)
            {
                var row = matrix.GetRowSpan(r);

                if (row.ContainsAny(values))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the 2D span
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index2D IndexOf<T>(this Span2D<T> matrix, T value)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).IndexOf(value);
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the 2D span
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Index2D IndexOf<T>(this ReadOnlySpan2D<T> matrix, T value)
        where T : struct, IEquatable<T>
    {
        if (matrix.TryGetSpan(out var span))
        {
            var index = span.IndexOf(value);

            if (index >= 0)
            {
                var row = index / matrix.Width;
                var column = index % matrix.Width;
                return new Index2D(row, column);
            }
        }
        else
        {
            for (var r = 0; r < matrix.Height; r++)
            {
                var row = matrix.GetRowSpan(r);
                var index = row.IndexOf(value);

                if (index >= 0)
                {
                    return new Index2D(r, index);
                }
            }
        }

        return new Index2D(-1, -1);
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the 2D span, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOf<T>(this Span2D<T> matrix, T value)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).RowIndexesOf(value);
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the 2D span, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ReadOnlySpan<Index2D> RowIndexesOf<T>(this ReadOnlySpan2D<T> matrix, T value)
        where T : struct, IEquatable<T>
    {
        var indexes = new Index2D[matrix.Height];

        for (var r = 0; r < matrix.Height; r++)
        {
            var row = matrix.GetRowSpan(r);
            indexes[r] = (r, row.IndexOf(value));
        }

        return indexes;
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this Span2D<T> matrix, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).RowIndexesOfAny(values);
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this ReadOnlySpan2D<T> matrix, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        var indexes = new Index2D[matrix.Height];

        for (var r = 0; r < matrix.Height; r++)
        {
            var row = matrix.GetRowSpan(r);
            indexes[r] = (r, row.IndexOfAny(values));
        }

        return indexes;
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this Span2D<T> matrix, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)matrix).RowIndexesOfAny(values);
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this ReadOnlySpan2D<T> matrix, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        var indexes = new Index2D[matrix.Height];

        for (var r = 0; r < matrix.Height; r++)
        {
            var row = matrix.GetRowSpan(r);
            indexes[r] = (r, row.IndexOfAny(values));
        }

        return indexes;
    }
}
