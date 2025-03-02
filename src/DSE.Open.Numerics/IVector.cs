// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IVector
{
    int Length { get; }

    bool IsNumeric { get; }

    VectorDataType DataType { get; }
}

public interface IVector<T>
    : IVector,
      IReadOnlyList<T>,
      IEquatable<IVector<T>>
{
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    Span<T> Span { get; }

    new MemoryEnumerator<T> GetEnumerator();

    /// <summary>
    /// Copies the elements of the <see cref="IVector{T}"/> to a new array.
    /// </summary>
    /// <returns></returns>
    T[] ToArray();
}
