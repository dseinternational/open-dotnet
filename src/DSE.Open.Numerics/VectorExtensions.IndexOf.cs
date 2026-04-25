// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Extension methods over <see cref="IReadOnlyVector{T}"/> and
/// <see cref="IVector{T}"/>.
/// </summary>
public static partial class VectorExtensions
{
    /// <summary>
    /// Returns the zero-based index of the first occurrence of
    /// <paramref name="value"/> in <paramref name="vector"/>, or <c>-1</c> when
    /// the value is not present.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The value to find.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    public static int IndexOf<T>(this IReadOnlyVector<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().IndexOf(value);
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of the sequence
    /// <paramref name="value"/> in <paramref name="vector"/>, or <c>-1</c>
    /// when the sequence is not present.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The sequence to find.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    public static int IndexOf<T>(this IReadOnlyVector<T> vector, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().IndexOf(value);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of
    /// <paramref name="value"/> in <paramref name="vector"/>, or <c>-1</c> when
    /// the value is not present.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The value to find.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    public static int LastIndexOf<T>(this IReadOnlyVector<T> vector, T value)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return vector.AsSpan().LastIndexOf(value);
    }
}
