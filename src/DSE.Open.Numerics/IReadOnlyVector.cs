// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyVector
{
    int Length { get; }

    bool IsNumeric { get; }

    VectorDataType DataType { get; }
}

public interface IReadOnlyVector<T>
    : IReadOnlyVector,
      IReadOnlyList<T>,
      IEquatable<IReadOnlyVector<T>>
{
    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> Span { get; }

    new ReadOnlyMemoryEnumerator<T> GetEnumerator();

    /// <summary>
    /// Copies the elements of the <see cref="IReadOnlyVector{T}"/> to a new array.
    /// </summary>
    /// <returns></returns>
    T[] ToArray();
}
