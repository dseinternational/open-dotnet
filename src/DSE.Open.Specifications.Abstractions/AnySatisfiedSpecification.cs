// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AnySatisfiedSpecification<T> : AggregateSpecification<T>
{
    private readonly bool _asParallel;

    public AnySatisfiedSpecification(IEnumerable<ISpecification<T>> specifications, bool asParallel = false) : base(specifications)
    {
        _asParallel = asParallel;
    }

    public override bool IsSatisfiedBy(T candidate)
    {
        var enumerable = _asParallel ? Specifications.AsParallel() : Specifications.AsEnumerable();

        return enumerable.Any(s => s.IsSatisfiedBy(candidate));
    }
}
