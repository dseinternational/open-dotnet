// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IVector : IReadOnlyVector
{
}

public interface IVector<T>
    : IVector,
      IReadOnlyVector<T>,
      IEquatable<IVector<T>>
{
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    new Span<T> Span { get; }

    new MemoryEnumerator<T> GetEnumerator();
}
