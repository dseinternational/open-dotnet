// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A set of data frames (<see cref="DataFrame"/>).
/// </summary>
[JsonConverter(typeof(DataSetJsonConverter))]
public class DataSet : IDataSet
{
    private readonly Collection<DataFrame> _dataFrames;

    public DataSet(IEnumerable<DataFrame> dataFrames)
    {
        ArgumentNullException.ThrowIfNull(dataFrames);
        _dataFrames = [.. dataFrames];
    }

    public DataFrame this[int index] { get => _dataFrames[index]; set => _dataFrames[index] = value; }

    public string? Name { get; }

    public int Count => _dataFrames.Count;

    public bool IsReadOnly => _dataFrames.IsReadOnly;

    public void Add(DataFrame item)
    {
        _dataFrames.Add(item);
    }

    public void Clear()
    {
        _dataFrames.Clear();
    }

    public bool Contains(DataFrame item)
    {
        return _dataFrames.Contains(item);
    }

    public void CopyTo(DataFrame[] array, int arrayIndex)
    {
        _dataFrames.CopyTo(array, arrayIndex);
    }

    public IEnumerator<DataFrame> GetEnumerator()
    {
        return ((IEnumerable<DataFrame>)_dataFrames).GetEnumerator();
    }

    public int IndexOf(DataFrame item)
    {
        return _dataFrames.IndexOf(item);
    }

    public void Insert(int index, DataFrame item)
    {
        _dataFrames.Insert(index, item);
    }

    public bool Remove(DataFrame item)
    {
        return _dataFrames.Remove(item);
    }

    public void RemoveAt(int index)
    {
        _dataFrames.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dataFrames).GetEnumerator();
    }
}
