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
public class DataSet : IList<DataFrame>
{
    private readonly Collection<DataFrame> _dataFrames;

    /// <summary>Creates an empty data set with the optional <paramref name="name"/>.</summary>
    public DataSet(string? name = null) : this([], name)
    {
    }

    /// <summary>Creates a data set wrapping <paramref name="dataFrames"/> with the optional <paramref name="name"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="dataFrames"/> is <see langword="null"/>.</exception>
    public DataSet(Collection<DataFrame> dataFrames, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(dataFrames);
        _dataFrames = dataFrames;
        Name = name;
    }

    /// <summary>Gets or sets the data frame at <paramref name="index"/>.</summary>
    public DataFrame this[int index] { get => _dataFrames[index]; set => _dataFrames[index] = value; }

    /// <summary>Gets the optional set name.</summary>
    public string? Name { get; }

    /// <summary>Gets the number of data frames in the set.</summary>
    public int Count => _dataFrames.Count;

    /// <inheritdoc />
    public bool IsReadOnly => _dataFrames.IsReadOnly;

    /// <summary>Returns a read-only snapshot of this data set; data frames are individually converted.</summary>
    public ReadOnlyDataSet AsReadOnly()
    {
        return new ReadOnlyDataSet([.. _dataFrames.Select(df => df.AsReadOnly())], Name);
    }

    /// <summary>Adds <paramref name="item"/> to the set.</summary>
    public void Add(DataFrame item)
    {
        _dataFrames.Add(item);
    }

    /// <summary>Removes all data frames from the set.</summary>
    public void Clear()
    {
        _dataFrames.Clear();
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="item"/> is a member of the set (reference equality).</summary>
    public bool Contains(DataFrame item)
    {
        return _dataFrames.Contains(item);
    }

    /// <summary>Copies the data frames into <paramref name="array"/> starting at <paramref name="arrayIndex"/>.</summary>
    public void CopyTo(DataFrame[] array, int arrayIndex)
    {
        _dataFrames.CopyTo(array, arrayIndex);
    }

    /// <summary>Returns an enumerator over the data frames.</summary>
    public IEnumerator<DataFrame> GetEnumerator()
    {
        return ((IEnumerable<DataFrame>)_dataFrames).GetEnumerator();
    }

    /// <summary>Returns the index of <paramref name="item"/>, or <c>-1</c> when not present.</summary>
    public int IndexOf(DataFrame item)
    {
        return _dataFrames.IndexOf(item);
    }

    /// <summary>Inserts <paramref name="item"/> at <paramref name="index"/>.</summary>
    public void Insert(int index, DataFrame item)
    {
        _dataFrames.Insert(index, item);
    }

    /// <summary>Removes <paramref name="item"/>; returns <see langword="true"/> when it was found and removed.</summary>
    public bool Remove(DataFrame item)
    {
        return _dataFrames.Remove(item);
    }

    /// <summary>Removes the data frame at <paramref name="index"/>.</summary>
    public void RemoveAt(int index)
    {
        _dataFrames.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dataFrames).GetEnumerator();
    }
}
