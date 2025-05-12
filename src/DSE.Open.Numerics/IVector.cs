// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IVector : IReadOnlyVector
{
    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    ReadOnlyVector AsReadOnly();
}

public interface IVector<T>
    : IVector,
      IReadOnlyVector<T>,
      IEquatable<IVector<T>>
{
    new Memory<T> Data { get; }

    new T this[int index] { get; set; }

    bool IsReadOnly { get; }

    new IDictionary<string, T> Categories { get; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    new ReadOnlyVector<T> AsReadOnly();

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    Span<T> AsSpan();

    new Span<T> Slice(int start, int length);
}
