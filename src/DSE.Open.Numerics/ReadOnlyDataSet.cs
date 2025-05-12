// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(ReadOnlyDataSetJsonConverter))]
public class ReadOnlyDataSet : IReadOnlyDataSet
{
    private readonly Collection<DataFrame> _dataFrames;

    public ReadOnlyDataSet(IEnumerable<DataFrame> dataFrames)
    {
        ArgumentNullException.ThrowIfNull(dataFrames);
        _dataFrames = [.. dataFrames];
    }

    public DataFrame this[int index] => ((IReadOnlyList<DataFrame>)_dataFrames)[index];

    public string? Name { get; }

    public int Count => ((IReadOnlyCollection<DataFrame>)_dataFrames).Count;

    public IEnumerator<DataFrame> GetEnumerator()
    {
        return ((IEnumerable<DataFrame>)_dataFrames).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dataFrames).GetEnumerator();
    }
}
