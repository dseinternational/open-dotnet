// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using DSE.Open.Collections;

namespace DSE.Open.Specifications;

/// <summary>
/// A specification that determines if a candidate set of values satisfies
/// a comparison to another specified set of values.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class SetSpecification<TValue> : ICollectionSpecification<TValue>
{
    /// <summary>
    /// Initializes a new instance using the specified collection and comparison, with either a
    /// <see cref="FrozenSet{T}"/> or a <see cref="HashSet{T}"/> created, depending on the value
    /// of <paramref name="frozen"/>.
    /// </summary>
    /// <param name="set"></param>
    /// <param name="setComparison"></param>
    /// <param name="frozen"></param>
    public SetSpecification(IEnumerable<TValue> set, SetComparison setComparison, bool frozen = false)
        : this(frozen ? set.ToFrozenSet() : set.ToHashSet(), setComparison)
    {
    }

    /// <summary>
    /// Initializes a new instance using the specified set of values and comparison.
    /// </summary>
    /// <param name="set"></param>
    /// <param name="setComparison"></param>
    protected SetSpecification(IReadOnlySet<TValue> set, SetComparison setComparison)
    {
        SetComparison = setComparison;
        Set = set;
    }

    public IReadOnlySet<TValue> Set { get; }

    public SetComparison SetComparison { get; }

    public ValueTask<bool> IsSatisfiedByAsync(IEnumerable<TValue> candidate, CancellationToken cancellationToken = default)
    {
        if (candidate is FrozenSet<TValue> frozenSet)
        {
            return ValueTask.FromResult(IsSatisfiedBy(frozenSet));
        }

        return ValueTask.FromResult(IsSatisfiedBy(candidate.ToHashSet()));
    }

    private bool IsSatisfiedBy(HashSet<TValue> value)
    {
        Guard.IsNotNull(value);

        return SetComparison switch
        {
            SetComparison.Equal => value.SetEquals(Set),
            SetComparison.NotEqual => !value.SetEquals(Set),
            SetComparison.Subset => value.IsSubsetOf(Set),
            SetComparison.Superset => value.IsSupersetOf(Set),
            SetComparison.ProperSubset => value.IsProperSubsetOf(Set),
            SetComparison.ProperSuperset => value.IsProperSupersetOf(Set),
            _ => throw new InvalidOperationException(),
        };
    }

    private bool IsSatisfiedBy(FrozenSet<TValue> value)
    {
        Guard.IsNotNull(value);

        return SetComparison switch
        {
            SetComparison.Equal => value.SetEquals(Set),
            SetComparison.NotEqual => !value.SetEquals(Set),
            SetComparison.Subset => value.IsSubsetOf(Set),
            SetComparison.Superset => value.IsSupersetOf(Set),
            SetComparison.ProperSubset => value.IsProperSubsetOf(Set),
            SetComparison.ProperSuperset => value.IsProperSupersetOf(Set),
            _ => throw new InvalidOperationException(),
        };
    }
}
