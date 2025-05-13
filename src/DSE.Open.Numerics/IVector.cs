// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type with value equality semantics.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
public interface IVector : IReadOnlySeries
{
    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    IReadOnlySeries AsReadOnly();

    bool IsReadOnly { get; }
}

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type with value equality semantics.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
public interface IVector<T>
    : IVector,
      IReadOnlyVector<T>,
      IEquatable<IVector<T>>
{
    new Memory<T> Data { get; }

    new T this[int index] { get; set; }

    new IDictionary<string, T> Categories { get; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    new ReadOnlyVector<T> AsReadOnly();

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    Span<T> AsSpan();

    new Memory<T> Slice(int start, int length);
}
