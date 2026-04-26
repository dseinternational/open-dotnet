// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A read-only set of <see cref="ReadOnlyDataFrame"/> instances.
/// </summary>
[JsonConverter(typeof(ReadOnlyDataSetJsonConverter))]
[CollectionBuilder(typeof(ReadOnlyDataSet), nameof(Create))]
public class ReadOnlyDataSet : IReadOnlyList<ReadOnlyDataFrame>
{
    /// <summary>The shared empty data set.</summary>
    public static readonly ReadOnlyDataSet Empty = new();

    private readonly ReadOnlyCollection<ReadOnlyDataFrame> _dataFrames;

    private ReadOnlyDataSet() : this([])
    {
    }

    /// <summary>Creates a read-only data set wrapping <paramref name="dataFrames"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="dataFrames"/> is <see langword="null"/>.</exception>
    public ReadOnlyDataSet(ReadOnlyCollection<ReadOnlyDataFrame> dataFrames, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(dataFrames);
        _dataFrames = dataFrames;
        Name = name;
    }

    /// <summary>Gets the data frame at <paramref name="index"/>.</summary>
    public ReadOnlyDataFrame this[int index] => _dataFrames[index];

    /// <summary>Gets the optional set name.</summary>
    public string? Name { get; }

    /// <summary>Gets the number of data frames in the set.</summary>
    public int Count => _dataFrames.Count;

    /// <summary>Returns an enumerator over the data frames.</summary>
    public IEnumerator<ReadOnlyDataFrame> GetEnumerator()
    {
        return _dataFrames.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dataFrames).GetEnumerator();
    }

    /// <summary>
    /// Collection-initializer-friendly factory; copies <paramref name="dataFrames"/>
    /// into a fresh internal collection.
    /// </summary>
    public static ReadOnlyDataSet Create(ReadOnlySpan<ReadOnlyDataFrame> dataFrames)
    {
        return new ReadOnlyDataSet([.. dataFrames.ToArray()]);
    }
}
