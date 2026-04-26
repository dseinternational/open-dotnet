// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Stores related data as a collection of columns (<see cref="Series"/>).
/// </summary>
[JsonConverter(typeof(ReadOnlyDataFrameJsonConverter))]
[CollectionBuilder(typeof(ReadOnlyDataFrame), nameof(Create))]
public sealed class ReadOnlyDataFrame : IReadOnlyList<ReadOnlySeries>, IReadOnlyDataFrame
{
    /// <summary>The shared empty data frame.</summary>
    public static readonly ReadOnlyDataFrame Empty = new();

    private readonly ReadOnlyCollection<ReadOnlySeries> _columns;

    /// <summary>
    /// Eagerly-built name → column-index map. Built once at construction since the
    /// frame is immutable. Replaces an O(N) linear scan over <see cref="_columns"/>
    /// for every <see cref="this[string]"/> lookup.
    /// </summary>
    /// <remarks>
    /// <see cref="ReadOnlySeries.Name"/> is itself read-only on this type, so unlike
    /// the lazy index on <see cref="DataFrame"/> there is no rename hazard and no
    /// trust-but-verify is needed. <see langword="null"/> when the frame is empty,
    /// to avoid the per-frame allocation in that common case.
    /// </remarks>
    private readonly FrozenDictionary<string, int>? _nameIndex;

    private ReadOnlyDataFrame() : this([], null)
    {
    }

    /// <summary>Creates an unnamed read-only data frame from the supplied <paramref name="columns"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="columns"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException">The columns are not all of equal length.</exception>
    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlySeries> columns) : this(columns, null)
    {
    }

    /// <summary>Creates a read-only data frame from the supplied <paramref name="columns"/> with the given <paramref name="name"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="columns"/> is <see langword="null"/>.</exception>
    /// <exception cref="NumericsArgumentException">The columns are not all of equal length.</exception>
    public ReadOnlyDataFrame(ReadOnlyCollection<ReadOnlySeries> columns, string? name)
    {
        ArgumentNullException.ThrowIfNull(columns);
        NumericsArgumentException.ThrowIfNotEqualLength(columns);

        Name = name;

        _columns = columns;

        _nameIndex = BuildNameIndex(columns);
    }

    private static FrozenDictionary<string, int>? BuildNameIndex(ReadOnlyCollection<ReadOnlySeries> columns)
    {
        if (columns.Count == 0)
        {
            return null;
        }

        var builder = new Dictionary<string, int>(columns.Count, StringComparer.Ordinal);

        for (var i = 0; i < columns.Count; i++)
        {
            var columnName = columns[i].Name;
            if (columnName is null)
            {
                continue;
            }

            // Duplicate names: keep the first occurrence to preserve the
            // FirstOrDefault(s => s.Name == name) semantics of the previous
            // linear-scan implementation.
            _ = builder.TryAdd(columnName, i);
        }

        return builder.ToFrozenDictionary(StringComparer.Ordinal);
    }

    /// <summary>Gets the column at <paramref name="index"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside <c>[0, Count)</c>.</exception>
    public ReadOnlySeries this[int index] => _columns[index];

    /// <summary>
    /// Gets the column with the given <paramref name="name"/>, or
    /// <see langword="null"/> when no column has that name. Backed by an eager
    /// <see cref="FrozenDictionary{TKey, TValue}"/>; lookup is O(1).
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    public ReadOnlySeries? this[string name]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(name);
            return _nameIndex is not null && _nameIndex.TryGetValue(name, out var index)
                ? _columns[index]
                : null;
        }
    }

    IReadOnlySeries? IReadOnlyDataFrame.this[string name] => this[name];

    IReadOnlySeries IReadOnlyDataFrame.this[int index] => this[index];

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    /// <summary>Gets the number of columns.</summary>
    public int Count => _columns.Count;

    /// <summary>Gets the total number of cells in the frame (rows × columns).</summary>
    public int FlattenedLength => _columns.Count > 0
        ? _columns.Count * _columns[0].Length
        : 0;

    /// <summary>Gets a row-wise view of the frame.</summary>
    public ReadOnlyDataFrameRowCollection Rows => new(this);

    /// <summary>Returns an enumerator over the columns.</summary>
    public IEnumerator<ReadOnlySeries> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }

    /// <summary>Collection-initializer-friendly factory; copies <paramref name="columns"/> into a fresh internal collection.</summary>
    public static ReadOnlyDataFrame Create(ReadOnlySpan<ReadOnlySeries> columns)
    {
        return new ReadOnlyDataFrame([.. columns.ToArray()]);
    }

    IEnumerable<IReadOnlySeries> IReadOnlyDataFrame.GetReadOnlySeriesEnumerable()
    {
        return this;
    }
}
