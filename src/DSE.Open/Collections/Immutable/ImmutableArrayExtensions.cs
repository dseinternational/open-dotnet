// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace DSE.Open.Collections.Immutable;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfValue<T>(this ImmutableArray<T> array, T value)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().IndexOf(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfValue<T>(this ImmutableArray<T> array, T value)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().LastIndexOf(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfAny<T>(this ImmutableArray<T> array, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().IndexOfAny(values);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfAny<T>(this ImmutableArray<T> array, ReadOnlySpan<T> values)
        where T : struct, IEquatable<T>
    {
        return array.AsSpan().LastIndexOfAny(values);
    }
}
