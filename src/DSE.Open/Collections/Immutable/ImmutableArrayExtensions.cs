// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Immutable;

/// <summary>
/// Provides extension methods for <see cref="ImmutableArray{T}"/> that operate on its underlying span.
/// </summary>
public static class ImmutableArrayExtensions
{
    /// <summary>
    /// Searches for the specified value and returns <see langword="true"/> if found. If not found,
    /// returns <see langword="false"/>. Values are compared using <see cref="IEquatable{T}.Equals(T)"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsValue<T>(this ImmutableArray<T> array, T value)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().Contains(value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the array contains any of the specified values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAnyValues<T>(this ImmutableArray<T> array, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().ContainsAny(values);
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="value"/>, or -1 if not found.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfValue<T>(this ImmutableArray<T> array, T value)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().IndexOf(value);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="value"/>, or -1 if not found.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfValue<T>(this ImmutableArray<T> array, T value)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().LastIndexOf(value);
    }

    /// <summary>
    /// Returns the zero-based index of the first element that matches any of the specified values, or -1 if none match.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfAnyValues<T>(this ImmutableArray<T> array, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().IndexOfAny(values);
    }

    /// <summary>
    /// Returns the zero-based index of the last element that matches any of the specified values, or -1 if none match.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfAnyValues<T>(this ImmutableArray<T> array, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().LastIndexOfAny(values);
    }

    /// <summary>
    /// Searches for the first index of any of the specified values in each row of the 2D array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static ImmutableArray<int> IndexesOfAnyValues<T>(
        this ImmutableArray<ImmutableArray<T>> array2D,
        ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        var indexes = new int[array2D.Length];

        for (var r = 0; r < array2D.Length; r++)
        {
            var row = array2D[r].AsSpan();
            indexes[r] = row.IndexOfAny(values);
        }

        return ImmutableArray.Create(indexes);
    }

    /// <summary>
    /// Determines if all of the rows of the 2D array contains at least one of the specified values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array2D"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool AllContainAnyValues<T>(
        this ImmutableArray<ImmutableArray<T>> array2D,
        ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        foreach (var arr in array2D)
        {
            if (!arr.AsSpan().ContainsAny(values))
            {
                return false;
            }
        }

        return true;
    }
}
