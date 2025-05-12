// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyVector
{
    string? Name { get; }

    int Length { get; }

    bool IsNumeric { get; }

    VectorDataType DataType { get; }

    IReadOnlyDictionary<string, Variant>? Annotations { get; }
}

public interface IReadOnlyVector<T>
    : IReadOnlyVector,
      IReadOnlyList<T>,
      IEquatable<IReadOnlyVector<T>>
{
    IReadOnlyDictionary<string, T> Categories { get; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsReadOnlySpan();

    ReadOnlySpan<T> Slice(int start, int length);

    /// <summary>
    /// Copies the elements of the <see cref="IReadOnlyVector{T}"/> to a new array.
    /// </summary>
    /// <returns></returns>
    T[] ToArray();
}
