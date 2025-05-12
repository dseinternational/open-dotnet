// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(DataFrameJsonConverter))]
public class DataFrame : IDataFrame
{
    private readonly Collection<Vector> _columnVectors;
    private readonly Collection<string> _columnNames;

    public DataFrame() : this([])
    {
    }

    public DataFrame(Collection<Vector> columns)
    {
        ArgumentNullException.ThrowIfNull(columns);

        _columnVectors = columns;
        _columnNames = new Collection<string>(columns.Count);
    }

    /// <summary>
    /// A name for the data frame (optional).
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the number of columns in the data frame.
    /// </summary>
    public int Count => _columnVectors.Count;

    public bool IsReadOnly => false;

    public Vector? this[string name]
    {
        get
        {
            var index = _columnNames.IndexOf(name);

            if (index < 0)
            {
                return null;
            }

            return _columnVectors[index];
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            var index = _columnNames.IndexOf(name);

            if (index < 0)
            {
                _columnNames.Add(name);
                _columnVectors.Add(value);
            }
            else
            {
                _columnVectors[index] = value;
            }
        }
    }

    public Vector this[int index]
    {
        get => _columnVectors[index];
        set => _columnVectors[index] = value;
    }

    public int IndexOf(Vector item)
    {
        return _columnVectors.IndexOf(item);
    }

    public void Insert(int index, Vector item)
    {
        _columnVectors.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _columnVectors.RemoveAt(index);
    }

    public void Add(Vector item)
    {
        _columnVectors.Add(item);
    }

    public void Clear()
    {
        _columnVectors.Clear();
    }

    public bool Contains(Vector item)
    {
        return _columnVectors.Contains(item);
    }

    public void CopyTo(Vector[] array, int arrayIndex)
    {
        _columnVectors.CopyTo(array, arrayIndex);
    }

    public bool Remove(Vector item)
    {
        return _columnVectors.Remove(item);
    }

    public IEnumerator<Vector> GetEnumerator()
    {
        return _columnVectors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_columnVectors).GetEnumerator();
    }
}
