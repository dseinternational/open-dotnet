// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface ISeries : IReadOnlySeries
{
    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    IReadOnlySeries AsReadOnly();
}

public interface ISeries<T>
    : ISeries,
      IReadOnlySeries<T>,
      IEquatable<ISeries<T>>
{
    new Memory<T> Vector { get; }

    new T this[int index] { get; set; }

    bool IsReadOnly { get; }

    new IDictionary<string, T> Categories { get; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    new ReadOnlySeries<T> AsReadOnly();

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    Span<T> AsSpan();

    new Memory<T> Slice(int start, int length);
}
