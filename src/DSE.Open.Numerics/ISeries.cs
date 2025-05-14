// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
public interface ISeries : IReadOnlySeries
{
    new string? Name { get; set; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    IReadOnlySeries AsReadOnly();
}

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
public interface ISeries<T>
    : ISeries,
      IReadOnlySeries<T>
    where T : IEquatable<T>
{
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    new ReadOnlySeries<T> AsReadOnly();

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    Span<T> AsSpan();

    new ISeries<T> Slice(int start, int length);
}
