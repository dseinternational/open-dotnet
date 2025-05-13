// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values with value
/// equality semantics. Optionally named, labelled or categorised for use with
/// a <see cref="IReadOnlyDataFrame"/>.
/// </summary>
public interface IReadOnlyVector
{
    string? Name { get; }

    int Length { get; }

    bool IsNumeric { get; }

    VectorDataType DataType { get; }
}

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values of data type <typeparamref name="T"/>
///. Optionally named, labelled or categorised for use with
/// a <see cref="IReadOnlyDataFrame"/>.
/// </summary>
public interface IReadOnlyVector<T>
    : IReadOnlyVector,
      IReadOnlyList<T>,
      IEquatable<IReadOnlyVector<T>>
{
    IReadOnlyDictionary<string, T> Categories { get; }

    bool HasCategories { get; }

    /// <summary>
    /// Gets a read-only view of the vector.
    /// </summary>
    ReadOnlyMemory<T> Data { get; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsReadOnlySpan();

    ReadOnlyMemory<T> Slice(int start, int length);
}
