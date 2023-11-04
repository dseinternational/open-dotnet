// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public sealed class PredicateSpecification<T> : ISpecification<T>
{
    private readonly Func<T, bool> _predicate;

    public PredicateSpecification(Func<T, bool> predicate)
    {
        Guard.IsNotNull(predicate);
        _predicate = predicate;
    }

    public ValueTask<bool> IsSatisfiedByAsync(T candidate, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_predicate(candidate));
    }
}
