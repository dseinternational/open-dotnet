// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using DSE.Open.Memory;

namespace DSE.Open.Memory;

public static partial class Memory2DExtensions
{
    /// <summary>
    /// Returns a value indicating whether the 2D span contains the specified value. The
    /// span is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this Memory2D<T> memory2D, T value)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.Contains(value);
    }

    /// <summary>
    /// Returns a value indicating whether the 2D span contains the specified value. The
    /// span is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this ReadOnlyMemory2D<T> memory2D, T value)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.Contains(value);
    }

    /// <summary>
    /// Returns a value indicating whether the 2D span contains the specified value. The
    /// span is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this Span2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).Contains(value);
    }

    /// <summary>
    /// Returns a value indicating whether the 2D span contains the specified value. The
    /// span is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains<T>(this ReadOnlySpan2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        if (span2D.TryGetSpan(out var span))
        {
            return span.Contains(value);
        }
        else
        {
            for (var r = 0; r < span2D.Height; r++)
            {
                var row = span2D.GetRowSpan(r);

                if (row.Contains(value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this Memory2D<T> memory2D, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this ReadOnlyMemory2D<T> memory2D, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this Span2D<T> span2D, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> span2D, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        if (span2D.TryGetSpan(out var span))
        {
            return span.ContainsAny(values);
        }
        else
        {
            for (var r = 0; r < span2D.Height; r++)
            {
                var row = span2D.GetRowSpan(r);

                if (row.ContainsAny(values))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this Memory2D<T> memory2D, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this ReadOnlyMemory2D<T> memory2D, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this Span2D<T> span2D, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).ContainsAny(values);
    }

    /// <summary>
    /// Searches for an occurrence of any of the specified values anywhere in the span2D and
    /// returns <see langword="true"/> if found. If not found, returns <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> span2D, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        if (span2D.TryGetSpan(out var span))
        {
            return span.ContainsAny(values);
        }
        else
        {
            for (var r = 0; r < span2D.Height; r++)
            {
                var row = span2D.GetRowSpan(r);

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
    /// <param name="memory2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index2D IndexOf<T>(this Memory2D<T> memory2D, T value)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.IndexOf(value);
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the 2D span
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index2D IndexOf<T>(this ReadOnlyMemory2D<T> memory2D, T value)
        where T : struct, IEquatable<T>
    {
        return memory2D.Span.IndexOf(value);
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the 2D span
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index2D IndexOf<T>(this Span2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).IndexOf(value);
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the 2D span
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Index2D IndexOf<T>(this ReadOnlySpan2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        if (span2D.TryGetSpan(out var span))
        {
            var index = span.IndexOf(value);

            if (index >= 0)
            {
                var row = index / span2D.Width;
                var column = index % span2D.Width;
                return new Index2D(row, column);
            }
        }
        else
        {
            for (var r = 0; r < span2D.Height; r++)
            {
                var row = span2D.GetRowSpan(r);
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
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOf<T>(this Memory2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        return span2D.Span.RowIndexesOf(value);
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the 2D span, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOf<T>(this ReadOnlyMemory2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        return span2D.Span.RowIndexesOf(value);
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the 2D span, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOf<T>(this Span2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).RowIndexesOf(value);
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the 2D span, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ReadOnlySpan<Index2D> RowIndexesOf<T>(this ReadOnlySpan2D<T> span2D, T value)
        where T : struct, IEquatable<T>
    {
        var indexes = new Index2D[span2D.Height];

        for (var r = 0; r < span2D.Height; r++)
        {
            var row = span2D.GetRowSpan(r);
            indexes[r] = (r, row.IndexOf(value));
        }

        return indexes;
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this Span2D<T> span2D, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).RowIndexesOfAny(values);
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this ReadOnlySpan2D<T> span2D, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        var indexes = new Index2D[span2D.Height];

        for (var r = 0; r < span2D.Height; r++)
        {
            var row = span2D.GetRowSpan(r);
            indexes[r] = (r, row.IndexOfAny(values));
        }

        return indexes;
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this Span2D<T> span2D, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        return ((ReadOnlySpan2D<T>)span2D).RowIndexesOfAny(values);
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D span.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static ReadOnlySpan<Index2D> RowIndexesOfAny<T>(this ReadOnlySpan2D<T> span2D, SearchValues<T> values)
        where T : struct, IEquatable<T>
    {
        var indexes = new Index2D[span2D.Height];

        for (var r = 0; r < span2D.Height; r++)
        {
            var row = span2D.GetRowSpan(r);
            indexes[r] = (r, row.IndexOfAny(values));
        }

        return indexes;
    }
}
