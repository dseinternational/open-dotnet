// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Numerics;
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
    public static MatrixIndex IndexOf<T>(this ReadOnlyMatrix<T> matrix, T value)
        where T : struct, INumber<T>
    {
        return matrix.Span.IndexOf(value);
    }
}
