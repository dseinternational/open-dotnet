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

    public int Count => _df.Count == 0 ? 0 : _df[0].Length;

    /// <summary>
    /// Returns a struct enumerator over the rows of the data frame. Iterating with
    /// <c>foreach</c> binds to this overload (resolved by duck-typing) and avoids
    /// the per-iteration heap allocation that the previous <c>yield return</c>-based
    /// implementation incurred from the compiler-generated state machine.
    /// </summary>
    public Enumerator GetEnumerator()
    {
        return new(this);
    }

    IEnumerator<ReadOnlyDataFrameRow> IEnumerable<ReadOnlyDataFrameRow>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct Enumerator : IEnumerator<ReadOnlyDataFrameRow>
    {
        private readonly ReadOnlyDataFrameRowCollection _rows;
        private int _index;

        internal Enumerator(ReadOnlyDataFrameRowCollection rows)
        {
            _rows = rows;
            _index = -1;
        }

        public readonly ReadOnlyDataFrameRow Current => _rows[_index];

        readonly object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var next = _index + 1;
            if (next < _rows.Count)
            {
                _index = next;
                return true;
            }

            _index = _rows.Count;
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
