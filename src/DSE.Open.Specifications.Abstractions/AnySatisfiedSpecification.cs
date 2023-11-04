// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AnySatisfiedSpecification<T> : AggregateSpecification<T>
{
    public AnySatisfiedSpecification(IEnumerable<ISpecification<T>> specifications): base(specifications)
    {
    }

    public override bool IsSatisfiedBy(T candidate)
    {
        return Specifications.Any(s => s.IsSatisfiedBy(candidate));
    }
}
