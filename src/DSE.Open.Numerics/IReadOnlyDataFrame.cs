// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Read-only contract over the <see cref="DataFrame"/> / <see cref="ReadOnlyDataFrame"/>
/// hierarchy. Exposes the metadata and column-level access common to both the
/// mutable and read-only frame types so callers can program against either.
/// </summary>
public interface IReadOnlyDataFrame
{
    /// <summary>Gets the optional frame name.</summary>
    string? Name { get; }

    /// <summary>Gets the number of columns in the frame.</summary>
    int Count { get; }

    /// <summary>Gets the column at <paramref name="index"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside <c>[0, Count)</c>.</exception>
    IReadOnlySeries this[int index] { get; }

    /// <summary>Gets the column with the given <paramref name="name"/>, or <see langword="null"/> when no column has that name.</summary>
    IReadOnlySeries? this[string name] { get; }

    /// <summary>Returns an enumerable over the frame's columns as <see cref="IReadOnlySeries"/>.</summary>
    IEnumerable<IReadOnlySeries> GetReadOnlySeriesEnumerable();
}
