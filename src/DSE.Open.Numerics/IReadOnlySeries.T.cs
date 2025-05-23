// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values of data type <typeparamref name="T"/>
///. Optionally named, labelled or categorised for use with
/// a <see cref="IReadOnlyDataFrame"/>.
/// </summary>
public interface IReadOnlySeries<T>
    : IReadOnlySeries,
      IReadOnlyList<T>,
      IEquatable<IReadOnlySeries<T>?>
    where T : IEquatable<T>
{
    new ReadOnlyVector<T> Vector { get; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsReadOnlySpan();

    IReadOnlySeries<T> Slice(int start, int length);

    /// <summary>
    /// Provides labels for data values in the series. There is no guarantee that every data value is
    /// labelled, nor that every label is associated with a data value.
    /// </summary>
    new IReadOnlyValueLabelCollection<T> ValueLabels { get; }
}
