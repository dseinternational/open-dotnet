// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values with value
/// equality semantics.
/// </summary>
public interface IReadOnlyVector
{
    int Length { get; }

    bool IsNumeric { get; }

    VectorDataType DataType { get; }
}

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values with value
/// equality semantics.
/// </summary>
public interface IReadOnlyVector<T>
    : IReadOnlyVector,
      IReadOnlyList<T>,
      IEquatable<IReadOnlyVector<T>>
{
    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsSpan();

    IReadOnlyVector<T> Slice(int start, int length);
}
