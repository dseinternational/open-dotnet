// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public static partial class MatrixExtensions
{
    /// <summary>
    /// Returns a value indicating whether the matrix contains the specified value. The
    /// matrix is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this Matrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return matrix.Span.Contains(value);
    }

    /// <summary>
    /// Returns a value indicating whether the matrix contains the specified value. The
    /// matrix is searched by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this ReadOnlyMatrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return matrix.Span.Contains(value);
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
    public static bool ContainsAny<T>(this Matrix<T> matrix, ReadOnlySpan<T> values)
        where T : struct, INumber<T>
    {
        return matrix.Span.ContainsAny(values);
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
    public static bool ContainsAny<T>(this ReadOnlyMatrix<T> matrix, ReadOnlySpan<T> values)
        where T : struct, INumber<T>
    {
        return matrix.Span.ContainsAny(values);
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
    public static bool ContainsAny<T>(this ReadOnlyMatrix<T> matrix, SearchValues<T> values)
        where T : struct, INumber<T>
    {
        return matrix.Span.ContainsAny(values);
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the matrix
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MatrixIndex IndexOf<T>(this Matrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return matrix.Span.IndexOf(value);
    }

    /// <summary>
    /// Returns the location of the first occurrence of a specified value within the matrix
    /// searching by rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MatrixIndex IndexOf<T>(this ReadOnlyMatrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return matrix.Span.IndexOf(value);
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the matrix, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<MatrixIndex> RowIndexesOf<T>(this Matrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return MemoryMarshal.Cast<Index2D, MatrixIndex>(matrix.Span.RowIndexesOf(value));
    }

    /// <summary>
    /// Returns the indexes of the first occurrences of a specified value in each row of
    /// the matrix, or -1 if the value is not found in the row.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<MatrixIndex> RowIndexesOf<T>(this ReadOnlyMatrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return MemoryMarshal.Cast<Index2D, MatrixIndex>(matrix.Span.RowIndexesOf(value));
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the matrix.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<MatrixIndex> RowIndexesOfAny<T>(this Matrix<T> matrix, ReadOnlySpan<T> values)
        where T : struct, INumber<T>
    {
        return MemoryMarshal.Cast<Index2D, MatrixIndex>(matrix.Span.RowIndexesOfAny(values));
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the matrix.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<MatrixIndex> RowIndexesOfAny<T>(this ReadOnlyMatrix<T> matrix, ReadOnlySpan<T> values)
        where T : struct, INumber<T>
    {
        return MemoryMarshal.Cast<Index2D, MatrixIndex>(matrix.Span.RowIndexesOfAny(values));
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the matrix.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<MatrixIndex> RowIndexesOfAny<T>(this Matrix<T> matrix, SearchValues<T> values)
        where T : struct, INumber<T>
    {
        return MemoryMarshal.Cast<Index2D, MatrixIndex>(matrix.Span.RowIndexesOfAny(values));
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the matrix.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<MatrixIndex> RowIndexesOfAny<T>(this ReadOnlyMatrix<T> matrix, SearchValues<T> values)
        where T : struct, INumber<T>
    {
        return MemoryMarshal.Cast<Index2D, MatrixIndex>(matrix.Span.RowIndexesOfAny(values));
    }
}
