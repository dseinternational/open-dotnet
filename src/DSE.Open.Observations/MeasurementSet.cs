// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

/// <summary>
/// Provides a set of observations where no two observations reference the same measurement.
/// </summary>
/// <typeparam name="TObs"></typeparam>
public class MeasurementSet<TObs> : IList<TObs>
    where TObs : IObservation
{
    private readonly OrderedDictionary<ulong, TObs> _observations = [];

    public MeasurementSet()
    {
        _observations = [];
    }

    public MeasurementSet(int capacity)
    {
        _observations = new(capacity);
    }

    public MeasurementSet(IEnumerable<TObs> observations)
    {
        ArgumentNullException.ThrowIfNull(observations);
        _observations = [];
        AddRange(observations);
    }

    public MeasurementSet(IReadOnlyCollection<TObs> observations)
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
            var key = GetKey(value);
            EnsureDoesNotContainKey(key);
            _observations.SetAt(index, key, value);
        }
    }

    protected ulong GetKey(TObs item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item.GetMeasurementHashCode();
    }

    protected void EnsureDoesNotContainKey(ulong key)
    {
        if (_observations.ContainsKey(key))
        {
            ThrowSameMeasurementExistsError();
        }
    }

    [DoesNotReturn]
    protected void ThrowSameMeasurementExistsError()
    {
        ThrowHelper.ThrowInvalidOperationException(
            $"An observation with the same measurement already exists in the {nameof(MeasurementSet)}.");
    }

    public void Add(TObs item)
    {
        if (TryAdd(item))
        {
            return;
        }

        ThrowSameMeasurementExistsError();
    }

    public bool TryAdd(TObs item)
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
        if (TryAddRange(observations))
        {
            return;
        }

        ThrowSameMeasurementExistsError();
    }

    public bool TryAddRange(IEnumerable<TObs> observations)
    {
        ArgumentNullException.ThrowIfNull(observations);

        List<KeyValuePair<ulong, TObs>> observationsToAdd = [];

        foreach (var observation in observations)
        {
            var key = GetKey(observation);

            if (_observations.ContainsKey(key))
            {
                return false;
            }

            observationsToAdd.Add(new(key, observation));
        }

        foreach (var observation in observationsToAdd)
        {
            _observations.Add(observation.Key, observation.Value);
        }

        return true;
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

    public bool ContainsKey(ulong measurementHash)
    {
        return _observations.ContainsKey(measurementHash);
    }

    public bool TryGetObservation(ulong measurementHash, [NotNullWhen(true)] out TObs observation)
    {
        return _observations.TryGetValue(measurementHash, out observation!);
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

public sealed class MeasurementSet<TObs, TValue> : MeasurementSet<TObs>
    where TObs : IObservation<TValue>
    where TValue : struct, IEquatable<TValue>
{
    public MeasurementSet()
    {
    }

    public MeasurementSet(int capacity) : base(capacity)
    {
    }

    public MeasurementSet(IEnumerable<TObs> observations) : base(observations)
    {
    }

    public MeasurementSet(IReadOnlyCollection<TObs> observations) : base(observations)
    {
    }
}

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

public sealed class MeasurementSet<TObs, TValue, TParam> : MeasurementSet<TObs>
    where TObs : IObservation<TValue, TParam>
    where TValue : struct, IEquatable<TValue>
    where TParam : IEquatable<TParam>
{
    public MeasurementSet()
    {
    }

    public MeasurementSet(int capacity) : base(capacity)
    {
    }

    public MeasurementSet(IEnumerable<TObs> observations) : base(observations)
    {
    }

    public MeasurementSet(IReadOnlyCollection<TObs> observations) : base(observations)
    {
    }
}

public sealed class MeasurementSet : MeasurementSet<Observation>
{
    public MeasurementSet()
    {
    }

    public MeasurementSet(int capacity) : base(capacity)
    {
    }

    public MeasurementSet(IEnumerable<Observation> observations) : base(observations)
    {
    }

    public MeasurementSet(IReadOnlyCollection<Observation> observations) : base(observations)
    {
    }
}
