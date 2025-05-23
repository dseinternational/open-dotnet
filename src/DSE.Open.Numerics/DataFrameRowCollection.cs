// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

public readonly record struct DataFrameRowCollection : IReadOnlyList<DataFrameRow>
{
    private readonly DataFrame _df;

    internal DataFrameRowCollection(DataFrame df)
    {
        _df = df;
    }

    public DataFrameRow this[int index] => new(_df, index);

    public int Count => _df.Count;

    public IEnumerator<DataFrameRow> GetEnumerator()
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
