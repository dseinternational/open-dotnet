// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlySeries
{
    int Length { get; }

    bool IsNumeric { get; }

    SeriesDataType DataType { get; }
}

public interface IReadOnlySeries<T>
    : IReadOnlySeries,
      IReadOnlyList<T>,
      IEquatable<IReadOnlySeries<T>>
{
    IReadOnlyDictionary<string, T> Categories { get; }

    ReadOnlyMemory<T> Vector { get; }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsReadOnlySpan();

    ReadOnlySpan<T> Slice(int start, int length);

    /// <summary>
    /// Copies the elements of the <see cref="IReadOnlySeries{T}"/> to a new array.
    /// </summary>
    /// <returns></returns>
    T[] ToArray();
}
