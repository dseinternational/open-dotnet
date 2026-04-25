// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values.
/// Optionally named, labelled or categorised for use with
/// a <see cref="IReadOnlyDataFrame"/>.
/// </summary>
public interface IReadOnlySeries
{
    /// <summary>
    /// Gets the optional name of the series. Frames assign positional names
    /// (<c>"0"</c>, <c>"1"</c>, …) to columns that are otherwise unnamed.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the number of elements in the series.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Gets <see langword="true"/> when this series has a non-empty
    /// <see cref="CategorySet{T}"/> attached.
    /// </summary>
    bool IsCategorical { get; }

    /// <summary>
    /// Gets <see langword="true"/> when the element type is one of the
    /// numeric <see cref="VectorDataType"/> variants.
    /// </summary>
    bool IsNumeric { get; }

    /// <summary>
    /// Gets the <see cref="VectorDataType"/> tag for the series' element type.
    /// </summary>
    VectorDataType DataType { get; }

    /// <summary>
    /// Gets the underlying read-only vector storage.
    /// </summary>
    IReadOnlyVector Vector { get; }

    /// <summary>
    /// Returns the element at <paramref name="index"/> boxed into a
    /// type-erased <see cref="VectorValue"/>.
    /// </summary>
    /// <param name="index">Zero-based element index.</param>
    VectorValue GetVectorValue(int index);

    /// <summary>
    /// Gets the optional set of allowed values for this series. When
    /// <see cref="IsCategorical"/> is <see langword="true"/>, every element of
    /// the series is guaranteed (at construction and assignment) to be a member
    /// of this set.
    /// </summary>
    IReadOnlyCategorySet Categories { get; }

    /// <summary>
    /// Provides labels for data values in the series. There is no guarantee that every data value is
    /// labelled, nor that every label is associated with a data value.
    /// </summary>
    IReadOnlyValueLabelCollection ValueLabels { get; }

    /// <summary>
    /// Gets <see langword="true"/> when at least one value-label is attached
    /// to the series.
    /// </summary>
    bool HasValueLabels { get; }
}
