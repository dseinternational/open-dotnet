// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AllSatisfiedSpecification<T> : AggregateSpecification<T>
{
    private readonly bool _asParallel;

    public AllSatisfiedSpecification(IEnumerable<ISpecification<T>> specifications, bool asParallel = false) : base(specifications)
    {
        _asParallel = asParallel;
    }

    public override bool IsSatisfiedBy(T candidate)
    {
        var enumerable = _asParallel ? Specifications.AsParallel() : Specifications.AsEnumerable();

        return enumerable.All(s => s.IsSatisfiedBy(candidate));
    }
}
