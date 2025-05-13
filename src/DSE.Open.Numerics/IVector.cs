// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values with value
/// equality semantics.
/// </summary>
public interface IVector : IReadOnlyVector
{
}

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values with value
/// equality semantics.
/// </summary>
public interface IVector<T> : IVector, IReadOnlyVector<T>
{
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    new Span<T> AsSpan();

    new IVector<T> Slice(int start, int length);
}
