// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

public readonly record struct DataFrameRow : IReadOnlyList<VectorValue>
{
    private readonly DataFrame _df;
    private readonly int _rowIndex;

    internal DataFrameRow(DataFrame df, int rowIndex)
    {
        _df = df;
        _rowIndex = rowIndex;
    }

    public VectorValue this[int index] => _df[index].GetVectorValue(_rowIndex);

    public int Count => _df.Count;

    public IEnumerator<VectorValue> GetEnumerator()
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
