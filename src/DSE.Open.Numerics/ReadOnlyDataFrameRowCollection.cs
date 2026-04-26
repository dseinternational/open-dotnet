// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

/// <summary>
/// A row-wise view over a <see cref="ReadOnlyDataFrame"/>. Constructed via
/// <see cref="ReadOnlyDataFrame.Rows"/>; iterating yields
/// <see cref="ReadOnlyDataFrameRow"/> instances zero-allocation via the public
/// struct enumerator.
/// </summary>
public readonly record struct ReadOnlyDataFrameRowCollection : IReadOnlyList<ReadOnlyDataFrameRow>
{
    private readonly ReadOnlyDataFrame _df;

    internal ReadOnlyDataFrameRowCollection(ReadOnlyDataFrame df)
    {
        _df = df;
    }

    /// <summary>Gets the row at <paramref name="index"/>.</summary>
    public ReadOnlyDataFrameRow this[int index] => new(_df, index);

    /// <summary>Gets the number of rows in the frame.</summary>
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

    /// <summary>Zero-allocation struct enumerator over the rows of a read-only frame.</summary>
    public struct Enumerator : IEnumerator<ReadOnlyDataFrameRow>
    {
        private readonly ReadOnlyDataFrameRowCollection _rows;
        private int _index;

        internal Enumerator(ReadOnlyDataFrameRowCollection rows)
        {
            _rows = rows;
            _index = -1;
        }

        /// <summary>
        /// Gets the row at the enumerator's current position. Throws
        /// <see cref="InvalidOperationException"/> when accessed before
        /// the first <see cref="MoveNext"/> or after enumeration ends —
        /// clearer than producing a <see cref="ReadOnlyDataFrameRow"/>
        /// backed by an out-of-range row index that fails later when its
        /// cells are read.
        /// </summary>
        public readonly ReadOnlyDataFrameRow Current
        {
            get
            {
                if ((uint)_index >= (uint)_rows.Count)
                {
                    ThrowEnumeratorInvalid();
                }

                return _rows[_index];
            }
        }

        readonly object? IEnumerator.Current => Current;

        /// <summary>Advances the enumerator to the next row.</summary>
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

        /// <summary>No-op.</summary>
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
