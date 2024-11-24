// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace DSE.Open.Observations;

/// <summary>
/// Provides a set of observations where no two observations reference the same measurement
/// and where observations that do reference the same measurement as an existing item in the
/// set are only added if they are more recent. 
/// </summary>
/// <typeparam name="TObs"></typeparam>
public class SnapshotSet<TObs> : ISet<TObs>
    where TObs : IObservation
{
    private readonly MeasurementSet<TObs> _observations;

    public SnapshotSet()
    {
        _observations = [];
    }

    public SnapshotSet(IEnumerable<TObs> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        _observations = collection is ICollection counted ? new(counted.Count) : new();

        foreach (var obs in collection)
        {
            _ = TryAdd(obs);
        }
    }

    public SnapshotSet(int capacity)
    {
        _observations = new(capacity);
    }

    public int Count => _observations.Count;

    public bool IsReadOnly => ((ICollection<TObs>)_observations).IsReadOnly;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool Add(TObs item)
    {
        return TryAdd(item);
    }

    /// <summary>
    /// Attempts to add a new observation to the set. The observation will be added if no existing
    /// observation references the same measure or if the observation is newer than the existing
    /// observation.
    /// </summary>
    /// <param name="observation">The observation to add.</param>
    /// <returns><see langword="true"/> if the observation is added, otherwise <see langword="false"/>.</returns>
    /// <remarks>See <see cref="Observation.GetMeasurementHashCode"/> for details of measurement equality.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="observation"/> is <see langword="null"/>.</exception>
    public bool TryAdd(TObs observation)
    {
        ArgumentNullException.ThrowIfNull(observation);

        if (_observations.TryGetValue(observation, out var existing))
        {
            if (existing.Time < observation.Time)
            {
                var removed = _observations.Remove(existing);
                Debug.Assert(removed);
                var added = _observations.Add(observation);
                Debug.Assert(added);
            }
            else
            {
                return false;
            }
        }

        return _observations.Add(observation);
    }

    public bool TryAddRange(IEnumerable<TObs> observations, out IReadOnlyList<TObs> notAdded)
    {
        ArgumentNullException.ThrowIfNull(observations);

        var notAddedList = new List<TObs>();

        foreach (var observation in observations)
        {
            if (!TryAdd(observation))
            {
                notAddedList.Add(observation);
            }
        }

        notAdded = notAddedList;
        return notAdded.Count == 0;
    }

    public void Clear()
    {
        _observations.Clear();
    }

    public bool Contains(TObs item)
    {
        return _observations.Contains(item);
    }

    public void CopyTo(TObs[] array, int arrayIndex)
    {
        _observations.CopyTo(array, arrayIndex);
    }

    public void ExceptWith(IEnumerable<TObs> other)
    {
        _observations.ExceptWith(other);
    }

    public IEnumerator<TObs> GetEnumerator()
    {
        return _observations.GetEnumerator();
    }

    public void IntersectWith(IEnumerable<TObs> other)
    {
        _observations.IntersectWith(other);
    }

    public bool IsProperSubsetOf(IEnumerable<TObs> other)
    {
        return _observations.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<TObs> other)
    {
        return _observations.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<TObs> other)
    {
        return _observations.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<TObs> other)
    {
        return _observations.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<TObs> other)
    {
        return _observations.Overlaps(other);
    }

    public bool Remove(TObs item)
    {
        return _observations.Remove(item);
    }

    public bool SetEquals(IEnumerable<TObs> other)
    {
        return _observations.SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<TObs> other)
    {
        _observations.SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<TObs> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        foreach (var obs in other)
        {
            _ = TryAdd(obs);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_observations).GetEnumerator();
    }

    void ICollection<TObs>.Add(TObs item)
    {
        _ = Add(item);
    }
}

/// <summary>
/// Provides a set of observations where no two observations reference the same measurement
/// and where observations that do reference the same measurement as an existing item in the
/// set are only added if they are more recent. 
/// </summary>
public sealed class SnapshotSet : SnapshotSet<Observation>
{
    public SnapshotSet()
    {
    }

    public SnapshotSet(IEnumerable<Observation> collection) : base(collection)
    {
    }

    public SnapshotSet(int capacity) : base(capacity)
    {
    }
}
