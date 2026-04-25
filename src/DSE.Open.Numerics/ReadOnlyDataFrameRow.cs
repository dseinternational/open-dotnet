// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Numerics;

public readonly record struct ReadOnlyDataFrameRow : IReadOnlyList<VectorValue>
{
    private readonly ReadOnlyDataFrame _df;
    private readonly int _rowIndex;

    internal ReadOnlyDataFrameRow(ReadOnlyDataFrame df, int rowIndex)
    {
        _df = df;
        _rowIndex = rowIndex;
    }

    public VectorValue this[int index] => _df[index].GetVectorValue(_rowIndex);

    public int Count => _df.Count;

    /// <summary>
    /// Returns a struct enumerator over the cells of this row. Iterating with
    /// <c>foreach</c> binds to this overload (resolved by duck-typing) and avoids
    /// the per-iteration heap allocation that the previous <c>yield return</c>-based
    /// implementation incurred from the compiler-generated state machine.
    /// </summary>
    public Enumerator GetEnumerator()
    {
        return new(this);
    }

    IEnumerator<VectorValue> IEnumerable<VectorValue>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct Enumerator : IEnumerator<VectorValue>
    {
        private readonly ReadOnlyDataFrameRow _row;
        private int _index;

        internal Enumerator(ReadOnlyDataFrameRow row)
        {
            _row = row;
            _index = -1;
        }

        public readonly VectorValue Current => _row[_index];

        readonly object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var next = _index + 1;
            if (next < _row.Count)
            {
                _index = next;
                return true;
            }

            _index = _row.Count;
            return false;
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        public readonly void Dispose()
        {
        }
    }
}
