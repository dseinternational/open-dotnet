// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

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

        /// <summary>
        /// Gets the cell at the enumerator's current position. Throws
        /// <see cref="InvalidOperationException"/> when accessed before
        /// the first <see cref="MoveNext"/> or after enumeration ends —
        /// clearer than letting the row indexer surface an
        /// <see cref="IndexOutOfRangeException"/> from inside the
        /// underlying column.
        /// </summary>
        public readonly VectorValue Current
        {
            get
            {
                if ((uint)_index >= (uint)_row.Count)
                {
                    ThrowEnumeratorInvalid();
                }

                return _row[_index];
            }
        }

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

        [DoesNotReturn]
        private static void ThrowEnumeratorInvalid()
        {
            throw new InvalidOperationException(
                "Enumeration has not started or has already finished.");
        }
    }
}
