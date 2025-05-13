// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(ReadOnlyDataSetJsonConverter))]
[CollectionBuilder(typeof(ReadOnlyDataSet), nameof(Create))]
public class ReadOnlyDataSet : IReadOnlyList<ReadOnlyDataFrame>
{
    private readonly ReadOnlyCollection<ReadOnlyDataFrame> _dataFrames;

    public ReadOnlyDataSet(ReadOnlyCollection<ReadOnlyDataFrame> dataFrames, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(dataFrames);
        _dataFrames = dataFrames;
        Name = name;
    }

    public ReadOnlyDataFrame this[int index] => _dataFrames[index];

    public string? Name { get; }

    public int Count => _dataFrames.Count;

    public IEnumerator<ReadOnlyDataFrame> GetEnumerator()
    {
        return _dataFrames.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dataFrames).GetEnumerator();
    }

    public static ReadOnlyDataSet Create(ReadOnlySpan<ReadOnlyDataFrame> dataFrames)
    {
        return new ReadOnlyDataSet([.. dataFrames.ToArray()]);
    }
}
