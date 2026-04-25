// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of values.
/// </summary>
public interface IVector : IReadOnlyVector
{
}

/// <summary>
/// A serializable, fixed-length, contiguous sequence of values of type <typeparamref name="T"/>.
/// </summary>
public interface IVector<T> : IVector, IReadOnlyVector<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets or sets the element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Zero-based index of the element.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when
    /// <paramref name="index"/> is outside <c>[0, Length)</c>.</exception>
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    new Span<T> AsSpan();

    /// <summary>
    /// Returns a mutable slice of this vector starting at <paramref name="start"/>
    /// with the given <paramref name="length"/>. The slice shares the underlying
    /// storage with the source — writes through either view are visible to the other.
    /// </summary>
    /// <param name="start">Zero-based start index of the slice.</param>
    /// <param name="length">Number of elements in the slice.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when
    /// <paramref name="start"/> or <paramref name="length"/> is negative, or
    /// when <c>start + length</c> exceeds <see cref="IReadOnlyVector.Length"/>.</exception>
    new IVector<T> Slice(int start, int length);
}
