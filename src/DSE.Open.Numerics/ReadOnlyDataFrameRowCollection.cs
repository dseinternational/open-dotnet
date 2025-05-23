// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

public readonly record struct ReadOnlyDataFrameRowCollection : IReadOnlyList<ReadOnlyDataFrameRow>
{
    private readonly ReadOnlyDataFrame _df;

    internal ReadOnlyDataFrameRowCollection(ReadOnlyDataFrame df)
    {
        _df = df;
    }

    public ReadOnlyDataFrameRow this[int index] => new(_df, index);

    public int Count => _df.Count;

    public IEnumerator<ReadOnlyDataFrameRow> GetEnumerator()
    {
        for (var i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
