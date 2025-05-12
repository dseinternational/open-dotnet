// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics.Data;

public class DataFrame : IDataFrame
{
    private readonly Collection<Vector> _columns;

    public DataFrame() : this([])
    {
    }

    public DataFrame(Collection<Vector> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);
        _columns = columns;
    }

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the number of columns in the data frame.
    /// </summary>
    public int Count => _columns.Count;

    public bool IsReadOnly => false;

    public Vector this[int index]
    {
        get => _columns[index];
        set => _columns[index] = value;
    }

    public Vector? this[string name] => _columns.FirstOrDefault(s => s.Name == name);

    public bool TryGetColumn(string name, out Vector? column)
    {
        column = _columns.FirstOrDefault(s => s.Name == name);
        return column != null;
    }

    public bool TryGetColumn<T>(string name, out Vector<T>? column)
    {
        column = _columns.OfType<Vector<T>>().FirstOrDefault(s => s.Name == name);
        return column != null;
    }

    public int IndexOf(Vector item)
    {
        return _columns.IndexOf(item);
    }

    public void Insert(int index, Vector item)
    {
        _columns.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _columns.RemoveAt(index);
    }

    public void Add(Vector item)
    {
        _columns.Add(item);
    }

    public void Clear()
    {
        _columns.Clear();
    }

    public bool Contains(Vector item)
    {
        return _columns.Contains(item);
    }

    public void CopyTo(Vector[] array, int arrayIndex)
    {
        _columns.CopyTo(array, arrayIndex);
    }

    public bool Remove(Vector item)
    {
        return _columns.Remove(item);
    }

    public IEnumerator<Vector> GetEnumerator()
    {
        return _columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columns).GetEnumerator();
    }
}
