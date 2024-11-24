// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;

namespace DSE.Open.Observations;

public class ObservationSet<TObs> : IList<TObs>
    where TObs : IObservation
{
    private readonly OrderedDictionary<int, TObs> _observations = [];

    public ObservationSet()
    {
        _observations = [];
    }

    public ObservationSet(int capacity)
    {
        _observations = new(capacity);
    }

    public ObservationSet(IEnumerable<TObs> observations)
    {
        ArgumentNullException.ThrowIfNull(observations);
        _observations = [];
        AddRange(observations);
    }

    public ObservationSet(IReadOnlyCollection<TObs> observations)
    {
        ArgumentNullException.ThrowIfNull(observations);
        _observations = new(observations.Count);
        AddRange(observations);
    }

    public int Count => _observations.Count;

    public bool IsReadOnly => false;

    public TObs this[int index]
    {
        get => _observations.GetAt(index).Value;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _observations.SetAt(index, GetKey(value), value);
        }
    }

    protected int GetKey(TObs observation)
    {
        ArgumentNullException.ThrowIfNull(observation);
        return observation.GetHashCode();
    }

    public bool Add(TObs item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var key = GetKey(item);

        if (_observations.ContainsKey(key))
        {
            return false;
        }

        _observations.Add(key, item);

        return true;
    }

    public void AddRange(IEnumerable<TObs> observations)
    {
        ArgumentNullException.ThrowIfNull(observations);

        foreach (var observation in observations)
        {
            _ = Add(observation);
        }
    }

    public void Clear()
    {
        _observations.Clear();
    }

    public bool Contains(TObs item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _observations.ContainsKey(GetKey(item));
    }

    public void CopyTo(TObs[] array, int arrayIndex)
    {
        _observations.Values.CopyTo(array, arrayIndex);
    }

    public IEnumerator<TObs> GetEnumerator()
    {
        return _observations.Values.GetEnumerator();
    }

    public bool Remove(TObs item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _observations.Remove(GetKey(item));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_observations).GetEnumerator();
    }

    public int IndexOf(TObs item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _observations.IndexOf(GetKey(item));
    }

    public void Insert(int index, TObs item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _observations.Insert(index, GetKey(item), item);
    }

    public void RemoveAt(int index)
    {
        _observations.RemoveAt(index);
    }

    void ICollection<TObs>.Add(TObs item)
    {
        throw new NotImplementedException();
    }
}
