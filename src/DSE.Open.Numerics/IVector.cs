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
{
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    new Span<T> AsSpan();

    new IVector<T> Slice(int start, int length);
}
