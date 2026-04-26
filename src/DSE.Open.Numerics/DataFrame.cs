// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A set of related data, organised as a collection of columns (<see cref="Series"/>) of equal length,
/// optimised for serialization.
/// </summary>
[JsonConverter(typeof(DataFrameJsonConverter))]
[CollectionBuilder(typeof(DataFrame), nameof(Create))]
public sealed class DataFrame : IList<Series>, IReadOnlyDataFrame
{
    private readonly Collection<Series> _columns;

    /// <summary>
    /// Lazily-built name → column-index map. Populated on first lookup against
    /// <see cref="this[string]"/> and invalidated by any structural mutation.
    /// </summary>
    /// <remarks>
    /// <see cref="Series.Name"/> is publicly settable, so an external rename of a
    /// column already inside the frame can desync this index without the frame
    /// observing it. The lookup path is therefore "trust but verify" — a hit is
    /// double-checked against the column's current <see cref="Series.Name"/>, and
    /// a stale hit triggers a rebuild via <see cref="InvalidateNameIndex"/>.
    /// </remarks>
    private Dictionary<string, int>? _nameIndex;

    /// <summary>Creates an empty, unnamed data frame.</summary>
    public DataFrame() : this([], null)
    {
    }

    /// <summary>Creates an empty data frame with the given <paramref name="name"/>.</summary>
    public DataFrame(string name) : this([], name)
    {
    }

    /// <summary>Creates an unnamed data frame from the supplied <paramref name="columns"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="columns"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException">The columns are not all of equal length.</exception>
    public DataFrame(Collection<Series> columns) : this(columns, null)
    {
    }

    /// <summary>
    /// Creates a data frame from the supplied <paramref name="columns"/> with the given
    /// <paramref name="name"/>. Each column without a <see cref="Series.Name"/> receives
    /// a positional default ("0", "1", …).
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="columns"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException">The columns are not all of equal length.</exception>
    public DataFrame(Collection<Series> columns, string? name)
    {
        ArgumentNullException.ThrowIfNull(columns);
        NumericsArgumentException.ThrowIfNotEqualLength(columns);

        Name = name;

        _columns = columns;

        for (var i = 0; i < _columns.Count; i++)
        {
            _columns[i].Name ??= i.ToStringInvariant();
        }
    }

