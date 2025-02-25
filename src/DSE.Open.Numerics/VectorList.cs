// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Numerics;
using System.Numerics.Tensors;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

public class VectorList<T> : IList<Vector<T>>, IReadOnlyList<Vector<T>>
    where T : struct, INumber<T>
{
    private readonly Collection<Vector<T>> _collection;

    public VectorList()
    {
        _collection = [];
    }

    public VectorList(IEnumerable<T[]> vectors)
    {
        _collection = [.. vectors];
    }

    public VectorList(IEnumerable<Vector<T>> vectors)
    {
        _collection = [.. vectors];
    }

    public VectorList(IEnumerable<Memory<T>> vectors)
    {
        _collection = [.. vectors];
    }

    public VectorList(T[][] vectors)
    {
        _collection = [.. vectors];
    }

    public Vector<T> this[int index]
    {
        get => _collection[index];
        set => _collection[index] = value;
    }

    public int Count => _collection.Count;

    protected virtual bool IsReadOnly => false;

    bool ICollection<Vector<T>>.IsReadOnly => IsReadOnly;

    public virtual void Add(Vector<T> vector)
    {
        _collection.Add(vector);
    }

    public bool AllLengthsAreEqual()
    {
        if (_collection.Count < 1)
        {
            return true;
        }

        var length = _collection[0].Length;

        for (var i = 1; i < _collection.Count; i++)
        {
            if (_collection[i].Length != length)
            {
                return false;
            }
        }

        return true;
    }

    public void Clear()
    {
        _collection.Clear();
    }

    public bool Contains(Vector<T> vector)
    {
        return _collection.Contains(vector);
    }

    protected virtual void CopyTo(Vector<T>[] array, int arrayIndex)
    {
        _collection.CopyTo(array, arrayIndex);
    }

    void ICollection<Vector<T>>.CopyTo(Vector<T>[] array, int arrayIndex)
    {
        CopyTo(array, arrayIndex);
    }

    public IEnumerator<Vector<T>> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    public int IndexOf(Vector<T> vector)
    {
        return _collection.IndexOf(vector);
    }

    public virtual void Insert(int index, Vector<T> vector)
    {
        _collection.Insert(index, vector);
    }

    public bool Remove(Vector<T> vector)
    {
        return _collection.Remove(vector);
    }

    public void RemoveAt(int index)
    {
        _collection.RemoveAt(index);
    }

    public T[][] ToMultidimensionalArray(bool pinned = false)
    {
        var array = new T[_collection.Count][];

        for (var i = 0; i < _collection.Count; i++)
        {
            array[i] = _collection[i].ToArray();
        }

        return array;
    }

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    public Tensor<T> ToTensor(bool pinned = false)
    {
        var maxLength = _collection.Max(v => v.Length);
        var totalLength = _collection.Count * maxLength;

        Span<nint> lengths = [_collection.Count, maxLength];

        var values = new T[totalLength];

        var offset = 0;

        for (var i = 0; i < _collection.Count; i++)
        {
            _collection[i].Span.CopyTo(values.AsSpan(offset));
            offset += maxLength;
        }

        return Tensor.Create(values, lengths, pinned);
    }
}
