// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public interface ISeries<T>
    : ISeries,
      IReadOnlySeries<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets or sets the element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Zero-based element index.</param>
    new T this[int index] { get; set; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    new IReadOnlySeries<T> AsReadOnly();

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    Span<T> AsSpan();

    /// <summary>
    /// Returns a mutable slice of this series sharing the underlying storage
    /// and metadata.
    /// </summary>
    /// <param name="start">Zero-based start index of the slice.</param>
    /// <param name="length">Number of elements in the slice.</param>
    new ISeries<T> Slice(int start, int length);

    /// <summary>
    /// Provides labels for data values in the series. There is no guarantee that every data value is
    /// labelled, nor that every label is associated with a data value.
    /// </summary>
    new IValueLabelCollection<T> ValueLabels { get; }
}