    /// <summary>
    /// Gets or sets the column with the given <paramref name="name"/>. Get returns
    /// <see langword="null"/> when no column has that name; set replaces an existing
    /// column with the given name (preserving the column order) or appends a new
    /// column when no match exists. Lookup is O(1) via a lazy name index; see
    /// <see cref="this[int]"/> for index-based access.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> or the assigned value is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException">The assigned value's length does not match the frame's other columns.</exception>
    public Series? this[string name]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(name);
            return TryFindIndexByName(name, out var index) ? _columns[index] : null;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (TryFindIndexByName(name, out var index))
            {
                EnsureLengthMatchesOtherColumns(value, replacingIndex: index);
                _columns[index] = value;
                // Default the replacement's name to the lookup name so it remains
                // findable as `frame[name]`. The previous code inferred a positional
                // name ("0", "1", …) here, which silently desynced indexer-set from
                // indexer-get.
                value.Name ??= name;
                InvalidateNameIndex();
            }
            else
            {
                Add(value);
                value.Name = name;
                InvalidateNameIndex();
            }
        }
    }

    /// <summary>
    /// Gets or sets the column at <paramref name="index"/>. Setting requires the new
    /// column's length to match the frame's other columns.
    /// </summary>
    /// <exception cref="ArgumentNullException">The assigned value is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside <c>[0, Count)</c>.</exception>
    /// <exception cref="NumericsArgumentException">The assigned value's length does not match the frame's other columns.</exception>
    public Series this[int index]
    {
        get => _columns[index];
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            EnsureLengthMatchesOtherColumns(value, replacingIndex: index);
            _columns[index] = value;
            InvalidateNameIndex();
        }
    }

    IReadOnlySeries? IReadOnlyDataFrame.this[string name] => this[name];

    IReadOnlySeries IReadOnlyDataFrame.this[int index] => this[index];

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the number of columns in the data frame.
    /// </summary>
    public int Count => _columns.Count;

    /// <summary>
    /// Returns a row-wise view of the frame. <c>frame.Rows[i]</c> exposes the
    /// <c>i</c>-th row across all columns; iteration uses zero-allocation struct
    /// enumerators.
    /// </summary>
    public DataFrameRowCollection Rows => new(this);

    bool ICollection<Series>.IsReadOnly => _columns.IsReadOnly;

    /// <summary>Returns a read-only snapshot of this frame; columns are individually converted to read-only.</summary>
    public ReadOnlyDataFrame AsReadOnly()
    {
        return new ReadOnlyDataFrame([.. _columns.Select(v => v.AsReadOnly())], Name);
    }

    /// <summary>Returns the index of <paramref name="item"/> within the column collection, or <c>-1</c> when not present.</summary>
    public int IndexOf(Series item)
    {
        return _columns.IndexOf(item);
    }

    /// <summary>
    /// Inserts <paramref name="item"/> at the given <paramref name="index"/>. The
    /// new column's length must match the existing columns' length.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="item"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException"><paramref name="item"/>'s length does not match the frame.</exception>
    public void Insert(int index, Series item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureCompatibleColumnLength(item, requireMatch: _columns.Count > 0);
        _columns.Insert(index, item);
        item.Name ??= index.ToStringInvariant();
        InvalidateNameIndex();
    }

    /// <summary>Removes the column at <paramref name="index"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside <c>[0, Count)</c>.</exception>
    public void RemoveAt(int index)
    {
        _columns.RemoveAt(index);
        InvalidateNameIndex();
    }

    /// <summary>
    /// Appends <paramref name="item"/> to the frame. The new column's length must
    /// match the existing columns' length; if the column has no <see cref="Series.Name"/>,
    /// it receives a positional default ("0", "1", …).
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="item"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException"><paramref name="item"/>'s length does not match the frame.</exception>
    public void Add(Series item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureCompatibleColumnLength(item, requireMatch: _columns.Count > 0);
        _columns.Add(item);
        item.Name ??= (_columns.Count - 1).ToStringInvariant();
        InvalidateNameIndex();
    }

    /// <summary>
    /// Verifies that <paramref name="candidate"/>'s length matches the length of the existing columns.
    /// </summary>
    /// <remarks>
    /// When the frame is empty, the candidate becomes the length-defining column and any length is
    /// accepted. When the frame already has columns, the candidate must match their length; otherwise
    /// row-level access (e.g. <see cref="Rows"/>) would be inconsistent.
    /// </remarks>
    private void EnsureCompatibleColumnLength(Series candidate, bool requireMatch)
    {
        if (!requireMatch || _columns.Count == 0)
        {
            return;
        }

        var expected = _columns[0].Length;

        if (candidate.Length != expected)
        {
            NumericsArgumentException.Throw(
                $"Column length {candidate.Length} does not match the data frame's column length of {expected}.");
        }
    }

    /// <summary>
    /// Verifies that <paramref name="candidate"/>'s length matches the length of any column other
    /// than the one at <paramref name="replacingIndex"/>. Used by indexer-set replacement, where
    /// the column being replaced does not constrain the new value's length.
    /// </summary>
    private void EnsureLengthMatchesOtherColumns(Series candidate, int replacingIndex)
    {
        if (_columns.Count <= 1)
        {
            // No other columns to constrain length against.
            return;
        }

        var referenceIndex = replacingIndex == 0 ? 1 : 0;
        var expected = _columns[referenceIndex].Length;

        if (candidate.Length != expected)
        {
            NumericsArgumentException.Throw(
                $"Column length {candidate.Length} does not match the data frame's column length of {expected}.");
        }
    }

    /// <summary>Removes all columns.</summary>
    public void Clear()
    {
        _columns.Clear();
        InvalidateNameIndex();
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="item"/> is one of the frame's columns (reference equality).</summary>
    public bool Contains(Series item)
    {
        return _columns.Contains(item);
    }

    /// <summary>Copies the frame's columns into <paramref name="array"/> starting at <paramref name="arrayIndex"/>.</summary>
    public void CopyTo(Series[] array, int arrayIndex)
    {
        _columns.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes <paramref name="item"/> from the frame. Returns <see langword="true"/>
    /// when the column was found and removed.
    /// </summary>
    public bool Remove(Series item)
    {
        var removed = _columns.Remove(item);
        if (removed)
        {
            InvalidateNameIndex();
        }

        return removed;
    }

    /// <summary>Returns an enumerator over the frame's columns.</summary>
    public IEnumerator<Series> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }

    IEnumerable<IReadOnlySeries> IReadOnlyDataFrame.GetReadOnlySeriesEnumerable()
    {
        return this;
    }

    /// <summary>
    /// Returns the index of the column whose <see cref="Series.Name"/> matches
    /// <paramref name="name"/> (ordinal comparison; first occurrence wins on duplicates),
    /// or <c>false</c> if no such column exists.
    /// </summary>
    /// <remarks>
    /// Backed by a lazily-built dictionary; replaces the previous O(N) linear scan
    /// over <see cref="_columns"/>. A dictionary hit is verified against the column's
    /// current <see cref="Series.Name"/> to guard against external rename via the
    /// publicly settable Series.Name setter; a stale hit triggers a rebuild and a
    /// linear-scan fallback so the lookup remains correct.
    /// </remarks>
    private bool TryFindIndexByName(string name, out int index)
    {
        var nameIndex = NameIndex;

        if (nameIndex.TryGetValue(name, out index))
        {
            // Trust-but-verify: the cached index is valid only while the column
            // at that position still has the expected name.
            if (_columns[index].Name == name)
            {
                return true;
            }

            // External rename has invalidated the cached entry. Drop the cache,
            // fall through to a linear scan, and let the next lookup repopulate.
            InvalidateNameIndex();
        }

        for (var i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].Name == name)
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }

    private Dictionary<string, int> NameIndex
    {
        get
        {
            if (_nameIndex is not null)
            {
                return _nameIndex;
            }

            var index = new Dictionary<string, int>(_columns.Count, StringComparer.Ordinal);

            for (var i = 0; i < _columns.Count; i++)
            {
                var columnName = _columns[i].Name;
                if (columnName is null)
                {
                    continue;
                }

                // Duplicate names: keep the first occurrence to preserve the
                // FirstOrDefault(s => s.Name == name) semantics of the previous
                // linear-scan implementation.
                _ = index.TryAdd(columnName, i);
            }

            _nameIndex = index;
            return index;
        }
    }

    private void InvalidateNameIndex()
    {
        _nameIndex = null;
    }

    /// <summary>Creates an unnamed data frame wrapping <paramref name="columns"/>.</summary>
    [OverloadResolutionPriority(1)]
    public static DataFrame Create(Collection<Series> columns)
    {
        return new DataFrame(columns);
    }

    /// <summary>Creates a data frame wrapping <paramref name="columns"/> with the given <paramref name="name"/>.</summary>
    public static DataFrame Create(Collection<Series> columns, string? name)
    {
        return new DataFrame(columns, name);
    }

    /// <summary>
    /// Collection-initializer-friendly factory; copies <paramref name="columns"/>
    /// into a fresh internal collection.
    /// </summary>
    public static DataFrame Create(ReadOnlySpan<Series> columns)
    {
        return new DataFrame([.. columns.ToArray()]);
    }
}
