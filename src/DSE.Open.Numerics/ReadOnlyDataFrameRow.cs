// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

public sealed class ReadOnlyDataFrameRow : IReadOnlyList<VectorValue>
{
    private readonly DataFrame _df;
    private readonly int _rowIndex;

    internal ReadOnlyDataFrameRow(DataFrame df, int rowIndex)
    {
        _df = df;
        _rowIndex = rowIndex;
    }

    // ** TODO

#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations

    public VectorValue this[int index] => throw new NotImplementedException();

    public int Count => throw new NotImplementedException();

#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations

    public IEnumerator<VectorValue> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
