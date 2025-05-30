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
    string? Name { get; }

    int Length { get; }

    bool IsCategorical { get; }

    bool IsNumeric { get; }

    VectorDataType DataType { get; }

    IReadOnlyVector Vector { get; }

    VectorValue GetVectorValue(int index);

    IReadOnlyCategorySet Categories { get; }

    /// <summary>
    /// Provides labels for data values in the series. There is no guarantee that every data value is
    /// labelled, nor that every label is associated with a data value.
    /// </summary>
    IReadOnlyValueLabelCollection ValueLabels { get; }

    bool HasValueLabels { get; }
}
