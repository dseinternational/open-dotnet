// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of read-only values.
/// </summary>
public interface IReadOnlyVector
{
    VectorDataType DataType { get; }

    bool IsEmpty { get; }

    bool IsNumeric { get; }

    Type ItemType { get; }

    int Length { get; }
}

/// <summary>
/// A serializable, fixed-length, contiguous sequence of read-only values of type <typeparamref name="T"/>.
/// </summary>
public interface IReadOnlyVector<T>
    : IReadOnlyVector,
      IReadOnlyList<T>,
      IEquatable<IReadOnlyVector<T>?>
    where T : IEquatable<T>
{

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsSpan();

    IReadOnlyVector<T> Slice(int start, int length);
}
