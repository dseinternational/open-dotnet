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

    public DataFrame() : this([], null)
    {
    }

    public DataFrame(string name) : this([], name)
    {
    }

    public DataFrame(Collection<Series> columns) : this(columns, null)
    {
    }

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

    public DataFrameRowCollection Rows => new(this);

    bool ICollection<Series>.IsReadOnly => _columns.IsReadOnly;

    public ReadOnlyDataFrame AsReadOnly()
    {
        return new ReadOnlyDataFrame([.. _columns.Select(v => v.AsReadOnly())], Name);
    }

    public int IndexOf(Series item)
    {
        return _columns.IndexOf(item);
    }

    public void Insert(int index, Series item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureCompatibleColumnLength(item, requireMatch: _columns.Count > 0);
        _columns.Insert(index, item);
        item.Name ??= index.ToStringInvariant();
        InvalidateNameIndex();
    }

    public void RemoveAt(int index)
    {
        _columns.RemoveAt(index);
        InvalidateNameIndex();
    }

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

    public void Clear()
    {
        _columns.Clear();
        InvalidateNameIndex();
    }

    public bool Contains(Series item)
    {
        return _columns.Contains(item);
    }

    public void CopyTo(Series[] array, int arrayIndex)
    {
        _columns.CopyTo(array, arrayIndex);
    }

    public bool Remove(Series item)
    {
        var removed = _columns.Remove(item);
        if (removed)
        {
            InvalidateNameIndex();
        }

        return removed;
    }

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

    [OverloadResolutionPriority(1)]
    public static DataFrame Create(Collection<Series> columns)
    {
        return new DataFrame(columns);
    }

    public static DataFrame Create(Collection<Series> columns, string? name)
    {
        return new DataFrame(columns, name);
    }

    public static DataFrame Create(ReadOnlySpan<Series> columns)
    {
        return new DataFrame([.. columns.ToArray()]);
    }
}
